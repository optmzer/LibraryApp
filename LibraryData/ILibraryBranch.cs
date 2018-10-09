﻿using LibraryData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryData
{
    public interface ILibraryBranch
    {
        IEnumerable<LibraryBranch> GetAllBranches();
        IEnumerable<Patron> GetAllPatrons(int branchId);
        IEnumerable<LibraryAsset> GetAllAssets(int branchId);
        IEnumerable<string> GetBranchHours(int branchId);

        LibraryBranch Get(int branchId);
        void Add(LibraryBranch newBranch);
        bool IsBranchOpen(int branchId);
    }
}
