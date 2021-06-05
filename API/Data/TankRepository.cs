using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class TankRepository : ITankRepository
    {
        private readonly DataContext context;

        public TankRepository(DataContext context)
        {
            this.context = context;
        }

        public void AddTank(Tank tank)
        {
            context.Tanks.Add(tank);
        }

        public async Task<Tank> GetTankByIdAsync(int id)
        {
            return await context.Tanks.FindAsync(id);
        }

        public async Task<IEnumerable<Tank>> GetTanksAsync()
        {
            return await context.Tanks.ToListAsync();
        }

        public void RemoveTank(int id)
        {
            Tank tank = context.Tanks.Find(id);
            context.Tanks.Remove(tank);
        }

        public void Update(Tank tank)
        {
            context.Entry(tank).State = EntityState.Modified;
        }
    }
}