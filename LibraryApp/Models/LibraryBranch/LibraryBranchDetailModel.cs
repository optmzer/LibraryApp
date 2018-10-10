using System.Collections.Generic;

namespace LibraryApp.Models.LibraryBranch
{
    public class LibraryBranchDetailModel
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public string OpenDate { get; set; }
        public string Telephone { get; set; }
        public bool isOpen { get; set; }
        public string Description { get; set; }
        public int NumberOfPatrons { get; set; }
        public int NumberOfAssets { get; set; }
        public decimal TotlaAssetValue { get; set; }
        public string ImageUrl { get; set; }
        public IEnumerable<string> BranchHours { get; set; }

    }
}
