using BeerNetApi.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeerNetApi.Models.PostModels
{
    public class BeerPostModel
    {
        [Required(ErrorMessage = "{0} is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        public int? BreweryId { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        public BeerStyle? Style { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        public float? Extract { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        public float? Abv { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        public string Description { get; set; }
        public byte[] Image { get; set; }

        public static explicit operator Beer(BeerPostModel model)
        {
            return new Beer
            {
                Abv = model.Abv.Value,
                AverageRating = 0,
                BeerRates = new List<BeerRate>(),
                Brewery = null,
                Description = model.Description,
                Extract = model.Extract.Value,
                Image = model.Image,
                Name = model.Name,
                Style = model.Style.Value
            };
        }
    }
}