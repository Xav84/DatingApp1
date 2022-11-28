using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
       public AccountController(DataContext context, ITokenService tokenService)
       {
            _tokenService = tokenService;
            _context = context;
        
       } 

       [HttpPost("register")] // POST: api/account/register

       public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
       {
        if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");

        // utilisation de using permet de jeter la classe apres utilisation
            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
               UserName = registerDto.Username.ToLower(),
               PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
               PasswordSalt = hmac.Key 
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
               Username = user.UserName,
               Token = _tokenService.CreateToken(user)
            };
       }

       [HttpPost("login")]

       public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
       {
          // trouver le user dans la bd avec le username peux pas utiliser Find() vu que username n'est pas un PK
          // SingleOrDefaultAsync() retourne null ou le user on l'utilise vu que les username seront unique
          var user = await _context.Users.SingleOrDefaultAsync(x =>
          x.UserName == loginDto.Username);

          if (user == null) return Unauthorized("invalid username");

          using var hmac = new HMACSHA512(user.PasswordSalt);
          // byte array
          var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

          // compare le password existant avec le password entré par l'utilisateur
          for (int i = 0; i < computedHash.Length; i++)
          {
               if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("invalid password");
          }

          return new UserDto
          {
          Username = user.UserName,
          Token = _tokenService.CreateToken(user)
          };

       }
       private async Task<bool> UserExists(string username)
       {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
       }
    }
}