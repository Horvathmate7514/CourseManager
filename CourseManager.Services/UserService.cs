using AutoMapper;
using CourseManager.DataContext.Context;
using CourseManager.DataContext.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager.Services
{
    public interface IUserService
    {
        Task<UserDto> RegisterAsync(RegisterUserDto dto);

        Task<UserDto?> GetUserByIdAsync(int id);

        Task<UserDto> UpdateUser(int id, UpdateUserDto dto);

        Task DeactivateUser(int id);

        Task ReactivateUser(int id);
    }
    public class UserService : IUserService
    {
        private readonly NeptunDbContext _context;
        private readonly IMapper _mapper;
        public UserService(NeptunDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserDto> RegisterAsync(RegisterUserDto dto)
        {
            var user = _mapper.Map<DataContext.Entities.User>(dto);
            user.IsActive = true;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) throw new Exception("Felhasználó nem található!");
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> UpdateUser(int id, UpdateUserDto dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) throw new Exception("Felhasználó nem található!");
            if (!user.IsActive) throw new Exception("Inaktív felhasználó nem szerkeszthető!");
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            {
                throw new Exception("Email cím már foglalt!");
            }
            _mapper.Map(dto, user);
            await _context.SaveChangesAsync();
            return _mapper.Map<UserDto>(user);
        }

        public async Task DeactivateUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) throw new Exception("Felhasználó nem található!");
            user.IsActive = false;
            await _context.SaveChangesAsync();
        }

        public async Task ReactivateUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) throw new Exception("Felhasználó nem található!");
            user.IsActive = true;
            await _context.SaveChangesAsync();
        }
    }
}
