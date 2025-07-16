using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagement.API.DTOs;
using UserManagement.Domain.Entities;
using UserManagement.Infrastructure.Data;

namespace UserManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateUserDto> _createUserValidator;
        private readonly IValidator<UpdateUserDto> _updateUserValidator;

        public UsersController(
            AppDbContext context,
            IMapper mapper,
            IValidator<CreateUserDto> createUserValidator,
            IValidator<UpdateUserDto> updateUserValidator)
        {
            _context = context;
            _mapper = mapper;
            _createUserValidator = createUserValidator;
            _updateUserValidator = updateUserValidator;
        }


        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<UserDto>>(users));
        }


        // GET: api/users/id
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            return Ok(_mapper.Map<UserDto>(user));
        }

        // POST: api/users
        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto dto)
        {
            var validationResult = await _createUserValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

                return ValidationProblem(ModelState);
            }


            var user = _mapper.Map<User>(dto);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<UserDto>(user);
            return CreatedAtAction(nameof(GetUser), new { id = result.Id }, result);
        }

        // PUT: api/users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserDto dto)
        {
            var validationResult = await _updateUserValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

                return ValidationProblem(ModelState);
            }

            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null) return NotFound();

            _mapper.Map(dto, existingUser);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
