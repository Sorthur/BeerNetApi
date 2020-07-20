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
        Beer GetBeerWithHigherExtract(float givenExtract);
        Beer GetBeerWithLowerExtract(float givenExtract);
        Beer GetBeerWithExtractWithingRange(float moreThan, float lessThan);
    }
}
