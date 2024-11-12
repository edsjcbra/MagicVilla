using MagicVilla.Api.Data;
using MagicVilla.Api.Models;
using MagicVilla.Api.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla.Api.Controllers;

[Route("api/VillaApi")]
[ApiController]
public class VillaApiController : ControllerBase
{
    private readonly AppDbContext _db;
    public VillaApiController(AppDbContext db)
    {
        _db = db;
    }
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
    {
        return Ok(await _db.Villas.ToListAsync());
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

        var villa = await _db.Villas.FirstOrDefaultAsync(x => x.Id == id);
        if (villa == null)
        {
            return NotFound();
        }
        return Ok(villa);
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VillaDTO?>> CreateVilla([FromBody] CreateVillaDTO villaToCreate)
    {
        if (await _db.Villas.FirstOrDefaultAsync(x => x.Name.ToLower() == villaToCreate.Name.ToLower()) != null)
        {
            ModelState.AddModelError("CustomError", "Villa already exists !");
            return BadRequest(ModelState);
        }
        if (villaToCreate == null)
        {
            return BadRequest();
        }

        Villa villaToDb = new()
        {
            Name = villaToCreate.Name,
            Details = villaToCreate.Details,
            Rate = villaToCreate.Rate,
            Occupancy = villaToCreate.Occupancy,
            Sqft = villaToCreate.Sqft,
            ImageUrl = villaToCreate.ImageUrl,
            Amenity = villaToCreate.Amenity
        };

        await _db.Villas.AddAsync(villaToDb);
        await _db.SaveChangesAsync();

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
        if (await _db.Villas.FirstOrDefaultAsync(x => x.Name.ToLower() == villaToUpdate.Name.ToLower()) != null)
        {
            ModelState.AddModelError("CustomError", "Villa already exists !");
            return BadRequest(ModelState);
        }
        Villa villaUpdatedToDb = new()
        {
            Id = villaToUpdate.Id,
            Name = villaToUpdate.Name,
            Details = villaToUpdate.Details,
            Rate = villaToUpdate.Rate,
            Occupancy = villaToUpdate.Occupancy,
            Sqft = villaToUpdate.Sqft,
            ImageUrl = villaToUpdate.ImageUrl,
            Amenity = villaToUpdate.Amenity,
            UpdatedDate = DateTime.Now
        };
        _db.Villas.Update(villaUpdatedToDb);
        await _db.SaveChangesAsync();

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
        var villaToDelete = await _db.Villas.FirstOrDefaultAsync(x => x.Id == id);
        if (villaToDelete == null)
        {
            return NotFound();
        }
        _db.Villas.Remove(villaToDelete);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}