using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedData(DataContext context)
        {
            if(await context.Vehicles.AnyAsync() ||  await context.Tanks.AnyAsync() || await context.Refuelings.AnyAsync()) return;

            using var hmac = new HMACSHA512();
            
            AppUser admin = new AppUser()
            {
                UserName = "admin",
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("password")),
                PasswordSalt = hmac.Key,
            };

            context.Users.Add(admin);

            var vehiclesData = await System.IO.File.ReadAllTextAsync("Data/VehicleSeedData.json");
            var vehicles = JsonSerializer.Deserialize<List<Vehicle>>(vehiclesData);
            if(vehicles == null) return;

            foreach (var veh in vehicles)
            {
                context.Vehicles.Add(veh);
            }

            var tanksData = await System.IO.File.ReadAllTextAsync("Data/TankSeedData.json");
            var tanks = JsonSerializer.Deserialize<List<Tank>>(tanksData);
            if(tanks == null) return;

            foreach (var tank in tanks)
            {
                context.Tanks.Add(tank);
            }

            await context.SaveChangesAsync();

            var refuelingsData = await System.IO.File.ReadAllTextAsync("Data/RefuelingSeedData.json");
            var refuelings = JsonSerializer.Deserialize<List<Refueling>>(refuelingsData);
            if(refuelings == null) return;

            foreach (var refueling in refuelings)
            {
                refueling.AppUser = await context.Users.FindAsync(refueling.AppUserId);
                refueling.Vehicle = await context.Vehicles.FindAsync(refueling.VehicleId);
                refueling.Tank = await context.Tanks.FindAsync(refueling.TankId);

                context.Refuelings.Add(refueling);
            }

            await context.SaveChangesAsync();
        }
    }
}