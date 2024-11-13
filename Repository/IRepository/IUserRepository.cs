using MagicVilla.Api.Models;
using MagicVilla.Api.Models.DTOs;

namespace MagicVilla.Api.Repository.IRepository;

public interface IUserRepository
{
    Task<bool> IsUniqueUser(string username);
    Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDto);
    Task<User> Register(RegistrationRequestDTO registrationRequestDto);
}