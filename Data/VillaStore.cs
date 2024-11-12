using MagicVilla.Api.Models.DTOs;

namespace MagicVilla.Api.Data;

public static class VillaStore
{
    public static List<VillaDTO> VillasList = new List<VillaDTO>
    {
        new VillaDTO { Id = 1, Name = "Pool Villa", Occupancy = 4, Sqft = 100},
        new VillaDTO { Id = 2, Name = "Beach Villa", Occupancy = 3, Sqft = 300}
    };
}