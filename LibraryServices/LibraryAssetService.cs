using LibraryData;
using LibraryData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryServices
{
    public class LibraryAssetService : ILibraryAsset
    {
        private LibraryContext _context;

        public LibraryAssetService(LibraryContext context)
        {
            _context = context;
        }

        public void AddNewAsset(LibraryAsset asset)
        {
            /**
             * Entity framwork handls all logic for us
             * all we need to do is to call _context.Add() method
             * and EntityFramwork will figure out where to stick it.
             */
            _context.Add(asset);
            _context.SaveChanges(); // commits changes to DB.
        }

        public IEnumerable<LibraryAsset> GetAll()
        { /**
           * Returns all LibraryAssets from DB
           * including its Status
           * and Location
           */
            return _context.LibraryAssets
                .Include(asset => asset.Status)
                .Include(asset => asset.Location);
        }

        public LibraryAsset GetById(int id)
        { /**
           * .FirstOrDefault() is used to avoid Null reference exception
           * as .First() will return Null if nothing matches our query.
           * 
           * the same as 
           * return GetAll().FirstOrDefault(asset => asset.Id == id);
           */
            return _context.LibraryAssets
                .Include(asset => asset.Status)
                .Include(asset => asset.Location)
                .FirstOrDefault(asset => asset.Id == id);
        }

        public LibraryBranch GetCurrentLocation(int id)
        { // or symply 
            // return GetById(id).Location;
            return _context.LibraryAssets.FirstOrDefault(asset => asset.Id == id).Location;
        }

        public string GetDeweyIndex(int id)
        { // Only return Dewey index when asset is a type of Book
            // As video does not have dewey index by the setup.
            if(_context.Books.Any(book => book.Id == id))
            {
                return _context.Books.FirstOrDefault(book => book.Id == id).DeweyIndex;
            }
           
            return "";
        }   

        public string GetIsbn(int id)
        {
            if (_context.Books.Any(book => book.Id == id))
            {
                return _context.Books.FirstOrDefault(book => book.Id == id).ISBN;
            }

            return "";
        }

        public string GetTytle(int id)
        {
            return _context.LibraryAssets
                .FirstOrDefault(asset => asset.Id == id)
                .Title;
        }

        public string GetType(int id)
        {
            return _context.LibraryAssets
                .FirstOrDefault(asset => asset.Id == id)
                .GetType()
                .ToString();
        }

        public string GetAuthorOrDirector(int id)
        {
            var isBook = _context.LibraryAssets.OfType<Book>()
                .Where(asset => asset.Id == id).Any();

            //var isVideo = _context.LibraryAssets.OfType<Video>()
             //   .Where(asset => asset.Id == id).Any();
            /**
             * Tertiary operator + Null coalescing operator:
             * True ? thisIfTrue : thisIfFalse ?? "Unknown"
             * (If for some reason tertiary operator returns Null coalescing operator will return "Unknown"
             */
            return isBook
               ?
               _context.Books.FirstOrDefault(book => book.Id == id).Author
               :
               _context.Videos.FirstOrDefault(video => video.Id == id).Director
               ??
               "Author/Director Unknown";
        }
    }
}
