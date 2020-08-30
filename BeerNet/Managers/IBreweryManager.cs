using System;
using System.Collections.Generic;
using BeerNet.Models;
using BeerNet.Models.Enums;

namespace BeerNet.Managers
{
    public interface IBreweryManager
    {
        List<Brewery> GetBreweries();
        List<Brewery> GetBreweries(string namePhrase);
        List<Brewery> GetBreweries(Country country);
        Brewery GetBrewery(int id);
        Brewery GetBrewery(string name);
        List<Brewery> AdvancedBrewerySearch(string breweryName, Country? country);
    }
}
