using System.ComponentModel.DataAnnotations;

// Just a different type of asses shuch as a Book
namespace LibraryData.Models
{
    public class Video: LibraryAsset
    {
        [Required]
        public string Director { get; set; }
    }
}
