using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class RefuelingRepository : IRefuelingRepository
    {
        private readonly DataContext context;

        public RefuelingRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<Refueling> GetRefuelingByIdAsync(int id)
        {
            return await context.Refuelings.FindAsync(id);
        }

        public async Task<IEnumerable<Refueling>> GetRefuelingsByUserIdAsync(int id)
        {
            return await context.Refuelings
                .Where(r => r.AppUserId == id)
                .OrderBy(r => r.RefuelDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Refueling>> GetRefuelingsAsync()
        {
            return await context.Refuelings.ToListAsync();
        }

        public void AddRefueling(Refueling refueling)
        {
            context.Refuelings.Add(refueling);
        }

        public void RemoveRefueling(int id)
        {
            Refueling refueling = context.Refuelings.Find(id);
            context.Refuelings.Remove(refueling);
        }
    }
}