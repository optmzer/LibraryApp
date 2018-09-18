using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryData.Models
{
    public class CheckoutHistory
    {
        public int Id { get; set; }

        [Required]
        public LibraryAsset LibraryAsset { get; set; }

        [Required]
        public LibraryCard LibraryCard { get; set; }

        [Required]
        public DateTime CheckedOut { get; set; }

        // DateTime? - Makes the column explicitly Nullable
        // kind of optional if the book is never returned.
        public DateTime? CheckedIn { get; set; }
    }
}
