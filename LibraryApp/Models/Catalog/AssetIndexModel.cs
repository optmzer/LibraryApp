using System.Collections.Generic;

/**
 * It is a wrapper for AssetIndexListingModel. 
 */

namespace LibraryApp.Models.Catalog
{
    public class AssetIndexModel
    {
        public IEnumerable<AssetIndexListingModel> Assets { get; set; }
    }
}
