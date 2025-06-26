using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 6, ErrorMessage = "{0} alanı en az {2} karakter, en fazla {1} karakter olmalıdır.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}