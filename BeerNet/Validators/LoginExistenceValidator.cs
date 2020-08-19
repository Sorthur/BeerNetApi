using BeerNet.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeerNet.Validators
{
    public class LoginExistenceValidator : ValidationAttribute
    {
        /// <returns>
        /// ErrorMessage if already login exists in database.
        /// Success if login is free
        /// </returns>
        protected override ValidationResult IsValid(object login, ValidationContext validationContext)
        {
            var service = (UserManager<BeerNetUser>)validationContext
                .GetService(typeof(UserManager<BeerNetUser>));
            var loginExists = service.Users.Any(u => u.Login == login.ToString());
            if (loginExists)
            {
                return new ValidationResult(ErrorMessage);
            }
            return ValidationResult.Success;
        }
    }
}
