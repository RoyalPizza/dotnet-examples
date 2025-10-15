using FMS_API.Db;
using FMS_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FMS_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarrierController : ControllerBase
{
    private readonly ILogger<CarrierController> _logger;
    private readonly IService<Carrier> _carrierService;

    public CarrierController(ILogger<CarrierController> logger, IService<Carrier> carrierService)
    {
        _logger = logger;
        _carrierService = carrierService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Carrier>>> GetAllAsync(CancellationToken ct = default)
    {
        _logger.LogInformation("Fetching all carriers");
        var carriers = await _carrierService.GetAllAsync(ct: ct);
        return Ok(carriers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Carrier>> GetSingleAsync(int id, CancellationToken ct = default)
    {
        _logger.LogInformation("Fetching carrier with id {Id}", id);
        var carrier = await _carrierService.GetSingleAsync(c => c.Id == id, ct);
        if (carrier == null)
        {
            _logger.LogWarning("Carrier with id {Id} not found", id);
            return NotFound();
        }
        return Ok(carrier);
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync([FromBody] Carrier carrier, CancellationToken ct = default)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid carrier data provided");
            return BadRequest(ModelState);
        }

        _logger.LogInformation("Creating new carrier");
        await _carrierService.CreateAsync(carrier, ct);
        return CreatedAtAction(nameof(GetSingleAsync), new { id = carrier.Id }, carrier);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAsync(int id, [FromBody] Carrier carrier, CancellationToken ct = default)
    {
        if (!ModelState.IsValid || carrier.Id != id)
        {
            _logger.LogWarning("Invalid carrier data or id mismatch for id {Id}", id);
            return BadRequest(ModelState);
        }

        _logger.LogInformation("Updating carrier with id {Id}", id);
        var existing = await _carrierService.GetSingleAsync(c => c.Id == id, ct);
        if (existing == null)
        {
            _logger.LogWarning("Carrier with id {Id} not found for update", id);
            return NotFound();
        }

        await _carrierService.UpdateAsync(carrier, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id, CancellationToken ct = default)
    {
        _logger.LogInformation("Deleting carrier with id {Id}", id);
        var exists = await _carrierService.ExistAsync(c => c.Id == id, ct);
        if (!exists)
        {
            _logger.LogWarning("Carrier with id {Id} not found for deletion", id);
            return NotFound();
        }

        await _carrierService.DeleteAsync(c => c.Id == id, ct);
        return NoContent();
    }
}