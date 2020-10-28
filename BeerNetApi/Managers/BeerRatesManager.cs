using BeerNetApi.Data;
using BeerNetApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BeerNetApi.Managers
{
    public class BeerRatesManager : IBeerRatesManager
    {
        private readonly ApplicationDbContext _dbContext;
        public BeerRatesManager(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddBeerRate(BeerRate beerRate, int beerId, string userId)
        {
            var beer = _dbContext.Beers.FirstOrDefault(b => b.Id == beerId);
            var user = _dbContext.BeerNetUsers.FirstOrDefault(b => b.Id == userId);
            int numberOfRates = _dbContext.Beers
                .Include(b => b.BeerRates)
                .FirstOrDefault(b => b.Id == beerId).BeerRates
                .Count();

            if (beer != null && user != null)
            {
                beerRate.Beer = beer;
                beerRate.User = user;
                float newAvgRating = beer.AverageRating * numberOfRates++;
                newAvgRating += beerRate.Rate;
                newAvgRating /= numberOfRates;
                beer.AverageRating = newAvgRating;
                _dbContext.BeerRates.Add(beerRate);

                _dbContext.SaveChanges();
            }
        }

        public void RemoveBeerRate(int beerRateId, int beerId)
        {
            var beer = _dbContext.Beers.FirstOrDefault(b => b.Id == beerId);
            var beerRate = _dbContext.BeerRates.FirstOrDefault(b => b.Id == beerRateId);
            int numberOfRates = _dbContext.Beers
                .Include(b => b.BeerRates)
                .FirstOrDefault(b => b.Id == beerId).BeerRates
                .Count();
            if (beer != null && beerRate != null)
            {
                float NewAvgRating = beer.AverageRating * numberOfRates--;
                NewAvgRating -= beerRate.Rate;
                NewAvgRating /= numberOfRates;
                beer.AverageRating = NewAvgRating;

                _dbContext.Remove(beerRate);
                _dbContext.SaveChanges();
            }
        }

        public void EditBeerRate(BeerRate editedBeerRate, int beerId, string userId)
        {
            var beer = _dbContext.Beers.FirstOrDefault(b => b.Id == beerId);
            int numberOfRates = _dbContext.Beers
                .Include(b => b.BeerRates)
                .FirstOrDefault(b => b.Id == beerId).BeerRates
                .Count();

            var oldBeerRate = _dbContext.BeerRates.FirstOrDefault(b => b.Id == editedBeerRate.Id);
            if (oldBeerRate != null && beer != null)
            {
                float NewAvgRating = beer.AverageRating * numberOfRates;
                NewAvgRating -= oldBeerRate.Rate;
                NewAvgRating += editedBeerRate.Rate;
                NewAvgRating /= numberOfRates;
                beer.AverageRating = NewAvgRating;

                oldBeerRate.Rate = editedBeerRate.Rate;
                oldBeerRate.Description = editedBeerRate.Description;

                _dbContext.SaveChanges();
            }
        }


        public BeerRate GetBeerRate(int beerRateId)
        {
            return _dbContext.BeerRates.FirstOrDefault(b => b.Id == beerRateId);
        }

        public List<BeerRate> GetBeerRates(int beerId)
        {
            throw new NotImplementedException();
        }
    }
}
