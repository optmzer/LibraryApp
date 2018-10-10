using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryApp.Models.LibraryBranch;
using LibraryData;
using LibraryServices;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Controllers
{
    public class BranchController : Controller
    {
        private ILibraryBranch _branch;

        public BranchController(ILibraryBranch branch)
        {
            _branch = branch;
        }

        public IActionResult Index()
        {
            var allBranches = _branch
                .GetAllBranches()
                .Select(branch => new LibraryBranchDetailModel
                {
                    Id = branch.Id,
                    Name = branch.Name,
                    isOpen = _branch.IsBranchOpen(branch.Id),
                    NumberOfAssets = _branch.GetAllAssets(branch.Id).Count(),
                    NumberOfPatrons = _branch.GetAllPatrons(branch.Id).Count(),
                    Address = branch.Address,
                    Telephone = branch.Telephone
                });

            var model = new LibraryBranchIndexModel()
            {
                Branches = allBranches
            };

            return View(model);
        }

        public IActionResult Detail(int id)
        {
            var branch = _branch.Get(id);

            var model = new LibraryBranchDetailModel()
            {
                Id = branch.Id,
                Name = branch.Name,
                Address = branch.Address,
                Telephone = branch.Telephone,
                OpenDate = branch.OpenDate.ToString("dd-MM-yyyy"),
                NumberOfAssets = _branch.GetAllAssets(id).Count(),
                NumberOfPatrons = _branch.GetAllPatrons(id).Count(),
                TotlaAssetValue = _branch.GetAllAssets(id).Sum(asset => asset.Cost),
                ImageUrl = branch.ImageUrl,
                BranchHours = _branch.GetBranchHours(id)
            };

            return View(model);
        }

    }
}