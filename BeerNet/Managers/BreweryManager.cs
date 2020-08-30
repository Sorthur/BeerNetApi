using System;
using System.Collections.Generic;
using System.Linq;
using BeerNet.Data;
using BeerNet.Models;
using BeerNet.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace BeerNet.Managers
{
    public class BreweryManager : IBreweryManager
    {
        private readonly ApplicationDbContext _dbContext;
        public BreweryManager(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Brewery> GetBreweries()
        {
            return _dbContext.Breweries
                .Include(b => b.Beers)
                .ToList();
        }

        public List<Brewery> GetBreweries(string namePhrase)
        {
            return _dbContext.Breweries
                .Where(b => b.Name.ToLower().Contains(namePhrase.ToLower()))
                .Include(b => b.Beers)
                .ToList();
        }

        public List<Brewery> GetBreweries(Country country)
        {
            return _dbContext.Breweries
                .Where(b => b.Country == country)
                .Include(b => b.Beers)
                .ToList();
        }

        public Brewery GetBrewery(int id)
        {
            return _dbContext.Breweries
                .Include(b => b.Beers)
                .FirstOrDefault(b => b.Id == id);
        }

        public Brewery GetBrewery(string name)
        {
            return _dbContext.Breweries
                .Include(b => b.Beers)
                .FirstOrDefault(b => b.Name == name);
        }
        public List<Brewery> AdvancedBrewerySearch(string breweryName, Country? country)
        {
            return _dbContext.Breweries.Where(b =>
            (string.IsNullOrEmpty(breweryName) || b.Name.Contains(breweryName) &&
            country == null || b.Country == country))
            .Include(b => b.Beers)
            .ToList();
        }
    }
}
