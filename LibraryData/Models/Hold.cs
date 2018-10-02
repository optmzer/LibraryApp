using System;

namespace LibraryData.Models
{
    public class Hold
    {
        public int Id { get; set; }
        // virtual == Foreighn Key
        public virtual LibraryAsset LibraryAsset { get; set; }
        public virtual LibraryCard LibraryCard { get; set; }
        public DateTime HoldPlaced { get; set; }
    }
}
