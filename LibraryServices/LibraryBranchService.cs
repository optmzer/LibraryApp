using LibraryData;
using LibraryData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryServices
{
    public class LibraryBranchService : ILibraryBranch
    {
        private LibraryContext _context;

        public LibraryBranchService(LibraryContext context)
        {
            _context = context;
        }

        public void Add(LibraryBranch newBranch)
        {
            _context.Add(newBranch);
            _context.SaveChanges();
        }

        public LibraryBranch Get(int branchId)
        {
            return GetAllBranches().FirstOrDefault(b => b.Id == branchId);
        }

        public IEnumerable<LibraryAsset> GetAllAssets(int branchId)
        {
            return _context
                .LibraryBranches
                .Include(b => b.LibraryAssets)
                .FirstOrDefault(b => b.Id == branchId)
                .LibraryAssets;
        }

        public IEnumerable<LibraryBranch> GetAllBranches()
        {
            //returns all Patrons as well as LibraryAssets
            return _context
                .LibraryBranches
                .Include(b => b.Patrons) 
                .Include(b => b.LibraryAssets);
        }

        public IEnumerable<string> GetBranchHours(int branchId)
        {
            var hours = _context
                .BranchHours
                .Where(h => h.Branch.Id == branchId);

            return 
        }

        public IEnumerable<Patron> GetAllPatrons(int branchId)
        {
            throw new NotImplementedException();
        }

        public bool IsBranchOpen(int branchId)
        {
            throw new NotImplementedException();
        }
    }
}
