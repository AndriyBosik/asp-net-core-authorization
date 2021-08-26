using System.ComponentModel.DataAnnotations;

namespace AuthorizationExample.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }
        
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords are not equal")]
        public string ConfirmPassword { get; set; }
    }
}