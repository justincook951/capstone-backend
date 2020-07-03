using CapstoneQuizAPI.DTOs;
using CapstoneQuizAPI.Models;
using CapstoneQuizAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace CapstoneQuizAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly CapstoneQuizContext _context;
        private IConfiguration _iconfig;

        public TokenController(IConfiguration iconfig, CapstoneQuizContext context) { 
            _iconfig = iconfig;
            _context = context;
        }

        [HttpPost]
        [AllowAnonymous]
        public string PostUser(LoginDTO loginDto)
        {
            var username = loginDto.username;
            var password = loginDto.password;
            if (CheckUser(username, password)) {
                var jwtmgr = new JwtManager(_iconfig);
                return jwtmgr.GenerateToken(username);
            }

            throw new UnauthorizedAccessException();
        }

        public bool CheckUser(string username, string password)
        {
            var context = _context;
            var query = from qUser in context.User
                        where qUser.Username == username
                        select qUser;
            var user = query.FirstOrDefault<User>();
            var passHasher = new PasswordHasher<User>();
            var result = passHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}