using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.Models.LibraryBranch
{
    public class LibraryBranchIndexModel
    {
        public IEnumerable<LibraryBranchDetailModel> Branches { get; set; }
    }
}
