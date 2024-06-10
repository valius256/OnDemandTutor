using OnDemandTutor.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDemandTutor.Models.Dtos.Register
{
    public class RegisterDtos
    {
        public string? FirstName { get; set; }
        public required string LastName { get; set; }
        public string? Phone { get; set; }
        [EmailAddress(ErrorMessage = "The email format is not valid")]
        public required string Email { get; set; }
        public string? Address { get; set; }
        public required string Password { get; set; }
        [Compare(nameof(Password), ErrorMessage = "The passwords didn't match.")]
        public required string ConfirmPassword { get; set; }
        public DateTime? Dob { get; set; }
    }
}
