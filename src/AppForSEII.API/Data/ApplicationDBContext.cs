using AppForSEII.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AppForSEII.API.DTOs.ApplicationUserDTO;

namespace AppForSEII.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }


    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<ClaseInscrita> ClasesInscritas { get; set; }
    public DbSet<ClaseDeportiva> ClasesDeportivas { get; set; }
    
    public DbSet<TipoDeporte> TiposDeportes { get; set; }
 
    public DbSet<Material> Material { get; set; }
    public DbSet<TipoMaterial> TipoMaterial { get; set; }
    public DbSet<MaterialAlquilado> MaterialAlquilado { get; set; }
    public DbSet<PistaReservada> PistasReservadas { get; set; }

    public DbSet<Alquiler> Alquileres { get; set; }
    
    public DbSet<Inscripcion> Inscripciones { get; set; }
    public DbSet<Competicion> Competiciones { get; set; }

    public DbSet<CompeticionInscrita> CompeticionesInscritas { get; set; }
    public DbSet<Pista> Pistas { get; set; }
    public DbSet<Reserva> Reservas { get; set; }
   



}
