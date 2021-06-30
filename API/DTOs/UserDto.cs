using System;
using System.Collections.Generic;

namespace API.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }

        public ICollection<RefuelingDto> Refuelings { get; set; }

        public string UserName { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;
    }
}