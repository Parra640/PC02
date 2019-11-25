
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;

namespace PC02.Models
{
    public class Usuario : IdentityUser
    {
        [Required]
        [Display(Name = "Correo Electrónico")]
        public string Correo { get; set; }
        
    }
}