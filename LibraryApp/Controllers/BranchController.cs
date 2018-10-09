using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryData;
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
            //var model = 

            return View();
        }
    }
}