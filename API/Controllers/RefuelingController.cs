using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class RefuelingController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        public RefuelingController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<ActionResult<Refueling>> AddRefueling(AddRefuelingDto addRefuelingDto)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;

            var refueller = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            var vehicle = await _unitOfWork.VehicleRepository.GetVehicleByIdAsync(addRefuelingDto.VehicleId);
            var tank = await _unitOfWork.TankRepository.GetTankByIdAsync(addRefuelingDto.TankId);

            if(vehicle == null || tank == null) return NotFound();

            if(vehicle.Mileage > addRefuelingDto.Mileage)
                return BadRequest("Reversing the milleage is illegal!");

            var refueling = new Refueling
            {
                AppUser = refueller,
                Vehicle = vehicle,
                Tank = tank,
                Mileage = addRefuelingDto.Mileage,
                FuelAmount = addRefuelingDto.FuelAmount,
            };

            _unitOfWork.RefuelingRepository.AddRefueling(refueling);

            if(await _unitOfWork.Complete()) return Ok(refueling);

            return BadRequest("Failed to refuel!");
        }
    }
}