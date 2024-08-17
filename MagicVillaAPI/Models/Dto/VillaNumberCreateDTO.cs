using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicVillaAPI.Models;

public class VillaNumberCreateDTO
{
    [Required]
    public int VillaNo { get; set; }
    public string SpecialDetails { get; set; }
    [Required]
    public int VillaID { get; set; }
}