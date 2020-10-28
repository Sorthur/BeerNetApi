using System;
using System.Collections.Generic;
using System.Linq;
using BeerNetApi.Data;
using BeerNetApi.Models;
using BeerNetApi.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace BeerNetApi.Managers
{
    public class BreweriesManager : IBreweriesManager
    {
        private readonly ApplicationDbContext _dbContext;
        public BreweriesManager(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Brewery GetBrewery(int id)
        {
            return _dbContext.Breweries.Include(b => b.Beers).FirstOrDefault(b => b.Id == id);
        }

        public List<Brewery> GetBreweries(BreweryFilter breweryFilter)
        {
            if (breweryFilter.BreweriesToTake <= 0)
            {
                return null;
            }

            var breweries = _dbContext.Breweries
                .Skip(breweryFilter.BreweriesToSkip)
                .Take(breweryFilter.BreweriesToTake)
                .Where(b =>
                (string.IsNullOrEmpty(breweryFilter.BreweryName) || b.Name.Contains(breweryFilter.BreweryName)) &&
                (breweryFilter.Country == null || b.Country == breweryFilter.Country));

            if (breweryFilter.IncludeBeers == true)
            {
                return breweries.Include(b => b.Beers).ToList();
            }
            return breweries.ToList();

        }
    }
}
