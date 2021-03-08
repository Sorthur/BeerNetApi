using System;
using System.Collections.Generic;
using BeerNetApi.Models.Enums;
using BeerNetApi.Models.PostModels;

namespace BeerNetApi.Models
{
    public class PublicUserData
    {
        public string UserId { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public Country? Country { get; set; }
        public List<BeerRate> BeerRates { get; set; }
        public byte[] Image { get; set; }

        public PublicUserData() { }

        public PublicUserData(BeerNetUser beerNetUser)
        {
            UserId = beerNetUser.Id;
            Login = beerNetUser.Login;
            Name = beerNetUser.Name;
            Surname = beerNetUser.Surname;
            Country = beerNetUser.Country;
            BeerRates= beerNetUser.BeerRates;
            Image = beerNetUser.Image;
        }

        public PublicUserData(PublicUserDataPostModel publicUserDataPostModel)
        {
            UserId = publicUserDataPostModel.UserId;
            Name = publicUserDataPostModel.Name;
            Surname = publicUserDataPostModel.Surname;
            Country = publicUserDataPostModel.Country;
            Image = publicUserDataPostModel.Image;
        }
    }
}
