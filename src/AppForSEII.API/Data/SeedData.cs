namespace AppForSEII.API.Data {
    public class SeedData {
        public static void Initialize(ApplicationDbContext dbContext, IServiceProvider serviceProvider, ILogger logger) {
            List<string> rolesNames = new List<string> { "Administrator", "Employee", "Customer" };

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            try {
                SeedRoles(roleManager, rolesNames);
            }
            catch (Exception ex) {
                logger.LogError(ex, "An error occurred seeding the roles in the Database.");
            }

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            try {
                SeedUsers(userManager, rolesNames);
            }
            catch (Exception ex) {
                logger.LogError(ex, "An error occurred seeding the Users in the Database.");
            }

 

        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager, List<string> roles) {

            foreach (string roleName in roles) {
                //it checks such role does not exist in the database 
                if (!roleManager.RoleExistsAsync(roleName).Result) {
                    IdentityRole role = new IdentityRole();
                    role.Name = roleName;
                    role.NormalizedName = roleName;
                    IdentityResult roleResult = roleManager.CreateAsync(role).Result;
                }
            }

        }

        public static void SeedUsers(UserManager<ApplicationUser> userManager, List<string> roles) {
            //first, it checks the user does not already exist in the DB
            if (userManager.FindByNameAsync("elena@uclm.es").Result == null) {
                ApplicationUser user = new ApplicationUser("1", "Elena", "Navarro Martínez", "elena@uclm.es");
                user.EmailConfirmed = true;

                var result = userManager.CreateAsync(user, "Password1234%");
                result.Wait();

                if (result.IsCompletedSuccessfully) {
                    //administrator role
                    userManager.AddToRoleAsync(user, roles[0]).Wait();
                }
            }


            if (userManager.FindByNameAsync("peter@uclm.es").Result == null) {
                //A customer class has been defined because it has different attributes (purchase, rental, etc.)
                ApplicationUser user = new ApplicationUser("3", "Peter", "Jackson", "peter@uclm.es");
                user.EmailConfirmed = true;

                var result = userManager.CreateAsync(user, "OtherPass12$");

                result.Wait();

                if (result.IsCompletedSuccessfully) {
                    //customer role
                    userManager.AddToRoleAsync(user, roles[2]).Wait();

                }
            }

        }

        //CU1
        public static void SeedReservaPista(ApplicationDbContext dbContext, ApplicationUser user)
      {
         TipoDeporte padel;

         if (dbContext.TiposDeportes.FirstOrDefault(td => td.Nombre == "Padel") == null)
         {
          padel = new TipoDeporte("Padel");

         dbContext.TiposDeportes.Add(padel);
         dbContext.SaveChanges();
         }
         else 
         {
         padel = dbContext.TiposDeportes.First(td => td.Nombre == "Padel");
          }

         if (dbContext.Pistas.FirstOrDefault(p => p.NombrePista == "Pista Padel 1") == null)
         {
         var pista = new Pista("Pista Padel 1", 4, 15, 10, padel.Id);

         dbContext.Pistas.Add(pista);
         dbContext.SaveChanges();
          }

         if (dbContext.Reservas.FirstOrDefault(r => r.IdReserva == 1) == null)
        {
         var pista = dbContext.Pistas.First();

         var reserva = new Reserva(DateTime.Now, 15, MetodoPago.Bizum, user);

         reserva.PistasReservadas.Add(new PistaReservada(1, pista.Precio, pista.IdPista, reserva.IdReserva));

         dbContext.Reservas.Add(reserva);
         }

          dbContext.SaveChanges();
        }

        //CU2
        //CU3
        public static void SeedInscripcionCompeticion(ApplicationDbContext dbContext, ApplicationUser user)
{
TipoDeporte tenis;

if (dbContext.TiposDeportes.FirstOrDefault(td => td.Nombre == "Tenis") == null)
{
tenis = new TipoDeporte("Tenis");

dbContext.TiposDeportes.Add(tenis);
dbContext.SaveChanges();
}
else
{
tenis = dbContext.TiposDeportes.First(td => td.Nombre == "Tenis");
}

if (dbContext.Competiciones.FirstOrDefault(c => c.Nombre == "Torneo Primavera") == null)
{
Competicion competicion = new Competicion( DateTime.Today.AddMonths(1), "Torneo Primavera", "Competición de iniciación", 20, 10, tenis.Id);

dbContext.Competiciones.Add(competicion);
dbContext.SaveChanges();
}

if (dbContext.Inscripciones.FirstOrDefault(i => i.Id == 1) == null)
{
var competicion = dbContext.Competiciones.First();

Inscripcion inscripcion = new Inscripcion(DateTime.Now, MetodoPago.Tarjeta, 20, user);

inscripcion.CompeticionesInscritas.Add(
new CompeticionInscrita(
competicion.Id,
inscripcion.Id,
"Ningún problema físico"
)
);

dbContext.Inscripciones.Add(inscripcion);
}

dbContext.SaveChanges();
}
        //CU4






    }
}
