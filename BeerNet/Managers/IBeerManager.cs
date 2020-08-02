using System;
using System.Collections.Generic;
using BeerNet.Models;

namespace BeerNet.Managers
{
    public interface IBeerManager
    {
        List<Beer> GetBeers();
        List<Beer> GetBeers(string namePhrase);
        Beer GetBeer(int id);
        Beer GetBeer(string name);
        List<Beer> GetBeersWithHigherExtract(float givenExtract);
        List<Beer> GetBeersWithLowerExtract(float givenExtract);
        List<Beer> GetBeersWithExtractWithingRange(float moreThan, float lessThan);
        List<Beer> GetBeersWithBestAverageRating(int numberOfBeers, int minNumberOfRatings);
        void AddBeerRating(Beer beer, BeerRate beerRate);
    }
}
