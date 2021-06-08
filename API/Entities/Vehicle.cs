using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class Vehicle
    {
        public int VehicleId { get; set; }
        public ICollection<Refueling> Refuelings { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        public string Model { get; set; }

        public float Mileage { get; set; }

        public float FuelAmount { get; set; }
    }
}