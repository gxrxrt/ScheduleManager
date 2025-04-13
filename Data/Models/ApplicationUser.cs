using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

public class ApplicationUser : IdentityUser
{
    [Required(ErrorMessage = "ФИО обязательно")]
    public string FullName { get; set; } // Можно добавить любые поля
}
