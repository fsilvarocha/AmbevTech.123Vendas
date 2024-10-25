using System.ComponentModel.DataAnnotations;

namespace AmbevTech._123Vendas.API.Account
{
    public class RegisterUserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
