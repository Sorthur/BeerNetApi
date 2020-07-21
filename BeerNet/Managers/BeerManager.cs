using System;
using System.Collections.Generic;
using System.Linq;
using BeerNet.Data;
using BeerNet.Models;
using Microsoft.EntityFrameworkCore;

namespace BeerNet.Managers
{
    public class BeerManager : IBeerManager
    {
        private readonly ApplicationDbContext _dbContext;
        public BeerManager(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Beer GetBeer(int id)
        {
            return _dbContext.Beers
                .Include(b => b.BeerRates)
                .FirstOrDefault(b => b.Id == id);
        }

        public Beer GetBeer(string name)
        {
            return _dbContext.Beers
                .Include(b => b.BeerRates)
                .FirstOrDefault(b => b.Name == name);
        }

        public List<Beer> GetBeers()
        {
            return _dbContext.Beers
                .Include(b => b.BeerRates)
                .ToList();
        }

        public List<Beer> GetBeers(string namePhrase)
        {
            return _dbContext.Beers.Where(b => b.Name.ToLower()
                .Contains(namePhrase.ToLower()))
                .Include(b => b.BeerRates)
                .ToList();
        }

        public List<Beer> GetBeersWithBestAverageRating(int numberOfBeers, int minNumberOfRatings)
        {
            return _dbContext.Beers
                .Where(b => b.BeerRates.Count() >= minNumberOfRatings)
                .OrderByDescending(b => b.AverageRating)
                .Take(5)
                .Include(b => b.BeerRates)
                .ToList();
        }

        public List<Beer> GetBeersWithExtractWithingRange(float moreThan, float lessThan)
        {
            return _dbContext.Beers
                .Where(b => b.Extract >= moreThan && b.Extract <= lessThan)
                .Include(b => b.BeerRates)
                .ToList();
        }

        public List<Beer> GetBeersWithHigherExtract(float givenExtract)
        {
            return _dbContext.Beers
                .Where(b => b.Extract >= givenExtract)
                .Include(b => b.BeerRates)
                .ToList();
        }

        public List<Beer> GetBeersWithLowerExtract(float givenExtract)
        {
            return _dbContext.Beers
                .Where(b => b.Extract <= givenExtract)
                .Include(b => b.BeerRates)
                .ToList();
        }
    }
}
