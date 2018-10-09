using LibraryApp.Models.Patron;
using LibraryData;
using LibraryData.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.Controllers
{
    public class PatronController : Controller
    {
        private IPatron _patrons;

        public PatronController(IPatron patrons)
        {
            _patrons = patrons;
        }

        public IActionResult Index()
        {
            var allPatrons = _patrons.GetAll();

            var patronModels = allPatrons.Select(p => new PatronDetailModel
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                LibraryCardId = p.LibraryCard.Id,
                PatronAddress = p.Address,
                Telephone = p.Telephone,
                HomeLibraryBranch = p.HomeLibraryBranch.Name,
                OverdueFees = p.LibraryCard.Fees
            }).ToList();

            var model = new PatronIndexModel()
            {
                Patrons = patronModels
            };

            return View(model);
        }

        public IActionResult Detail(int id)
        {
            var patron = _patrons.Get(id);

            var model = new PatronDetailModel()
            {
                Id = patron.Id,
                LastName = patron.LastName,
                FirstName = patron.FirstName,
                PatronAddress = patron.Address,
                HomeLibraryBranch = patron.HomeLibraryBranch.Address,
                MemberSince = patron.LibraryCard.Created,
                OverdueFees = patron.LibraryCard.Fees,
                LibraryCardId = patron.LibraryCard.Id,
                Telephone = patron.Telephone,
                // ?? means if left part returns null create an empty List<Checkout>.
                AssetsCheckedOut = _patrons.GetCheckouts(id).ToList() ?? new List<Checkout>(),
                CheckoutHistory = _patrons.GetCheckoutHistory(id),
                Holds = _patrons.GetHolds(id)
            };

            return View(model);
        }
    }
}
