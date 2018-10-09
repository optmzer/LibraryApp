using LibraryData;
using LibraryData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

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

            return DataHelpers.BusinessHoursToDateString(hours);
        }

        public IEnumerable<Patron> GetAllPatrons(int branchId)
        {
            return _context
                .LibraryBranches
                .Include(b => b.Patrons)
                .FirstOrDefault(b => b.Id == branchId)
                .Patrons;
        }

        public bool IsBranchOpen(int branchId)
        {
            var timeNow = DateTime.Now;
            var currentTimeHour = timeNow.Hour;
            // Because DB is set for days of the week starting from 
            //1 to 7 but .DayOfWeek strats 0 to 6. Where 0 = Sunday
            var currentDayOfWeek = (int)timeNow.DayOfWeek + 1;
            
            // Selects hours for a particular branch
            var hours = _context
                .BranchHours
                .Where(h => h.Branch.Id == branchId);

            // Selects only the line with todays hours based on currentDayOfWeek.
            var todaysHours = hours.FirstOrDefault(h => h.DayOfWeek == currentDayOfWeek);

            return currentTimeHour > todaysHours.OpenTime 
                && currentTimeHour < todaysHours.CloseTime;
        }
    }
}
