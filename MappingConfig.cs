using AutoMapper;
using MagicVilla.Api.Models;
using MagicVilla.Api.Models.DTOs;

namespace MagicVilla.Api;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<Villa, VillaDTO>().ReverseMap();
        CreateMap<Villa, CreateVillaDTO>().ReverseMap();
        CreateMap<Villa, UpdateVillaDTO>().ReverseMap();
    }
}