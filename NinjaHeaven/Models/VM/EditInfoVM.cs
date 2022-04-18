using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace NinjaHeaven.Models
{
    public class EditInfoVM
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(12, MinimumLength = 6)]
        public string Password { get; set; }

        [StringLength(12, MinimumLength = 6)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Gender { get; set; }
    }

}
