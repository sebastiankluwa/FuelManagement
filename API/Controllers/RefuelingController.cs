using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class RefuelingController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        public RefuelingController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<ActionResult<RefuelingDto>> AddRefueling(AddRefuelingDto addRefuelingDto)
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

            if(await _unitOfWork.Complete()) 
            {
        
                var refuelingDto = new RefuelingDto
                {
                    RefuelingId = refueling.RefuelingId,
                    AppUserId = refueling.AppUserId,
                    VehicleId = refueling.VehicleId,
                    TankId = refueling.TankId,
                    RefuelDate = refueling.RefuelDate,
                    Mileage = refueling.Mileage,
                    FuelAmount = refueling.FuelAmount
                };

                return Ok(refuelingDto);
            }

            return BadRequest("Failed to refuel!");
        }

        [HttpGet("user/{username}")]
        public async Task<ActionResult<IEnumerable<RefuelingDto>>> GetRefuelingsByUser(string username)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);

            if(user == null) return BadRequest("User not found");

            var refuelings = await _unitOfWork.RefuelingRepository.GetRefuelingsByUserIdAsync(user.Id);

            return Ok(refuelings);
        }

        [HttpGet("{id}", Name = "GetRefueling")]
        public async Task<ActionResult<RefuelingDto>> GetRefuelingById(int id)
        {

            return await _unitOfWork.RefuelingRepository.GetRefuelingByIdAsync(id);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RefuelingDto>>> GetRefuelings()
        {
            var refuelings = await _unitOfWork.RefuelingRepository.GetRefuelingsAsync();

            return Ok(refuelings);
        }

        [HttpDelete("{refuelingId}")]
        public async Task<ActionResult> RemoveRefueling(int refuelingId)
        {
            
            _unitOfWork.RefuelingRepository.RemoveRefueling(refuelingId);

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Failed to delete the refueling entry");


        }
    }
}