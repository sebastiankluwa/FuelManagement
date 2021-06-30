using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class RefuelingController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper mapper;
        public RefuelingController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<ActionResult<RefuelingDto>> AddRefueling(AddRefuelingDto addRefuelingDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var vehicle = await _unitOfWork.VehicleRepository.GetVehicleByIdAsync(addRefuelingDto.VehicleId);
            var tank = await _unitOfWork.TankRepository.GetTankByIdAsync(addRefuelingDto.TankId);

            if (vehicle == null || tank == null) return NotFound();

            if (vehicle.Mileage > addRefuelingDto.Mileage)
                return BadRequest("Mileage has to be more or equal to the last mileage");

            if (tank.FuelAmount < addRefuelingDto.FuelAmount)
                return BadRequest("Tank doesn't store such an amount of fuel");


            var refueling = new Refueling
            {
                AppUserId = userId,
                VehicleId = addRefuelingDto.VehicleId,
                TankId = addRefuelingDto.TankId,
                Vehicle = vehicle,
                Tank = tank,
                Mileage = addRefuelingDto.Mileage,
                FuelAmount = addRefuelingDto.FuelAmount,
            };

            _unitOfWork.RefuelingRepository.AddRefueling(refueling);

            if (await _unitOfWork.Complete())
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

                return CreatedAtRoute("GetRefueling", new { id = refueling.RefuelingId }, refuelingDto);
            }

            return BadRequest("Failed to refuel!");
        }

        [HttpGet("user/{username}")]
        public async Task<ActionResult<IEnumerable<RefuelingDto>>> GetRefuelingsByUser(string username)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);

            if (user == null) return BadRequest("User not found");

            var refuelings = await _unitOfWork.RefuelingRepository.GetRefuelingsByUserIdAsync(user.Id);

            return Ok(refuelings);
        }

        [HttpGet("tank/{id}")]
        public async Task<ActionResult<IEnumerable<RefuelingDto>>> GetCurrentRefuelingsByTank(int id)
        {
            var refuelings = await _unitOfWork.RefuelingRepository.GetCurrentRefuelingsByTankIdAsync(id);

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