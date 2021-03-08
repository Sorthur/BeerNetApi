using System;
using BeerNetApi.Models;

namespace BeerNetApi.Managers
{
    public interface IUserManager
    {
        /// <summary>
        /// Beer Rates included
        /// </summary>
        PublicUserData GetUserDataByLogin(string login);
        /// <summary>
        /// Beer Rates included
        /// </summary>
        PublicUserData GetUserDataByEmail(string email);
        /// <summary>
        /// Updates user's data whose ID is placed in given public PublicUserData
        /// </summary>
        void UpdateUserData(PublicUserData user);
    }
}
