using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CapstoneQuizAPI.Models;
using CapstoneQuizAPI.DTOs;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace CapstoneQuizAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly CapstoneQuizContext _context;

        public UsersController(CapstoneQuizContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUser()
        {
            return await _context.User
                .Select(User => UserToDTO(User))
                .ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(long id)
        {
            var User = await _context.User.FindAsync(id);

            if (User == null)
            {
                return NotFound();
            }

            return UserToDTO(User);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(long id, UserDTO UserDTO)
        {
            if (id != UserDTO.Id)
            {
                return BadRequest();
            }

            var User = await _context.User.FindAsync(id);
            if (User == null) {
                return NotFound();
            }

            User = UpdatePutableFields(User, UserDTO);

            _context.Entry(User).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostUser(UserDTO UserDTO)
        {
            var User = CreateFromDTO(UserDTO);
            _context.User.Add(User);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetUser", new { id = User.Id }, User);
            return CreatedAtAction(nameof(GetUser), new { id = User.Id }, UserToDTO(User));
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(long id)
        {
            var User = await _context.User.FindAsync(id);
            if (User == null)
            {
                return NotFound();
            }

            _context.User.Remove(User);
            await _context.SaveChangesAsync();

            return User;
        }

        private bool UserExists(long id)
        {
            return _context.User.Any(e => e.Id == id);
        }

        public static UserDTO UserToDTO(User User) =>
            new UserDTO
            {
                Id = User.Id,
                Username = User.Username,
                Password = "OBFUSCATED",
                FirstName = User.FirstName,
                LastName = User.LastName,
                IsAdmin = User.IsAdmin,
            };

        private static User UpdatePutableFields(User User, UserDTO UserDTO)
        {
            User.Username = UserDTO.Username;
            User.FirstName = UserDTO.FirstName;
            User.LastName = UserDTO.LastName;
            User.IsAdmin = UserDTO.IsAdmin;

            return User;
        }
        private static User CreateFromDTO(UserDTO UserDTO)
        {
            var User = new User
            {
                PasswordHash = UserDTO.Password,
                Username = UserDTO.Username,
                FirstName = UserDTO.FirstName,
                LastName = UserDTO.LastName,
                IsAdmin = UserDTO.IsAdmin,
            };
            // Despite the name, the password coming in from post is not actually hashed.
            User.PasswordHash = HashPassword(User);

            return User;
        }

        private static string HashPassword(User user)
        {
            var passHasher = new PasswordHasher<User>();
            user.PasswordHash = passHasher.HashPassword(user, user.PasswordHash);
            return user.PasswordHash;
        }
    }
}
