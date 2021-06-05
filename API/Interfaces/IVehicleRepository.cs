using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;

namespace API.Interfaces
{
    public interface IVehicleRepository
    {
        Task<IEnumerable<Vehicle>> GetVehiclesAsync();

        Task<Vehicle> GetVehicleByIdAsync(int id);

        void AddVehicle(Vehicle vehicle);

        void RemoveVehicle(int id);

        void UpdateVehicle(Vehicle vehicle);
    }
}