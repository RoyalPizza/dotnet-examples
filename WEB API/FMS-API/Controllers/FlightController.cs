using FMS_API.Db;
using FMS_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FMS_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FlightController : ControllerBase
{
    private readonly ILogger<FlightController> _logger;
    private readonly IService<Flight> _flightService;

    public FlightController(ILogger<FlightController> logger, IService<Flight> flightService)
    {
        _logger = logger;
        _flightService = flightService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Flight>>> GetAllAsync(CancellationToken ct = default)
    {
        _logger.LogInformation("Fetching all flights");
        var flights = await _flightService.GetAllAsync(ct: ct);
        return Ok(flights);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Flight>> GetSingleAsync(int id, CancellationToken ct = default)
    {
        _logger.LogInformation("Fetching flight with id {Id}", id);
        var flight = await _flightService.GetSingleAsync(c => c.Id == id, ct);
        if (flight == null)
        {
            _logger.LogWarning("Flight with id {Id} not found", id);
            return NotFound();
        }
        return Ok(flight);
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync([FromBody] Flight flight, CancellationToken ct = default)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid flight data provided");
            return BadRequest(ModelState);
        }

        _logger.LogInformation("Creating new flight");
        await _flightService.CreateAsync(flight, ct);
        return CreatedAtAction(nameof(GetSingleAsync), new { id = flight.Id }, flight);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAsync(int id, [FromBody] Flight flight, CancellationToken ct = default)
    {
        if (!ModelState.IsValid || flight.Id != id)
        {
            _logger.LogWarning("Invalid flight data or id mismatch for id {Id}", id);
            return BadRequest(ModelState);
        }

        _logger.LogInformation("Updating flight with id {Id}", id);
        var existing = await _flightService.GetSingleAsync(c => c.Id == id, ct);
        if (existing == null)
        {
            _logger.LogWarning("Flight with id {Id} not found for update", id);
            return NotFound();
        }

        await _flightService.UpdateAsync(flight, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id, CancellationToken ct = default)
    {
        _logger.LogInformation("Deleting flight with id {Id}", id);
        var exists = await _flightService.ExistAsync(c => c.Id == id, ct);
        if (!exists)
        {
            _logger.LogWarning("Flight with id {Id} not found for deletion", id);
            return NotFound();
        }

        await _flightService.DeleteAsync(c => c.Id == id, ct);
        return NoContent();
    }
}