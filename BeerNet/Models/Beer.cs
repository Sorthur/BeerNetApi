using System;
using System.Collections.Generic;
using System.IO;
using BeerNet.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace BeerNet.Models
{
    public class Beer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Brewery Brewery { get; set; }
        public BeerStyle Style { get; set; }
        public float Extract { get; set; }
        public float Abv { get; set; }
        public string Description { get; set; }
        public List<BeerRate> BeerRates { get; set; }
        public float AverageRating { get; set; }
        public byte[] Image { get; set; }

        public Beer()
        {
            BeerRates = new List<BeerRate>();
        }

        public void ImageToBytes(FileStream image)
        {
            using (var ms = new MemoryStream())
            {
                image.CopyTo(ms);
                Image = ms.ToArray();
            }
        }

        public string GetImage()
        {
            string base64Data = Convert.ToBase64String(Image);
            return string.Format("data:image/jpg;base64,{0}", base64Data);
        }
    }
}
