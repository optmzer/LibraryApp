using LibraryData.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace LibraryData
{
    public class LibraryContext: DbContext
    {
        //Passing options from LibraryContext constructor to base constructor
        public LibraryContext(DbContextOptions options): base(options)  { }

        public DbSet<Patron> Patrons { get; set; }

    }
}
