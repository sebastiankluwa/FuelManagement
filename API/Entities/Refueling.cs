using System;
using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class Refueling
    {
        public AppUser AppUser { get; set; }

        public int AppUserId { get; set; }

        public Vehicle Vehicle { get; set; }

        public int VehicleId { get; set; }

        public Tank Tank { get; set; }

        public int TankId { get; set; }

        public DateTime RefuelDate { get; set; } = DateTime.Now;

        public float Mileage { get; set; }

        public float FuelAmount { get; set; }
    }
}