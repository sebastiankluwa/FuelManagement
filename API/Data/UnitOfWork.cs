using System.Threading.Tasks;
using API.Interfaces;

namespace API.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext context;

        public UnitOfWork(DataContext context)
        {
            this.context = context;
        }

        public IRefuelingRepository RefuelingRepository => new RefuelingRepository(context);
        public ITankRepository TankRepository => new TankRepository(context);
        public IVehicleRepository VehicleRepository => new VehicleRepository(context);
        public IUserRepository UserRepository => new UserRepository(context);

        public async Task<bool> Complete()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public bool hasChanges()
        {
            return context.ChangeTracker.HasChanges();
        }
    }
}