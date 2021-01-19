using BeerNetApi.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeerNetApi.Models.PostModels
{
    public class BreweryPostModel
    {
        [Required(ErrorMessage = "{0} is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        public Country? Country { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        public byte[] Image { get; set; }

        public static explicit operator Brewery(BreweryPostModel model)
        {
            return new Brewery
            {
                Beers = new List<Beer>(),
                Country = model.Country.Value,
                Description = model.Description,
                Image = model.Image,
                Name = model.Name
            };
        }
    }
}
