using System;
using BeerNetApi.Models;

namespace BeerNetApi.Managers
{
    public interface IUserManager
    {
        PublicUserData GetUserData(string userId);
        void UpdateUserData(BeerNetUser user);
    }
}
