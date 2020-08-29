using System;
using System.Collections.Generic;
using BeerNet.Models;
using BeerNet.Models.Enums;

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
        void EditBeerRating(int beerId, string userMail, string beerRateDescription, float rate);
        List<Beer> AdvancedBeerSearch(string beerName, string breweryName, Country? country, BeerStyle? beerStyle, float? minExtract, float? maxExtract, float? minAbv, float? maxAbv);
    }
}
