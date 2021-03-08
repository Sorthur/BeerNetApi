using System;
using System.ComponentModel.DataAnnotations;
using BeerNetApi.Models.Enums;

namespace BeerNetApi.Models.PostModels
{
    public class PublicUserDataPostModel
    {
        [Required]
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public Country? Country { get; set; }
        public byte[] Image { get; set; }
    }
}
