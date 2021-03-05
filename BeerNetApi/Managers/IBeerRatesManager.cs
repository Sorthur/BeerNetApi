using System;
using System.Collections.Generic;
using System.Linq;
using BeerNetApi.Models;
using BeerNetApi.Models.Enums;

namespace BeerNetApi.Managers
{
    public interface IBeerRatesManager
    {
        void AddBeerRate(int beerId, string userId, string description, float rate);
        void EditBeerRate(int beerRateId, string description, float rate);
        bool DidUserRateBeer(int beerId, string userId);
        BeerRate GetBeerRate(int beerRateId);
        List<BeerRate> GetBeerRates(int beerId);
    }
}
