using System;
using System.Collections.Generic;
using BeerNet.Models;

namespace BeerNet.Managers
{
    public interface IBeerNetUserManager
    {
        List<BeerNetUser> GetBeerNetUsers();
        List<BeerNetUser> GetBeerNetUsers(string loginPhrase);
        BeerNetUser GetBeerNetUser(int id);
        BeerNetUser GetBeerNetUser(string mail);
        BeerNetUser GetBeerNetUserByLogin(string login);
    }
}
