using CapstoneQuizAPI.DTOs;
using CapstoneQuizAPI.Models;
using CapstoneQuizAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Collections.Generic;

namespace CapstoneQuizAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly CapstoneQuizContext _context;
        private IConfiguration _iconfig;
        private User _user;

        public TokenController(IConfiguration iconfig, CapstoneQuizContext context) { 
            _iconfig = iconfig;
            _context = context;
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult<IEnumerable<TokenDTO>> GetToken(LoginDTO loginDto)
        {
            var username = loginDto.username;
            var password = loginDto.password;
            if (CheckUser(username, password)) {
                var jwtmgr = new JwtManager(_iconfig);
                var tokenString = jwtmgr.GenerateToken(_user, _user.IsAdmin);
                var tokenDto = new TokenDTO
                {
                    Token = tokenString,
                    IsAdmin = _user.IsAdmin,
                    User = UsersController.UserToDTO(_user)
                };
                HttpContext.Response.Cookies.Append(
                    "token", 
                    tokenString,
                    new Microsoft.AspNetCore.Http.CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        IsEssential = true
                    });
                return new List<TokenDTO>() { tokenDto };
            }

            return Unauthorized();
        }

        public bool CheckUser(string username, string password)
        {
            var context = _context;
            var query = from qUser in context.User
                        where qUser.Username == username
                        select qUser;
            var user = query.FirstOrDefault<User>();
            if (user == null) {
                return false;
            }
            var passHasher = new PasswordHasher<User>();
            var result = passHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (result == PasswordVerificationResult.Success) {
                _user = user;
            }
            return result == PasswordVerificationResult.Success;
        }
    }
}