using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
       public AccountController(DataContext context, ITokenService tokenService, IMapper mapper)
       {
            _mapper = mapper;
            _tokenService = tokenService;
            _context = context;
        
       } 

       [HttpPost("register")] // POST: api/account/register

       public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
       {
        if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");

            var user = _mapper.Map<AppUser>(registerDto);
            // utilisation de using permet de jeter la classe apres utilisation
            using var hmac = new HMACSHA512();

            user.UserName = registerDto.Username.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            user.PasswordSalt = hmac.Key;
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
               Username = user.UserName,
               Token = _tokenService.CreateToken(user),
               KnownAs = user.KnownAs
            };
       }

       [HttpPost("login")]

       public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
       {
          // trouver le user dans la bd avec le username peux pas utiliser Find() vu que username n'est pas un PK
          // SingleOrDefaultAsync() retourne null ou le user on l'utilise vu que les username seront unique
          var user = await _context.Users
          .Include(p => p.Photos)
          .SingleOrDefaultAsync(x =>
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
          Token = _tokenService.CreateToken(user),
          PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
          KnownAs = user.KnownAs
          };

       }
       private async Task<bool> UserExists(string username)
       {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
       }
    }
}