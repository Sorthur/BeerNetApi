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

        public void AddBeerRate(int beerId, string userId, string description, float rate)
        {
            var beer = _dbContext.Beers.FirstOrDefault(b => b.Id == beerId);
            var user = _dbContext.BeerNetUsers.FirstOrDefault(b => b.Id == userId);
            if (beer == null || user == null)
            {
                return;
            }

            int numberOfRates = _dbContext.Beers
                .Include(b => b.BeerRates)
                .FirstOrDefault(b => b.Id == beerId).BeerRates
                .Count();

            var beerRate = new BeerRate(user, beer, rate, description);
            float newAvgRating = beer.AverageRating * numberOfRates++;
            newAvgRating += beerRate.Rate;
            newAvgRating /= numberOfRates;
            beer.AverageRating = newAvgRating;
            _dbContext.BeerRates.Add(beerRate);

            _dbContext.SaveChanges();
        }

        public void RemoveBeerRate(int beerRateId, int beerId)
        {
            var beer = _dbContext.Beers.FirstOrDefault(b => b.Id == beerId);
            var beerRate = _dbContext.BeerRates.FirstOrDefault(b => b.Id == beerRateId);
            if (beer == null || beerRate == null)
            {
                return;
            }

            int numberOfRates = _dbContext.Beers
                .Include(b => b.BeerRates)
                .FirstOrDefault(b => b.Id == beerId).BeerRates
                .Count();

            float NewAvgRating = beer.AverageRating * numberOfRates--;
            NewAvgRating -= beerRate.Rate;
            NewAvgRating /= numberOfRates;
            beer.AverageRating = NewAvgRating;

            _dbContext.Remove(beerRate);
            _dbContext.SaveChanges();
        }

        public void EditBeerRate(int beerRateId, string description, float rate)
        {
            var originalBeerRate = _dbContext.BeerRates
                .Include(b => b.Beer)
                .Include(b => b.User)
                .FirstOrDefault(b => b.Id == beerRateId);
            var beer = _dbContext.Beers.FirstOrDefault(b => b.Id == originalBeerRate.Beer.Id);

            if (originalBeerRate == null || beer == null)
            {
                return;
            }

            int numberOfRates = _dbContext.Beers
                .Include(b => b.BeerRates)
                .FirstOrDefault(b => b.Id == beer.Id).BeerRates
                .Count();

            float NewAvgRating = beer.AverageRating * numberOfRates;
            NewAvgRating -= originalBeerRate.Rate;
            NewAvgRating += rate;
            NewAvgRating /= numberOfRates;
            beer.AverageRating = NewAvgRating;

            originalBeerRate.Rate = rate;
            originalBeerRate.Description = description;

            _dbContext.SaveChanges();
        }

        public bool DidUserRateBeer(int beerId, string userId)
        {
            var beerRate = _dbContext.BeerRates.FirstOrDefault(b => b.User.Id == userId && b.Beer.Id == beerId);
            if (beerRate != null)
            {
                return true;
            }
            return false;
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
