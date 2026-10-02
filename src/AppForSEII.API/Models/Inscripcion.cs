using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppForSEII.API.Models
{
public class Inscripcion
{
[Key]
public int Id { get; set; }

[Required(ErrorMessage = "La fecha de inscripción es obligatoria.")]
public DateTime FechaInscripcion { get; set; }

[Required(ErrorMessage = "El método de pago es obligatorio.")]
public MetodoPago MetodoPago { get; set; }

[Required(ErrorMessage = "El precio total es obligatorio.")]
[Range(0.01, 99999,
ErrorMessage = "El precio total debe ser mayor que 0.")]
public decimal PrecioTotal { get; set; }

public ApplicationUser ApplicationUser {get; set; }//Para rel. con ApplicationUser
public string Name { get; set; }
public string Surname { get; set; }
public string PhoneNumber { get; set; }
public int Dni { get; set; }
public int Age { get; set; }
public string Sex { get; set; }
public string Email { get; set; }



// Relación 1:N con ClaseInscrita
public IList<ClaseInscrita> ClasesInscritas { get; set; }


public Inscripcion()
{
    
}


public Inscripcion(
DateTime fechaInscripcion,
MetodoPago metodoPago,
decimal precioTotal,
ApplicationUser applicationUser,
string name,
string surname,
string phoneNumber,
int dni,
int age,
string sex,
string email)
{
FechaInscripcion = fechaInscripcion;
MetodoPago = metodoPago;
PrecioTotal = precioTotal;
ApplicationUser = applicationUser;
Name = name;
Surname = surname;
PhoneNumber = phoneNumber;
Dni = dni;
Age = age;
Sex = sex;
Email = email;

}
}
}
