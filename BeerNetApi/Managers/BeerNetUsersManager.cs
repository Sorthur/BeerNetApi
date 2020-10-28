using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BeerNetApi.Data;
using BeerNetApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BeerNetApi.Managers
{
    public class BeerNetUsersManager : IBeerNetUsersManager
    {
        private readonly ApplicationDbContext _dbContext;
        public BeerNetUsersManager(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public BeerNetUser GetBeerNetUser(string id)
        {
            return _dbContext.BeerNetUsers
                .Include(b => b.BeerRates)
                    .ThenInclude(b => b.Beer)
                        .ThenInclude(b => b.Brewery)
                .FirstOrDefault(b => b.Id == id);
        }

        public BeerNetUser GetBeerNetUserByMail(string mail)
        {
            return _dbContext.BeerNetUsers
                .Include(b => b.BeerRates)
                    .ThenInclude(b => b.Beer)
                        .ThenInclude(b => b.Brewery)
                .FirstOrDefault(b => b.Email == mail);
        }

        public BeerNetUser GetBeerNetUserByLogin(string login)
        {
            return _dbContext.BeerNetUsers
                .Include(b => b.BeerRates)
                    .ThenInclude(b => b.Beer)
                        .ThenInclude(b => b.Brewery)
                .FirstOrDefault(b => b.Login == login);
        }

        public List<BeerNetUser> GetBeerNetUsers()
        {
            return _dbContext.BeerNetUsers
                .Include(b => b.BeerRates)
                    .ThenInclude(b => b.Beer)
                        .ThenInclude(b => b.Brewery)
                .ToList();
        }

        public List<BeerNetUser> GetBeerNetUsers(string loginPhrase)
        {
            return _dbContext.BeerNetUsers
                .Where(b => b.Login.ToLower().Contains(loginPhrase.ToLower()))
                .Include(b => b.BeerRates)
                    .ThenInclude(b => b.Beer)
                        .ThenInclude(b => b.Brewery)
                .ToList();
        }

        public void AddOrEditImage(string login, FileStream image)
        {
            var user = _dbContext.BeerNetUsers.FirstOrDefault(b => b.Login == login);
            user.ImageToBytes(image);
            _dbContext.SaveChanges();
        }
    }
}
