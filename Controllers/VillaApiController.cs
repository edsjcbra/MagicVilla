using System.Net;
using AutoMapper;
using MagicVilla.Api.Models;
using MagicVilla.Api.Models.DTOs;
using MagicVilla.Api.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla.Api.Controllers;

[Route("api/VillaApi")]
[ApiController]
public class VillaApiController : ControllerBase
{
    protected APIResponse _response;
    private readonly IVillaRepository _db;
    private readonly IMapper _mapper;
    public VillaApiController(IVillaRepository db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
        _response = new();
    }
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse>> GetVillas()
    {
        try
        {
            IEnumerable<Villa> villasList = await _db.GetAllAsync();
            _response.Data = _mapper.Map<List<VillaDTO>>(villasList);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.ErrorMessage = new List<string>() { e.ToString() };
        }
        return _response;
    }
    
    [HttpGet("{id:int}", Name = "GetVilla")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResponse>> GetVilla(int id)
    {
        try
        {
            if (id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var villa = await _db.GetAsync(x => x.Id == id);
            if (villa == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                return BadRequest(_response);
            }

            _response.Data = _mapper.Map<VillaDTO>(villa);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.ErrorMessage = new List<string>() { e.ToString() };
        }
        return _response;
        
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] CreateVillaDTO villaToCreate)
    {
        try
        {
            if (await _db.GetAsync(x => x.Name.ToLower() == villaToCreate.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Villa already exists !");
                return BadRequest(ModelState);
            }
            if (villaToCreate == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            Villa villaToDb = _mapper.Map<Villa>(villaToCreate);
            await _db.CreateAsync(villaToDb);
            _response.Data = _mapper.Map<VillaDTO>(villaToDb);
            _response.StatusCode = HttpStatusCode.Created;

            return CreatedAtRoute("GetVilla", new{id = villaToDb.Id}, _response);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.ErrorMessage = new List<string>() { e.ToString() };
        }
        return _response;
    }
    
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] UpdateVillaDTO villaToUpdate)
    {
        try
        {
            if (villaToUpdate == null || id != villaToUpdate.Id)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            if (await _db.GetAsync(x => x.Name.ToLower() == villaToUpdate.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Villa already exists !");
                return BadRequest(ModelState);
            }

            Villa villaUpdatedToDb = _mapper.Map<Villa>(villaToUpdate);
        
            await _db.UpdateAsync(villaUpdatedToDb);
        
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.ErrorMessage = new List<string>() { e.ToString() };
        }
        return _response;
        
    }
    
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
    {
        try
        {
            if (id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            var villaToDelete = await _db.GetAsync(x => x.Id == id);
            if (villaToDelete == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                return BadRequest(_response);
            }
            await _db.RemoveAsync(villaToDelete);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.ErrorMessage = new List<string>() { e.ToString() };
        }
        return _response;
    }
}