using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        IRefuelingRepository RefuelingRepository {get;}
        IVehicleRepository VehicleRepository {get;}
        ITankRepository TankRepository {get;}
        IUserRepository UserRepository {get;}
        Task<bool> Complete();
        bool hasChanges();
    }
}