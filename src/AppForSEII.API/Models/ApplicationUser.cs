using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AppForSEII.API.Models
{
public class ApplicationUser : IdentityUser
{
public ApplicationUser()
{
}

public ApplicationUser(
string id,
string name,
string surname,
string dni,
string sex,
int age)
{
Id = id;
Name = name;
Surname = surname;
Sex = sex;
Age = age;
DNI = dni;
}

[Required]
[StringLength(50)]
public string Name { get; set; } = string.Empty;

[Required]
[StringLength(100)]
public string Surname { get; set; } = string.Empty;

[Required]
[StringLength(9)]
public string DNI { get; set; } = string.Empty;

[Required]
public string Sex { get; set; } = string.Empty;

[Range(0, 120)]
public int Age { get; set; }
}
}