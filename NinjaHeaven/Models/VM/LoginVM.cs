using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace NinjaHeaven.Models
{
    public class LoginVM
    {
        [Remote(action: "VerifyUser", controller: "User")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

}
