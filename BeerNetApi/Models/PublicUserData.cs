using System;
using System.Collections.Generic;
using BeerNetApi.Models.Enums;

namespace BeerNetApi.Models
{
    public class PublicUserData
    {
        public string Login { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public Country? Country { get; set; }
        public List<BeerRate> BeerRates { get; set; }
        public byte[] Image { get; set; }

        public PublicUserData(BeerNetUser beerNetUser)
        {
            Login = beerNetUser.Login;
            Name = beerNetUser.Name;
            Surname = beerNetUser.Surname;
            Country = beerNetUser.Country;
            BeerRates= beerNetUser.BeerRates;
            Image = beerNetUser.Image;
        }
    }
}
