using BeerNet.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeerNet.Validators
{
    public class LoginOrEmailExistenceValidator : ValidationAttribute
    {
        /// <returns>
        /// ErrorMessage if already login and mail doesn't exist in database.
        /// Success if login or mail are in database
        /// </returns>
        protected override ValidationResult IsValid(object loginOrEmail, ValidationContext validationContext)
        {
            var service = (UserManager<BeerNetUser>)validationContext
                .GetService(typeof(UserManager<BeerNetUser>));
            var loginOrEmailExist = service.Users.Any(u => u.Login == loginOrEmail.ToString());
            if (loginOrEmailExist)
            {
                return ValidationResult.Success;
            }

            loginOrEmailExist = service.Users.Any(u => u.Email == loginOrEmail.ToString());
            if (loginOrEmailExist)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage);
        }
    }
}

