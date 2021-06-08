using System;
using System.Collections.Generic;

namespace API.Entities
{
    public enum Types
    {
        Stationary,
        Mobile
    }
    public class Tank
    {
        public int TankId { get; set; }
        public ICollection<Refueling> Refuelings { get; set; }

        public Types Type { get; set; }

        public float Capacity { get; set; }

        public float FuelAmount { get; set; }

        public DateTime FillingDate { get; set; } = DateTime.Now;
    }
}