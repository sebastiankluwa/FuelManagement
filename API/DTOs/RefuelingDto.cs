using System;

namespace API.DTOs
{
    public class RefuelingDto
    {
        public int RefuelingId { get; set; }
        public int AppUserId { get; set; }

        public int VehicleId { get; set; }

        public int TankId { get; set; }

        public DateTime RefuelDate { get; set; }

        public float Mileage { get; set; }

        public float FuelAmount { get; set; }
    }
}