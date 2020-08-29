using System;
using System.Collections.Generic;
using System.Linq;
using BeerNet.Data;
using BeerNet.Models;
using BeerNet.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

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
                .Include(b => b.Brewery)
                .Include(b => b.BeerRates)
                    .ThenInclude(b => b.User)
                .FirstOrDefault(b => b.Id == id);
        }

        public Beer GetBeer(string name)
        {
            return _dbContext.Beers
                .Include(b => b.Brewery)
                .Include(b => b.BeerRates)
                    .ThenInclude(b => b.User)
                .FirstOrDefault(b => b.Name == name);
        }

        public List<Beer> GetBeers()
        {
            return _dbContext.Beers
                .Include(b => b.Brewery)
                .Include(b => b.BeerRates)
                    .ThenInclude(b => b.User)
                .ToList();
        }

        public List<Beer> GetBeers(string namePhrase)
        {
            return _dbContext.Beers.Where(b => b.Name.ToLower()
                .Contains(namePhrase.ToLower()))
                .Include(b => b.Brewery)
                .Include(b => b.BeerRates)
                    .ThenInclude(b => b.User)
                .ToList();
        }

        public List<Beer> GetBeersWithBestAverageRating(int numberOfBeers, int minNumberOfRatings)
        {
            return _dbContext.Beers
                .Where(b => b.BeerRates.Count() >= minNumberOfRatings)
                .OrderByDescending(b => b.AverageRating)
                .Take(5)
                .Include(b => b.Brewery)
                .Include(b => b.BeerRates)
                    .ThenInclude(b => b.User)
                .ToList();
        }

        public List<Beer> GetBeersWithExtractWithingRange(float moreThan, float lessThan)
        {
            return _dbContext.Beers
                .Where(b => b.Extract >= moreThan && b.Extract <= lessThan)
                .Include(b => b.Brewery)
                .Include(b => b.BeerRates)
                    .ThenInclude(b => b.User)
                .ToList();
        }

        public List<Beer> GetBeersWithHigherExtract(float givenExtract)
        {
            return _dbContext.Beers
                .Where(b => b.Extract >= givenExtract)
                .Include(b => b.Brewery)
                .Include(b => b.BeerRates)
                    .ThenInclude(b => b.User)
                .ToList();
        }

        public List<Beer> GetBeersWithLowerExtract(float givenExtract)
        {
            return _dbContext.Beers
                .Where(b => b.Extract <= givenExtract)
                .Include(b => b.Brewery)
                .Include(b => b.BeerRates)
                    .ThenInclude(b => b.User)
                .ToList();
        }
        public void AddBeerRating(Beer beer, BeerRate beerRate)
        {
            double newAvgRating = beer.AverageRating * beer.BeerRates.Count();
            newAvgRating += beerRate.Rate;
            beer.BeerRates.Add(beerRate);
            newAvgRating /= beer.BeerRates.Count();
            beer.AverageRating = (float)newAvgRating;
            _dbContext.SaveChanges();
        }

        public void EditBeerRating(int beerId, string userMail, string beerRateDescription, float newRate)
        {
            var beer = GetBeer(beerId);
            var beerRate = beer.BeerRates.FirstOrDefault(r => r.User.Email == userMail);

            float newAvgRating = beer.AverageRating * beer.BeerRates.Count();
            newAvgRating -= beerRate.Rate;
            newAvgRating += newRate;
            newAvgRating /= beer.BeerRates.Count();
            beer.AverageRating = newAvgRating;

            beerRate.Rate = newRate;
            beerRate.Description = beerRateDescription;

            _dbContext.SaveChanges();
        }

        public List<Beer> AdvancedBeerSearch(string beerName, string breweryName, Country? country, BeerStyle? beerStyle, float? minExtract, float? maxExtract, float? minAbv, float? maxAbv)
        {
            var beers = _dbContext.Beers.Where(b =>
                (string.IsNullOrEmpty(beerName) || b.Name.Contains(beerName)) &&
                (string.IsNullOrEmpty(breweryName) || b.Brewery.Name.Contains(breweryName)) &&
                (country == null || b.Brewery.Country == country) &&
                (beerStyle == null || b.Style == beerStyle) &&
                (minExtract == null || b.Extract >= minExtract) &&
                (maxExtract == null || b.Extract <= maxExtract) &&
                (minAbv == null || b.Abv >= minAbv) &&
                (maxAbv == null || b.Abv <= maxAbv))
                .Include(b => b.Brewery)
                .Include(b => b.BeerRates)
                .ToList();

            return beers;
        }
    }
}
