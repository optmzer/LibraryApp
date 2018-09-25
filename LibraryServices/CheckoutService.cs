using LibraryData;
using LibraryData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryServices
{

    public class CheckoutService : ICheckout
    {
        private LibraryContext _context;

        public CheckoutService(LibraryContext context)
        {
            _context = context;
        }

        public void Add(Checkout newCheckout)
        {
            _context.Add(newCheckout);
            _context.SaveChanges();
        }

        public IEnumerable<Checkout> GetAll()
        {
            return _context.Checkouts;
        }

        public Checkout GetById(int checkoutId)
        {
            return GetAll().FirstOrDefault(checkout => checkout.Id == checkoutId);
        }

        public IEnumerable<CheckoutHistory> GetCheckoutHistory(int id)
        {
            return _context.CheckoutHistories
                .Include(hist => hist.LibraryAsset)
                .Include(hist => hist.LibraryCard)
                .Where(hist => hist.LibraryAsset.Id == id);
        }

        public IEnumerable<Hold> GetCurrentHolds(int id)
        {
            return _context.Holds
                .Include(hist => hist.LibraryAsset)
                .Where(hist => hist.LibraryAsset.Id == id);
        }

        public Checkout GetLatestCheckout(int assetId)
        {
            return _context.Checkouts
                .Where(checkout => checkout.LibraryAsset.Id == assetId)
                .OrderByDescending(checkout => checkout.Since)
                .FirstOrDefault();
        }

        public void MarkFound(int assetId)
        {
            var now = DateTime.Now;
            
            UpdateAssetStatus(assetId, "Available");
            RemoveExistingCheckouts(assetId);
            CloseExistingCheckoutHistory(assetId, now);

            _context.SaveChanges();
        }

        private void UpdateAssetStatus(int assetId, string assetStatus)
        {
            var item = _context.LibraryAssets
                .FirstOrDefault(asset => asset.Id == assetId);
            // Mark item for an Update. So EntityFramework locks it for Update
            _context.Update(item);

            item.Status = _context.Status
                .FirstOrDefault(status => status.Name == assetStatus);
        }

        private void CloseExistingCheckoutHistory(int assetId, DateTime now)
        {
            // Close any existing checkout history
            // && hist.CheckedIn == null - It has to be opened checkout
            var history = _context.CheckoutHistories
                .FirstOrDefault(hist =>
                hist.LibraryAsset.Id == assetId && hist.CheckedIn == null
                );

            if (history != null)
            {
                _context.Update(history);
                history.CheckedIn = now;
            }
        }

        private void RemoveExistingCheckouts(int assetId)
        {
            // remove any existing checkouts on the item
            var checkout = _context.Checkouts
                .FirstOrDefault(co => co.LibraryAsset.Id == assetId);

            if (checkout != null)
            {
                // Tracking Item for deletion not deleted until
                // _context.SaveChanges();
                _context.Remove(checkout);
            }
        }

        public void MarkLost(int assetId)
        {
            var item = _context
                .LibraryAssets
                .FirstOrDefault(asset => asset.Id == assetId);

            UpdateAssetStatus(assetId, "Lost");
            _context.SaveChanges();
        }

        public void CheckinItem(int assetId, int libraryCardId)
        {
            var now = DateTime.Now;
            var item = _context.LibraryAssets
                        .FirstOrDefault(asset => asset.Id == assetId);

            // We do not need to use _context.Update(item);
            // here as UpdateAssetStatus(assetId, "Available");
            // already does it for us.
            // Remove any existing checkouts on the item
            RemoveExistingCheckouts(assetId);

            // close any existing CheckoutHistory on the item
            CloseExistingCheckoutHistory(assetId, now);

            // look for existing holds on the item
            var currentHolds = _context.Holds
                .Include(hold => hold.LibraryAsset)
                .Include(hold => hold.LibraryCard)
                .Where(hold => hold.LibraryAsset.Id == assetId);

            // if there are holds, checkout the item to the 
            // LibraryCard with the earliest hold.
            if (currentHolds.Any())
            {
                CheckoutToEarliestHold(assetId, currentHolds);
            }
            // else update the item to available.
            UpdateAssetStatus(assetId, "Available");
            _context.SaveChanges();
        }

        private void CheckoutToEarliestHold(int assetId, IQueryable<Hold> currentHolds)
        {
            var earliestHold = currentHolds
                .OrderBy(hold => hold.HoldPlaced).FirstOrDefault();
            var card = earliestHold.LibraryCard;

            _context.Remove(earliestHold);
            _context.SaveChanges();

            CheckoutItem(assetId, card.Id);
        }

        public void CheckoutItem(int assetId, int libraryCardId)
        {
            var now = DateTime.Now;

            if (IsCheckedout(assetId))
            {
                // Might add some logic to notify the user
                return;
            }

            var item = _context.LibraryAssets
                .FirstOrDefault(asset => asset.Id == assetId);

            UpdateAssetStatus(assetId, "Checked Out");

            var libraryCard = _context.LibraryCards.
                Include(card => card.Checkouts).
                FirstOrDefault(card => card.Id == libraryCardId);

            var checkout = new Checkout
            {
                LibraryAsset = item,
                LibraryCard = libraryCard,
                Since = now,
                Until = GetDefaultCheckoutTime(now)
            };

            _context.Add(checkout);

            var checkoutHistory = new CheckoutHistory
            {
                CheckedOut = now,
                LibraryAsset = item,
                LibraryCard = libraryCard
            };

            _context.Add(checkoutHistory);
            _context.SaveChanges();
        }

        private DateTime GetDefaultCheckoutTime(DateTime now)
        {
            // Allows a book/video to be taken from library for 30 days.
            return now.AddDays(30);
        }

        private bool IsCheckedout(int assetId)
        {
            // Returns true if any checkout exists
            // else false.
            return _context
                .Checkouts
                .Where(co => co.LibraryAsset.Id == assetId)
                .Any();
        }

        public void PlaceHold(int assetId, int libraryCardId)
        {
            var now = DateTime.Now;

            var asset = _context
                .LibraryAssets
                .FirstOrDefault(a => a.Id == assetId);

            var card = _context.
                LibraryCards.
                FirstOrDefault(c => c.Id == libraryCardId);

            if(asset.Status.Name == "Available")
            {
                UpdateAssetStatus(assetId, "On Hold");
            }

            var hold = new Hold
            {
                HoldPlaced = now,
                LibraryAsset = asset,
                LibraryCard = card
            };

            _context.Add(hold);
            _context.SaveChanges();
        }

        public string GetCurrentHoldPatronName(int holdId)
        {
            var patronName = "Not Found";

            var hold = _context
                .Holds
                .Include(h => h.LibraryAsset)
                .Include(h => h.LibraryCard)
                .FirstOrDefault(h => h.Id == holdId);

            if( hold != null)
            {
                var cardId = hold.LibraryCard.Id;

                var patron = _context
                    .Patrons
                    .Include(p => p.LibraryCard)
                    .FirstOrDefault(p => p.LibraryCard.Id == cardId);
                patronName = patron.FirstName + " " + patron.LastName;
            }

            return patronName;
        }

        public DateTime GetCurrentHoldPlaced(int holdId)
        {
            return _context
                .Holds
                .Include(h => h.LibraryAsset)
                .Include(h => h.LibraryCard)
                .FirstOrDefault(h => h.Id == holdId)
                .HoldPlaced;
        }

        public string GetCurrentCheckoutPatron(int assetId)
        {
            var checkoutPatronName = "";
            var checkout = GetCheckoutByAssetId(assetId);

            if ( checkout != null)
            {
                var cardId = checkout.LibraryCard.Id;
                var patron = _context
                    .Patrons
                    .Include(p => p.LibraryCard)
                    .FirstOrDefault(p => p.LibraryCard.Id == cardId);
                checkoutPatronName = patron.FirstName + " " + patron.LastName;
            }

            return checkoutPatronName;
        }

        private Checkout GetCheckoutByAssetId(int assetId)
        {
            return _context
                .Checkouts
                .Include(co => co.LibraryAsset)
                .Include(co => co.LibraryCard)
                .FirstOrDefault(co => co.LibraryAsset.Id == assetId);
        }
    }
}
