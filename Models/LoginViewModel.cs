namespace PC02.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Correo Electrónico")]
        public string Correo{get; set;}

        [Required]
        [Display(Name = "Contraseña")]
        [Datatype(Datatype.Password)]
        public string Password{get; set;}
    }
}