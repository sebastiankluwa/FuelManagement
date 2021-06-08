using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IRefuelingRepository
    {
        Task<IEnumerable<RefuelingDto>> GetRefuelingsAsync();

        Task<RefuelingDto> GetRefuelingByIdAsync(int id);

        Task<IEnumerable<RefuelingDto>> GetRefuelingsByUserIdAsync(int id);
        Task<IEnumerable<RefuelingDto>> GetCurrentRefuelingsByTankIdAsync(int id);

        void AddRefueling(Refueling refueling);

        void RemoveRefueling(int id);

    }
}