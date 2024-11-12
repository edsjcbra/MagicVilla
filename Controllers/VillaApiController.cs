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
    private readonly IVillaRepository _db;
    private readonly IMapper _mapper;
    public VillaApiController(IVillaRepository db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
    {
        IEnumerable<Villa> villasList = await _db.GetAllAsync();
        return Ok(_mapper.Map<List<VillaDTO>>(villasList));
    }
    
    [HttpGet("{id:int}", Name = "GetVilla")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VillaDTO?>> GetVilla(int id)
    {
        if (id == 0)
        {
            return BadRequest();
        }

        var villa = await _db.GetAsync(x => x.Id == id);
        if (villa == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<VillaDTO>(villa));
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VillaDTO?>> CreateVilla([FromBody] CreateVillaDTO villaToCreate)
    {
        if (await _db.GetAsync(x => x.Name.ToLower() == villaToCreate.Name.ToLower()) != null)
        {
            ModelState.AddModelError("CustomError", "Villa already exists !");
            return BadRequest(ModelState);
        }
        if (villaToCreate == null)
        {
            return BadRequest();
        }

        Villa villaToDb = _mapper.Map<Villa>(villaToCreate);
        
        await _db.CreateAsync(villaToDb);

        return CreatedAtRoute("GetVilla", new{id = villaToDb.Id}, villaToDb);
    }
    
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateVilla(int id, [FromBody] UpdateVillaDTO villaToUpdate)
    {
        if (villaToUpdate == null || id != villaToUpdate.Id)
        {
            return BadRequest();
        }
        if (await _db.GetAsync(x => x.Name.ToLower() == villaToUpdate.Name.ToLower()) != null)
        {
            ModelState.AddModelError("CustomError", "Villa already exists !");
            return BadRequest(ModelState);
        }

        Villa villaUpdatedToDb = _mapper.Map<Villa>(villaToUpdate);
        
        await _db.UpdateAsync(villaUpdatedToDb);

        return NoContent();
    }
    
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteVilla(int id)
    {
        if (id == 0)
        {
            return BadRequest();
        }
        var villaToDelete = await _db.GetAsync(x => x.Id == id);
        if (villaToDelete == null)
        {
            return NotFound();
        }
        await _db.RemoveAsync(villaToDelete);
        return NoContent();
    }
}