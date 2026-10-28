using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace AppForSEII.API.Models;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public ApplicationUser()
    {
    }
    public ApplicationUser(string id, string name, string surname, string userName, int telefonoUsuario, string dni)
    {
        Id = id;
        Name = name;
        Surname = surname;
        UserName = userName;
        Email = userName;
        TelefonoUsuario = telefonoUsuario;
        DNI = dni;
    }

    [StringLength(50)]
    public string? Name {get;set;}

    [StringLength(50)]
    public string? Surname {get;set;}

    [Phone]
    public int TelefonoUsuario {get; set;}

    [StringLength(9)]
    [RegularExpression(@"^[0-9]{8}[A-Za-z]$")]
    public string? DNI {get; set;}
}
