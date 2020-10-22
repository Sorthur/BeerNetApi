using System;
using System.Collections.Generic;
using BeerNetApi.Models;
using BeerNetApi.Models.Enums;

namespace BeerNetApi.Managers
{
    public interface IBeersManager
    {
        int GetNumberOfBeers(BeerFilter beerFilter);
        Beer GetBeer(int id);
        List<Beer> GetBeers(BeerFilter beerFilter);
        List<Beer> GetTopBeers(int numberOfBeers, int minNumberOfRatings);
        void DodajListeLosowychPiw();
    }
}
