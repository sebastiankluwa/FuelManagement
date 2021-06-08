using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IRefuelingRepository
    {
        Task<IEnumerable<Refueling>> GetRefuelingsAsync();

        Task<RefuelingDto> GetRefuelingByIdAsync(int id);

        Task<IEnumerable<Refueling>> GetRefuelingsByUserIdAsync(int id);

        void AddRefueling(Refueling refueling);

        void RemoveRefueling(int id);

    }
}