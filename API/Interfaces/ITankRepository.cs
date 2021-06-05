using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;

namespace API.Interfaces
{
    public interface ITankRepository
    {
        Task<IEnumerable<Tank>> GetTanksAsync();

        Task<Tank> GetTankByIdAsync(int id);

        void AddTank(Tank tank);

        void RemoveTank(int id);

        void Update(Tank tank);
    }
}