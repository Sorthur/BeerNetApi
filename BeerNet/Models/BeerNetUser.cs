using System;
using System.Collections.Generic;
using System.IO;
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
        public byte[] Image { get; set; }


        public BeerNetUser()
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
            if (Image == null || Image.Length == 0)
            {
                // Transparent pixel
                string base64Data2 = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mNkYAAAAAYAAjCB0C8AAAAASUVORK5CYII=";
                return string.Format("data:image/jpg;base64,{0}", base64Data2);
            }
            string base64Data = Convert.ToBase64String(Image);
            return string.Format("data:image/jpg;base64,{0}", base64Data);
        }
    }
}
