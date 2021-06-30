using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{

    public class UsersController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly IMapper mapper;
        public UsersController(DataContext context, IMapper mapper)
        {
            this.mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var queryable = _context.Users
                                .Include(u => u.Refuelings)
                                .ProjectTo<UserDto>(mapper.ConfigurationProvider);
            return await queryable.ToListAsync();
        }

        //api/users/3
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            return await _context.Users
                .Include(u => u.Refuelings)
                .ProjectTo<UserDto>(mapper.ConfigurationProvider)
                .Where(u => u.Id == id)
                .SingleOrDefaultAsync();
        
        }

    }
}