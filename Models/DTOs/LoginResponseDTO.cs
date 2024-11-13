namespace MagicVilla.Api.Models.DTOs;

public class LoginResponseDTO
{
    public User User { get; set; }
    public string Token { get; set; }
}