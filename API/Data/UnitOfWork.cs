using System.Threading.Tasks;
using API.Interfaces;
using AutoMapper;

namespace API.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext context;
        private readonly IMapper _mapper;

        public UnitOfWork(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            this.context = context;
        }

        public IRefuelingRepository RefuelingRepository => new RefuelingRepository(context, _mapper);
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