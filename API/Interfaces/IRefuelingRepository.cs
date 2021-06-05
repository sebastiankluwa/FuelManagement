using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;

namespace API.Interfaces
{
    public interface IRefuelingRepository
    {
        Task<IEnumerable<Refueling>> GetRefuelingsAsync();

        Task<Refueling> GetRefuelingByIdAsync(int id);

        void AddRefueling(Refueling refueling);

        void RemoveRefueling(int id);

    }
}