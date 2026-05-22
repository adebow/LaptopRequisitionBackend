using LaptopRequisition.Application.DTOs;
using LaptopRequisition.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LaptopRequisition.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LaptopsController : ControllerBase
{
    private readonly ILaptopService _laptopService;

    public LaptopsController(ILaptopService laptopService)
    {
        _laptopService = laptopService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateLaptopDto dto)
    {
        var result = await _laptopService.CreateLaptopAsync(dto);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _laptopService.GetAllLaptopsAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _laptopService.GetLaptopByIdAsync(id);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateLaptopDto dto)
    {
        var result = await _laptopService.UpdateLaptopAsync(id, dto);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _laptopService.DeleteLaptopAsync(id);
        return NoContent();
    }
}