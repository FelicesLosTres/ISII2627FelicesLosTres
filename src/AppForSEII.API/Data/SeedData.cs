namespace AppForSEII.API.Data
{
    public class SeedData
    {
        private const string AdminEmail = "elena@uclm.es";
        private const string CustomerEmail = "peter@uclm.es";

        public static async Task InitializeAsync(
            ApplicationDbContext dbContext,
            IServiceProvider serviceProvider)
        {
            var roles = new[] { "Administrator", "Employee", "Customer" };
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    EnsureSucceeded(
                        await roleManager.CreateAsync(new IdentityRole(roleName)),
                        $"creating role '{roleName}'");
                }
            }

            var administrator = await userManager.FindByEmailAsync(AdminEmail);
            if (administrator is null)
            {
                administrator = new ApplicationUser("1", "Elena", "Navarro Martínez", AdminEmail)
                {
                    EmailConfirmed = true
                };
                EnsureSucceeded(
                    await userManager.CreateAsync(administrator, "Password1234%"),
                    $"creating user '{AdminEmail}'");
            }
            await EnsureInRoleAsync(userManager, administrator, roles[0]);

            var customer = await userManager.FindByEmailAsync(CustomerEmail);
            if (customer is null)
            {
                customer = new ApplicationUser("3", "Peter", "Jackson", CustomerEmail)
                {
                    EmailConfirmed = true
                };
                EnsureSucceeded(
                    await userManager.CreateAsync(customer, "OtherPass12$"),
                    $"creating user '{CustomerEmail}'");
            }
            await EnsureInRoleAsync(userManager, customer, roles[2]);

            await SeedReservaPista(dbContext, customer);
            await SeedAlquilerMaterial(dbContext, customer);
            await SeedInscripcionCompeticion(dbContext, customer);
            await SeedInscripcionClase(dbContext, customer);
        }

        private static async Task EnsureInRoleAsync(
            UserManager<ApplicationUser> userManager,
            ApplicationUser user,
            string role)
        {
            if (!await userManager.IsInRoleAsync(user, role))
            {
                EnsureSucceeded(
                    await userManager.AddToRoleAsync(user, role),
                    $"adding user '{user.Email}' to role '{role}'");
            }
        }

        private static void EnsureSucceeded(IdentityResult result, string operation)
        {
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(error => error.Description));
                throw new InvalidOperationException($"Identity failed while {operation}: {errors}");
            }
        }

        private static async Task<TipoDeporte> GetOrCreateTipoDeporte(
            ApplicationDbContext dbContext,
            string nombre)
        {
            var tipoDeporte = await dbContext.TiposDeportes
                .FirstOrDefaultAsync(tipo => tipo.Nombre == nombre);
            if (tipoDeporte is not null)
            {
                return tipoDeporte;
            }

            tipoDeporte = new TipoDeporte(nombre);
            dbContext.TiposDeportes.Add(tipoDeporte);
            await dbContext.SaveChangesAsync();
            return tipoDeporte;
        }

        // CU1
        public static async Task SeedReservaPista(ApplicationDbContext dbContext, ApplicationUser user)
        {
            var padel = await GetOrCreateTipoDeporte(dbContext, "Padel");
            var pista = await dbContext.Pistas
                .FirstOrDefaultAsync(item => item.NombrePista == "Pista Padel 1");
            if (pista is null)
            {
                pista = new Pista("Pista Padel 1", 4, 15, 10, padel.Id);
                dbContext.Pistas.Add(pista);
                await dbContext.SaveChangesAsync();
            }

            var reservaExists = await dbContext.Reservas.AnyAsync(reserva =>
                reserva.UserId == user.Id
                && reserva.PistasReservadas.Any(item => item.IdPista == pista.IdPista));
            if (!reservaExists)
            {
                var reserva = new Reserva(DateTime.Now, 15, MetodoPago.Bizum, user)
                {
                    PistasReservadas = new List<PistaReservada>
                    {
                        new(1, pista.Precio, pista.IdPista, 0)
                        {
                            Pista = pista,
                            Observaciones = string.Empty
                        }
                    }
                };
                dbContext.Reservas.Add(reserva);
                await dbContext.SaveChangesAsync();
            }
        }

        // CU2
        public static async Task SeedAlquilerMaterial(ApplicationDbContext dbContext, ApplicationUser user)
        {
            var tenis = await GetOrCreateTipoDeporte(dbContext, "Tenis");
            var tipoMaterial = await dbContext.TiposMateriales
                .FirstOrDefaultAsync(tipo => tipo.NombreTipoMaterial == "Raqueta");
            if (tipoMaterial is null)
            {
                tipoMaterial = new TipoMaterial("Raqueta");
                dbContext.TiposMateriales.Add(tipoMaterial);
                await dbContext.SaveChangesAsync();
            }

            var material = await dbContext.Materiales
                .FirstOrDefaultAsync(item => item.Nombre == "Raqueta Wilson");
            if (material is null)
            {
                material = new Material(3, "Raqueta Wilson", 15)
                {
                    IdTipoDeporte = tenis.Id,
                    IdTipoMaterial = tipoMaterial.IdTipoMaterial,
                    TipoDeporte = tenis,
                    TipoMaterial = tipoMaterial
                };
                dbContext.Materiales.Add(material);
                await dbContext.SaveChangesAsync();
            }

            var alquilerExists = await dbContext.Alquileres.AnyAsync(alquiler =>
                alquiler.UserId == user.Id
                && alquiler.MaterialesAlquilados.Any(item => item.IdMaterial == material.IdMaterial));
            if (!alquilerExists)
            {
                var alquiler = new Alquiler(DateTime.Now, MetodoPago.Tarjeta, material.Precio, user)
                {
                    MaterialesAlquilados = new List<MaterialAlquilado>()
                };
                alquiler.MaterialesAlquilados.Add(
                    new MaterialAlquilado(1, "Sin observaciones", 0, material.IdMaterial, material.Precio)
                    {
                        Alquiler = alquiler,
                        Material = material
                    });
                dbContext.Alquileres.Add(alquiler);
                await dbContext.SaveChangesAsync();
            }
        }

        // CU3
        public static async Task SeedInscripcionCompeticion(ApplicationDbContext dbContext, ApplicationUser user)
        {
            var tenis = await GetOrCreateTipoDeporte(dbContext, "Tenis");
            var competicion = await dbContext.Competiciones
                .FirstOrDefaultAsync(item => item.Nombre == "Torneo Primavera");
            if (competicion is null)
            {
                competicion = new Competicion(
                    DateTime.Today.AddMonths(1),
                    "Pista central",
                    "Torneo Primavera",
                    20,
                    10,
                    tenis.Id);
                dbContext.Competiciones.Add(competicion);
                await dbContext.SaveChangesAsync();
            }

            var inscriptionExists = await dbContext.Inscripciones.AnyAsync(inscripcion =>
                inscripcion.UserId == user.Id
                && inscripcion.CompeticionesInscritas.Any(item => item.CompeticionId == competicion.Id));
            if (!inscriptionExists)
            {
                var inscripcion = new Inscripcion(DateTime.Now, MetodoPago.Tarjeta, competicion.Precio, user)
                {
                    CompeticionesInscritas = new List<CompeticionInscrita>(),
                    ClasesInscritas = new List<ClaseInscrita>()
                };
                inscripcion.CompeticionesInscritas.Add(
                    new CompeticionInscrita(competicion.Id, 0, "Ningún problema físico")
                    {
                        Inscripcion = inscripcion,
                        Competicion = competicion
                    });
                dbContext.Inscripciones.Add(inscripcion);
                await dbContext.SaveChangesAsync();
            }
        }

        // CU4
        public static async Task SeedInscripcionClase(ApplicationDbContext dbContext, ApplicationUser user)
        {
            var tenis = await GetOrCreateTipoDeporte(dbContext, "Tenis");
            var clase = await dbContext.ClasesDeportivas
                .FirstOrDefaultAsync(item => item.Nombre == "Iniciacion al tenis");
            if (clase is null)
            {
                clase = new ClaseDeportiva(
                    "Iniciacion al tenis",
                    "Clase basica",
                    20,
                    tenis.Id,
                    DateTime.Now,
                    "Pista tenis",
                    "JuanDiego",
                    "Avanzado",
                    20,
                    20);
                dbContext.ClasesDeportivas.Add(clase);
                await dbContext.SaveChangesAsync();
            }

            var inscriptionExists = await dbContext.Inscripciones.AnyAsync(inscripcion =>
                inscripcion.UserId == user.Id
                && inscripcion.ClasesInscritas.Any(item => item.ClaseDeportivaId == clase.IdClaseDeportiva));
            if (!inscriptionExists)
            {
                var inscripcion = new Inscripcion(DateTime.Now, MetodoPago.Transferencia, clase.PrecioUnitario, user)
                {
                    CompeticionesInscritas = new List<CompeticionInscrita>(),
                    ClasesInscritas = new List<ClaseInscrita>()
                };
                inscripcion.ClasesInscritas.Add(
                    new ClaseInscrita(clase.IdClaseDeportiva, 0, "Sin observaciones", 1, clase.PrecioUnitario)
                    {
                        Inscripcion = inscripcion,
                        ClaseDeportiva = clase
                    });
                dbContext.Inscripciones.Add(inscripcion);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
