using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class RefuelingRepository : IRefuelingRepository
    {
        private readonly DataContext context;
        private readonly IMapper _mapper;

        public RefuelingRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            this.context = context;
        }

        public async Task<RefuelingDto> GetRefuelingByIdAsync(int id)
        {
            return await context.Refuelings
                .Where(r => r.RefuelingId == id)
                .ProjectTo<RefuelingDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Refueling>> GetRefuelingsByUserIdAsync(int id)
        {
            return await context.Refuelings
                .Where(r => r.AppUserId == id)
                .OrderBy(r => r.RefuelDate)
                .ToListAsync();//project to refuelingdto (mapper)
        }

        public async Task<IEnumerable<Refueling>> GetRefuelingsAsync()
        {
            return await context.Refuelings
                .OrderBy(r => r.RefuelDate)
                .ToListAsync();
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