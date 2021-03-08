using System;
using System.Linq;
using BeerNetApi.Data;
using BeerNetApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BeerNetApi.Managers
{
    public class UserManager : IUserManager
    {
        private readonly ApplicationDbContext _dbContext;
        public UserManager(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public PublicUserData GetUserDataByLogin(string login)
        {
            var beerNetUser = _dbContext.BeerNetUsers
                .Include(u => u.BeerRates)
                .FirstOrDefault(u => u.Login == login);
            if (beerNetUser == null)
            {
                return null;
            }
            var publicUserData = new PublicUserData(beerNetUser);
            return publicUserData;
        }

        public PublicUserData GetUserDataByEmail(string email)
        {
            var beerNetUser = _dbContext.BeerNetUsers
                .Include(u => u.BeerRates)
                .FirstOrDefault(u => u.Email == email);
            if (beerNetUser == null)
            {
                return null;
            }
            var publicUserData = new PublicUserData(beerNetUser);
            return publicUserData;
        }

        public void UpdateUserData(PublicUserData updatedPublicUserData)
        {
            var beerNetUser = _dbContext.BeerNetUsers.FirstOrDefault(u => u.Id == updatedPublicUserData.UserId);
            if (beerNetUser == null)
            {
                return;
            }
            beerNetUser.UpdatePublicData(updatedPublicUserData);
            _dbContext.SaveChanges();
        }
    }
}
