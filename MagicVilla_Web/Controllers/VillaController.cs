using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers;

public class VillaController : Controller
{
    private readonly IVillaService _villaService;
    private readonly IMapper _mapper;

    public VillaController(IVillaService villaService, IMapper mapper)
    {
        _villaService = villaService;
        _mapper = mapper;
    }
    //---------------------------------------------------------------
    //---------------------------------------------------------------
    //---------------------------------------------------------------
    //VIEWS
    public async Task<IActionResult> Index()
    {
        List<VillaDTO> list = new();

        var response = await _villaService.GetAllAsync<APIResponse>();
        if (response != null && response.IsSuccess)
        {
            list = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
        }
        
        return View(list);
    }
    public async Task<IActionResult> Create()
    {
        return View();
    }
    public async Task<IActionResult> Update(int id)
    {
        var response = await _villaService.GetAsync<APIResponse>(id);
        if (response != null && response.IsSuccess)
        {
            VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result));
            return View(_mapper.Map<VillaUpdateDTO>(model));
        }
        return NotFound();
    }
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _villaService.GetAsync<APIResponse>(id);
        if (response != null && response.IsSuccess)
        {
            VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result));
            return View(model);
        }
        return NotFound();
    }
    //---------------------------------------------------------------
    //---------------------------------------------------------------
    //---------------------------------------------------------------
    //ACTIONS
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateVilla(VillaCreateDTO villaToCreate)
    {
        if (ModelState.IsValid)
        {
            var response = await _villaService.CreateAsync<APIResponse>(villaToCreate);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Villa created successfully";
                return RedirectToAction(nameof(Index));
            }
        }
        return View(villaToCreate);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateVilla(VillaUpdateDTO villaToUpdate)
    {
        if (ModelState.IsValid)
        {
            var response = await _villaService.UpdateAsync<APIResponse>(villaToUpdate);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Villa Updated successfully";
                return RedirectToAction(nameof(Index));
            }
        }
        return View(villaToUpdate);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteVilla(VillaDTO villaToDelete)
    {
            var response = await _villaService.DeleteAsync<APIResponse>(villaToDelete.Id);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Villa Deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(villaToDelete);
    }
}