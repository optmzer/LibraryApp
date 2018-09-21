using LibraryApp.Models.Catalog;
using LibraryData;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace LibraryApp.Controllers
{
    public class CatologController: Controller
    {
        private ILibraryAsset _assets;

        public CatologController(ILibraryAsset assets)
        {
            _assets = assets;
        }

        public IActionResult Index()
        {
            var assetModels = _assets.GetAll();

            // Maps data from assetModels to anonimus class
            // AssetIndexListingModel one to one.
            var listingResults = assetModels
                .Select(result => new AssetIndexListingModel {
                    Id = result.Id,
                    ImageUrl = result.ImageUrl,
                    AuthorOrDirector = _assets.GetAuthorOrDirector(result.Id),
                    DeweyCallNumber = _assets.GetDeweyIndex(result.Id),
                    Title = result.Title,
                    Type = _assets.GetType(result.Id)
                });

            var model = new AssetIndexModel()
            {
                Assets = listingResults
            };

            return View(model);
        }

    }
}
