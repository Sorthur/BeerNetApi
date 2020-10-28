using System;
using System.Collections.Generic;
using System.IO;
using BeerNetApi.Models;

namespace BeerNetApi.Managers
{
    public interface IBeerNetUsersManager
    {
        List<BeerNetUser> GetBeerNetUsers();
        List<BeerNetUser> GetBeerNetUsers(string loginPhrase);
        BeerNetUser GetBeerNetUser(string id);
        BeerNetUser GetBeerNetUserByMail(string mail);
        BeerNetUser GetBeerNetUserByLogin(string login);
        void AddOrEditImage(string login, FileStream image);
    }
}
