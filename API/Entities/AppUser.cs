using System;
using System.Collections;
using System.Collections.Generic;

namespace API.Entities
{
    public class AppUser
    {
        public int Id { get; set; }

        public ICollection<Refueling> Refuelings { get; set; }

        public string UserName { get; set; }

        public byte[] PasswordHash { get; set; } 

        public byte[] PasswordSalt { get; set; }   

        public DateTime Created { get; set; } = DateTime.Now;

        
    }
}