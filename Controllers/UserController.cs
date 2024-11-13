using System.Net;
using MagicVilla.Api.Models;
using MagicVilla.Api.Models.DTOs;
using MagicVilla.Api.Repository.IRepository;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla.Api.Controllers;

[Route("api/UsersAuth")]
[ApiController]
public class UserController : Controller
{
    private readonly IUserRepository _userRepo;
    protected APIResponse _response;

    public UserController(IUserRepository userRepo)
    {
        _userRepo = userRepo;
        _response = new();
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
    {
        var loginResponse = await _userRepo.Login(loginRequest);
        if (loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token))
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorMessage.Add("Username or Password incorrect");
            return BadRequest(_response);
        }
        _response.StatusCode = HttpStatusCode.OK;
        _response.IsSuccess = true;
        _response.Data = loginResponse;
        return Ok(_response);
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO registrationRequest)
    {
        bool ifUserIsUnique = await _userRepo.IsUniqueUser(registrationRequest.UserName);
        if (!ifUserIsUnique)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorMessage.Add("Username already exists");
            return BadRequest(_response);
        }

        var user = _userRepo.Register(registrationRequest);
        if (user == null)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorMessage.Add("Error while registering");
            return BadRequest(_response);
        }
        _response.StatusCode = HttpStatusCode.OK;
        _response.IsSuccess = true;
        return Ok(_response);
    }
}