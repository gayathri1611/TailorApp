using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TailorApp.API.DTOs;
using TailorApp.API.Services;

namespace TailorApp.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class FabricInventoryController : ControllerBase
{
    private readonly IFabricInventoryService _fabricService;

    public FabricInventoryController(IFabricInventoryService fabricService)
    {
        _fabricService = fabricService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FabricInventoryDto>>> GetAll()
    {
        return Ok(await _fabricService.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<FabricInventoryDto>> GetById(int id)
    {
        var fabric = await _fabricService.GetByIdAsync(id);
        if (fabric == null) return NotFound();
        return Ok(fabric);
    }

    [HttpPost]
    public async Task<ActionResult<FabricInventoryDto>> Create(CreateFabricInventoryDto dto)
    {
        var fabric = await _fabricService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = fabric.FabricId }, fabric);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdateFabricInventoryDto dto)
    {
        if (!await _fabricService.UpdateAsync(id, dto)) return NotFound();
        return NoContent();
    }

    [HttpPatch("{id}/adjust-stock")]
    public async Task<ActionResult> AdjustStock(int id, AdjustStockDto dto)
    {
        if (!await _fabricService.AdjustStockAsync(id, dto)) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        if (!await _fabricService.DeleteAsync(id)) return NotFound();
        return NoContent();
    }
}
