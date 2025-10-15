using FMS_API.Db;
using FMS_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FMS_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PierController : ControllerBase
{
    private readonly ILogger<PierController> _logger;
    private readonly IService<Pier> _pierService;

    public PierController(ILogger<PierController> logger, IService<Pier> pierService)
    {
        _logger = logger;
        _pierService = pierService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Pier>>> GetAllAsync(CancellationToken ct = default)
    {
        _logger.LogInformation("Fetching all piers");
        var piers = await _pierService.GetAllAsync(ct: ct);
        return Ok(piers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Pier>> GetSingleAsync(int id, CancellationToken ct = default)
    {
        _logger.LogInformation("Fetching pier with id {Id}", id);
        var pier = await _pierService.GetSingleAsync(c => c.Id == id, ct);
        if (pier == null)
        {
            _logger.LogWarning("Pier with id {Id} not found", id);
            return NotFound();
        }
        return Ok(pier);
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync([FromBody] Pier pier, CancellationToken ct = default)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid pier data provided");
            return BadRequest(ModelState);
        }

        _logger.LogInformation("Creating new pier");
        await _pierService.CreateAsync(pier, ct);
        return CreatedAtAction(nameof(GetSingleAsync), new { id = pier.Id }, pier);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAsync(int id, [FromBody] Pier pier, CancellationToken ct = default)
    {
        if (!ModelState.IsValid || pier.Id != id)
        {
            _logger.LogWarning("Invalid pier data or id mismatch for id {Id}", id);
            return BadRequest(ModelState);
        }

        _logger.LogInformation("Updating pier with id {Id}", id);
        var existing = await _pierService.GetSingleAsync(c => c.Id == id, ct);
        if (existing == null)
        {
            _logger.LogWarning("Pier with id {Id} not found for update", id);
            return NotFound();
        }

        await _pierService.UpdateAsync(pier, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id, CancellationToken ct = default)
    {
        _logger.LogInformation("Deleting pier with id {Id}", id);
        var exists = await _pierService.ExistAsync(c => c.Id == id, ct);
        if (!exists)
        {
            _logger.LogWarning("Pier with id {Id} not found for deletion", id);
            return NotFound();
        }

        await _pierService.DeleteAsync(c => c.Id == id, ct);
        return NoContent();
    }
}