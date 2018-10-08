using LibraryData;
using LibraryData.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace LibraryServices
{
    public class PatronService : IPatron
    {
        private LibraryContext _context;

        public PatronService(LibraryContext context)
        {
            _context = context;
        }

        public void Add(Patron newPatron)
        {
            _context.Add(newPatron);
            _context.SaveChanges();
        }

        public Patron Get(int id)
        {
            return GetAll().FirstOrDefault(patron => patron.Id == id);
        }

        /**
         * Returns Patron with LibraryCard and HomeLibraryBranch
         */ 
        public IEnumerable<Patron> GetAll()
        {
            return _context
                .Patrons
                .Include(patron => patron.LibraryCard)
                .Include(patron => patron.HomeLibraryBranch);
        }


        public IEnumerable<CheckoutHistory> GetCheckoutHistory(int patronId)
        {
            var cardId = Get(patronId).LibraryCard.Id;

            return _context
                .CheckoutHistories
                .Include(hist => hist.LibraryCard)
                .Include(hist => hist.LibraryAsset)
                .Where(hist => hist.LibraryCard.Id == cardId)
                .OrderByDescending(hist => hist.CheckedOut);
        }

        public IEnumerable<Checkout> GetCheckouts(int patronId)
        {
            var cardId = Get(patronId).LibraryCard.Id;

            return _context
                .Checkouts
                .Include(co => co.LibraryCard)
                .Include(co => co.LibraryAsset)
                .Where(co => co.LibraryCard.Id == cardId);
        }

        public IEnumerable<Hold> GetHolds(int patronId)
        {
            var cardId = Get(patronId).LibraryCard.Id;

            return _context
                .Holds
                .Include(hold => hold.LibraryCard)
                .Include(hold => hold.LibraryAsset)
                .Where(hold => hold.LibraryCard.Id == cardId)
                .OrderByDescending(hold => hold.HoldPlaced);
        }
    }
}
