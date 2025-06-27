using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Kullanıcı Adı")]
        public string? UserName { get; set; }

        [Required]
        [Display(Name = "Ad Soyad")]
        public string? Name { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 6, ErrorMessage = "{0} alanı en az {2} karakter, en fazla {1} karakter olmalıdır.")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string? Password { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Şifreniz eşleşmiyor.")]
        [Display(Name = "Şifre Tekrar")]
        public string? ConfirmPassword { get; set; }
    }
}