using System;
using System.Collections.Generic;
using System.Text;
using BeerNet.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BeerNet.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<BeerNetUser> BeerNetUsers{ get; set; }
        public DbSet<Beer> Beers { get; set; }
        public DbSet<Brewery> Breweries { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
