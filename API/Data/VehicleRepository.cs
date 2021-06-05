using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly DataContext context;

        public VehicleRepository(DataContext context)
        {
            this.context = context;
        }

        public void AddVehicle(Vehicle vehicle)
        {
            context.Vehicles.Add(vehicle);
        }

        public async Task<Vehicle> GetVehicleByIdAsync(int id)
        {
            return await context.Vehicles.FindAsync(id);
        }

        public async Task<IEnumerable<Vehicle>> GetVehiclesAsync()
        {
            return await context.Vehicles.ToListAsync();
        }

        public void RemoveVehicle(int id)
        {
            Vehicle vehicle = context.Vehicles.Find(id);
            context.Vehicles.Remove(vehicle);
        }

        public void UpdateVehicle(Vehicle vehicle)
        {
            context.Entry(vehicle).State = EntityState.Modified;
        }
    }
}