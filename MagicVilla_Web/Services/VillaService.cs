﻿using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services;

public class VillaService : BaseService, IVillaService
{
    private readonly IHttpClientFactory _clientFactory;
    private string url;

    public VillaService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
    {
        _clientFactory = clientFactory;
        url = configuration.GetValue<string>("ServiceUrls:VillaAPI");
    }
    public Task<T> GetAllAsync<T>()
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.GET,
            Url = url+"/api/VillaAPI"
        });
    }

    public Task<T> GetAsync<T>(int id)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.GET,
            Url = url+"/api/VillaAPI/"+id
        });
    }

    public Task<T> CreateAsync<T>(VillaCreateDTO dto)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.POST,
            Data = dto,
            Url = url+"/api/VillaAPI"
        });
    }

    public Task<T> UpdateAsync<T>(VillaUpdateDTO dto)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.PUT,
            Data = dto,
            Url = url+"/api/VillaAPI/"+ dto.Id
        });
    }

    public Task<T> DeleteAsync<T>(int id)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.DELETE,
            Url = url+"/api/VillaAPI/"+id
        });
    }
}