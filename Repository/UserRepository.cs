using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MagicVilla.Api.Data;
using MagicVilla.Api.Models;
using MagicVilla.Api.Models.DTOs;
using MagicVilla.Api.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace MagicVilla.Api.Repository;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;
    private string secretkey;

    public UserRepository(AppDbContext db, IConfiguration config)
    {
        _db = db;
        secretkey = config.GetValue<string>("ApiSettings", "Secret");
    }


    public async Task<bool> IsUniqueUser(string username)
    {
        var user = await _db.Users.FirstOrDefaultAsync(x => x.UserName == username);
        if (user == null)
        {
            return true;
        };

        return false;
    }

    public async Task<User> Register(RegistrationRequestDTO registrationRequestDto)
    {
        User user = new User()
        {
            UserName = registrationRequestDto.UserName,
            Name = registrationRequestDto.Name,
            Password = registrationRequestDto.Password,
            Role = registrationRequestDto.Role
        };
        await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();
        user.Password = "";
        return user;
    }

    public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDto)
    {
        var user = await _db.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == loginRequestDto.UserName.ToLower()
        && x.Password == loginRequestDto.Password);

        if (user == null)
        {
            return new LoginResponseDTO()
            {
                Token = "",
                User = null
            };
        }

        // if user was found generate then the JWT Token
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secretkey);
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        LoginResponseDTO loginResponse = new LoginResponseDTO()
        {
            Token = tokenHandler.WriteToken(token),
            User = user
        };
        return loginResponse;
    }
}