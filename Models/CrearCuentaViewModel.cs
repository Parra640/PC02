using System.ComponentModel.DataAnnotations;

namespace PC02.Models
{
    public class CrearCuentaViewModel
    {
        [Required]
        [Display(Name ="Correo Electrónico")]
        public string Correo{get; set;}    

        [Required]
        [Display(Name="Contraseña")]
        [DataType(DataType.Password)]
        public string Password1 { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name="Confirmar Contraseña")]
        [Compare("Password1", ErrorMessage = "Las contraseñas no coinciden")]
        public string Password2 { get; set; }
    }
}