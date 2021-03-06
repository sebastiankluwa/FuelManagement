using System;
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
        private readonly IUnitOfWork unitOfWork;

        public RefuelingRepository(DataContext context, IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
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

        public async Task<IEnumerable<RefuelingDto>> GetCurrentRefuelingsByTankIdAsync(int id)
        {
            return await context.Refuelings
                .Where(r => r.TankId == id
                    && DateTime.Compare(r.RefuelDate, r.Tank.FillingDate) > 0
                )
                .OrderByDescending(r => r.RefuelDate)
                .ProjectTo<RefuelingDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<IEnumerable<RefuelingDto>> GetRefuelingsByUserIdAsync(int id)
        {
            return await context.Refuelings
                .Where(r => r.AppUserId == id)
                .OrderByDescending(r => r.RefuelDate)
                .ProjectTo<RefuelingDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<IEnumerable<RefuelingDto>> GetRefuelingsAsync()
        {
            return await context.Refuelings
                .OrderByDescending(r => r.RefuelDate)
                .ProjectTo<RefuelingDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public void AddRefueling(Refueling refueling)
        {
            var tank = refueling.Tank;
            var vehicle = refueling.Vehicle;

            tank.FuelAmount = tank.FuelAmount - refueling.FuelAmount;
            unitOfWork.TankRepository.Update(tank);

            vehicle.FuelAmount = refueling.FuelAmount;
            vehicle.Mileage = refueling.Mileage;
            unitOfWork.VehicleRepository.UpdateVehicle(vehicle);

            context.Refuelings.Add(refueling);
        }

        public void RemoveRefueling(int id)
        {
            Refueling refueling = context.Refuelings.Find(id);
            context.Refuelings.Remove(refueling);
        }


    }
}