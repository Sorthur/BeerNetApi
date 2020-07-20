using System;
using System.Collections.Generic;
using BeerNet.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace BeerNet.Models
{
    public class BeerNetUser : IdentityUser
    {        
        public string Login { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public Country Country { get; set; }
        public List<BeerRate> BeerRates { get; set; }

        public BeerNetUser() { }
    }
}
