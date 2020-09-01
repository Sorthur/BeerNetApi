using System;
using System.Collections.Generic;
using System.IO;
using BeerNet.Models;

namespace BeerNet.Managers
{
    public interface IBeerNetUserManager
    {
        List<BeerNetUser> GetBeerNetUsers();
        List<BeerNetUser> GetBeerNetUsers(string loginPhrase);
        BeerNetUser GetBeerNetUser(string id);
        BeerNetUser GetBeerNetUserByMail(string mail);
        BeerNetUser GetBeerNetUserByLogin(string login);
        void AddOrEditImage(string login, FileStream image);
    }
}
