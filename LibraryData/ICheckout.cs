using LibraryData.Models;
using System;
using System.Collections.Generic;

namespace LibraryData
{
    public interface ICheckout
    {
        void Add(Checkout newCheckout);

        IEnumerable<Checkout> GetAll();
        IEnumerable<CheckoutHistory> GetCheckoutHistory(int id);
        IEnumerable<Hold> GetCurrentHolds(int id);

        Checkout GetById(int checkoutId);
        Checkout GetLatestCheckout(int assetId);

        string GetCurrentCheckoutPatron(int assetId);
        string GetCurrentHoldPatronName(int holdId);
        DateTime GetCurrentHoldPlaced(int holdId);
        bool IsCheckedOut(int Id);

        void CheckoutItem(int assetId, int libraryCardId);
        void CheckinItem(int assetId);
        void PlaceHold(int assetId, int LibraryCardId);
        void MarkLost(int assetId);
        void MarkFound(int assetId);

    }
}
