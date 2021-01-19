using System;
using System.Collections.Generic;
using BeerNetApi.Models;
using BeerNetApi.Models.Enums;

namespace BeerNetApi.Managers
{
    public interface IBreweriesManager
    {
        Brewery GetBrewery(int id);
        void AddBrewery(Brewery brewery);
        List<Brewery> GetBreweries(BreweryFilter breweryFilter);
    }
}
