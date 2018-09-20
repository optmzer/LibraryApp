using LibraryData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryData
{
    public interface ILibraryAsset
    {
        IEnumerable<LibraryAsset> GetAll();

        LibraryAsset GetById(int id);

        // ==============
        void AddNewAsset(LibraryAsset asset);

        string GetAuthorOrDirector(int id);

        string GetDeweyIndex(int id);

        string GetType(int id);

        string GetTytle(int id);

        string GetIsbn(int id);
        
        // ==============
        LibraryBranch GetCurrentLocation(int id);

    }
}
