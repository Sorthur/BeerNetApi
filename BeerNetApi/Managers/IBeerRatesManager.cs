using System;
using System.Collections.Generic;
using System.Linq;
using BeerNetApi.Models;
using BeerNetApi.Models.Enums;

namespace BeerNetApi.Managers
{
    public interface IBeerRatesManager
    {
        void AddBeerRate(BeerRate beerRate, int beerId, string userId);
        void EditBeerRate(BeerRate editedBeerRate, int beerId, string userId);
        BeerRate GetBeerRate(int beerRateId);
        List<BeerRate> GetBeerRates(int beerId);
    }
}
