using Core.Domain;
using Microsoft.EntityFrameworkCore;
using System;

namespace Core.Configuration
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Vehicle> Vehicles { get; set; }

        public DbSet<Rental> Rentals { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Client>().HasKey(x => x.Id);
            builder.Entity<Vehicle>().HasKey(x => x.Id);
            builder.Entity<Rental>().HasKey(x => x.Id);

            builder.Entity<Rental>().HasOne(x => x.Client).WithMany(x => x.Rentals).HasForeignKey(x => x.ClientId);
            builder.Entity<Rental>().HasOne(x => x.Vehicle).WithMany(x => x.Rentals).HasForeignKey(x => x.VehicleId);

            SeedData(builder);
        }

        /// <summary>
        /// Seed initial data in database
        /// </summary>
        /// <param name="builder"></param>
        private void SeedData(ModelBuilder builder)
        {
            var clients = new[]
            {
                new Client { Id = 1, Name = "Homero", Phone = "11111", Active = true },
                new Client { Id = 2, Name = "Bart", Phone = "22222", Active = true },
                new Client { Id = 3, Name = "Lisa", Phone = "33333", Active = true },
                new Client { Id = 4, Name = "Marge", Phone = "44444", Active = true },
                new Client { Id = 5, Name = "Maggie", Phone = "55555", Active = false },
                new Client { Id = 6, Name = "Bob Patiño", Phone = "66666", Active = true },
                new Client { Id = 7, Name = "Jefe Gorgory", Phone = "777777", Active = false },
                new Client { Id = 8, Name = "Moe", Phone = "888888", Active = true },
            };

            var vehicles = new[]
            {
                new Vehicle { Id = 1, Brand = "Renault", Year = 2021, PricePerDay = 10, Active = true },
                new Vehicle { Id = 2, Brand = "Audi", Year = 2021, PricePerDay = 20, Active = true },
                new Vehicle { Id = 3, Brand = "Ford", Year = 2021, PricePerDay = 15, Active = true },
                new Vehicle { Id = 4, Brand = "Fiat", Year = 2021, PricePerDay = 10, Active = false },
                new Vehicle { Id = 5, Brand = "BMW", Year = 2021, PricePerDay = 25, Active = true },
                new Vehicle { Id = 6, Brand = "volkswagen", Year = 2021, PricePerDay = 10, Active = false },
                new Vehicle { Id = 7, Brand = "Renault", Year = 2020, PricePerDay = 10, Active = true },
                new Vehicle { Id = 8, Brand = "Audi", Year = 2020, PricePerDay = 20, Active = true },
                new Vehicle { Id = 9, Brand = "Ford", Year = 2020, PricePerDay = 15, Active = false },
                new Vehicle { Id = 10, Brand = "Fiat", Year = 2020, PricePerDay = 10, Active = true },
                new Vehicle { Id = 11, Brand = "BMW", Year = 2020, PricePerDay = 25, Active = true },
                new Vehicle { Id = 12, Brand = "volkswagen", Year = 2020, PricePerDay = 10, Active = true }
            };

            var rentals = new[]
            {
                new Rental { Id = 1, ClientId = clients[0].Id,  VehicleId = vehicles[0].Id, StartDate = new DateTime(2021, 6, 1), EndDate = new DateTime(2021, 6, 7), Price = 70 },
                new Rental { Id = 2, ClientId = clients[0].Id, VehicleId = vehicles[1].Id, StartDate = new DateTime(2021, 7, 1), EndDate = new DateTime(2021, 7, 7), Price = 140 },
                new Rental { Id = 3, ClientId = clients[1].Id, VehicleId = vehicles[0].Id, StartDate = new DateTime(2021, 7, 1), EndDate = new DateTime(2021, 7, 7), Price = 70 },
                new Rental { Id = 4, ClientId = clients[2].Id, VehicleId = vehicles[6].Id, StartDate = new DateTime(2021, 8, 1), EndDate = new DateTime(2021, 8, 7), Price = 70 },
                new Rental { Id = 5, ClientId = clients[3].Id, VehicleId = vehicles[9].Id, StartDate = new DateTime(2021, 8, 15), EndDate = new DateTime(2021, 8, 17), Price = 20 },
                new Rental { Id = 6, ClientId = clients[7].Id, VehicleId = vehicles[11].Id, StartDate = new DateTime(2021, 7, 20), EndDate = new DateTime(2021, 7, 25), Price = 50 }
            };

            builder.Entity<Client>().HasData(clients);
            builder.Entity<Vehicle>().HasData(vehicles);
            builder.Entity<Rental>().HasData(rentals);
        }
    }
}
