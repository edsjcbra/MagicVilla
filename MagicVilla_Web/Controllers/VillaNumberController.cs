using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Models.VM;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers;

public class VillaNumberController : Controller
{
    private readonly IVillaNumberService _villaNumberService;
    private readonly IVillaService _villaService;
    private readonly IMapper _mapper;

    public VillaNumberController(IVillaNumberService villaNumberService, IMapper mapper,
        IVillaService villaService)
    {
        _villaNumberService = villaNumberService;
        _villaService = villaService;
        _mapper = mapper;
    }
    //---------------------------------------------------------------
    //VIEWS
    public async Task<IActionResult> Index()
    {
        List<VillaNumberDTO> list = new();

        var response = await _villaNumberService.GetAllAsync<APIResponse>();
        if (response != null && response.IsSuccess)
        {
            list = JsonConvert.DeserializeObject<List<VillaNumberDTO>>(Convert.ToString(response.Result));
        }
        
        return View(list);
    }
    public async Task<IActionResult> Create()
    {
        //GETTING DATA FOR DROPDOWN LIST--------------------------------------------------
        VillaNumberCreateVM villaNumberVm = new();
        
        var response = await _villaService.GetAllAsync<APIResponse>();
        if (response != null && response.IsSuccess)
        {
            villaNumberVm.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>
                (Convert.ToString(response.Result)).Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
        }
        //--------------------------------------------------------------------------------
        return View(villaNumberVm);
    }
    public async Task<IActionResult> Update(int id)
    {
        var response = await _villaNumberService.GetAsync<APIResponse>(id);
        if (response != null && response.IsSuccess)
        {
            VillaNumberDTO model = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));
            return View(_mapper.Map<VillaNumberUpdateDTO>(model));
        }
        return NotFound();
    }
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _villaNumberService.GetAsync<APIResponse>(id);
        if (response != null && response.IsSuccess)
        {
            VillaNumberDTO model = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));
            return View(model);
        }
        return NotFound();
    }
    //---------------------------------------------------------------
    
    //---------------------------------------------------------------
    //ACTIONS
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateVM villaNumberToCreate)
    {
        if (ModelState.IsValid)
        {
            var response = await _villaNumberService.CreateAsync<APIResponse>(villaNumberToCreate.VillaNumber);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }
        }
        return View(villaNumberToCreate);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateDTO villaNumberToUpdate)
    {
        if (ModelState.IsValid)
        {
            var response = await _villaNumberService.UpdateAsync<APIResponse>(villaNumberToUpdate);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }
        }
        return View(villaNumberToUpdate);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteVillaNumber(VillaNumberDTO villaNumberToDelete)
    {
        var response = await _villaNumberService.DeleteAsync<APIResponse>(villaNumberToDelete.VillaNo);
        if (response != null && response.IsSuccess)
        {
            return RedirectToAction(nameof(Index));
        }
        return View(villaNumberToDelete);
    }
}