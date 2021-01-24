using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeerNetApi.Models.PostModels
{
    public class BeerRatePostModel
    {
        [Required(ErrorMessage = "{0} is required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [Range(0.0, 5.0, ErrorMessage = "Value for {0} must be betwwen {1} and {2}")]
        public float? Rate { get; set; }
    }
}
