using FMS.Db;
using FMS.Models;
using FMS.Services;
using Microsoft.AspNetCore.Mvc;

namespace FMS.Controllers;

public class CarrierController : Controller
{
    private readonly ILogger<CarrierController> _logger;
    private readonly IService<Carrier> _service;

    public CarrierController(ILogger<CarrierController> logger, IService<Carrier> service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task<IActionResult> Index()
    {
        var carriers = await _service.GetAllAsync();
        var viewModel = new CarrierViewModel() { Carriers = carriers };
        return View(viewModel);
    }

    public IActionResult Create()
    {
        return View(new Carrier());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Carrier carrier)
    {
        if (ModelState.IsValid)
        {
            _logger.LogInformation("Creating new carrier: {Name}", carrier.Name);
            await _service.CreateAsync(carrier);
            return RedirectToAction(nameof(Index));
        }
        _logger.LogWarning("Invalid model state for creating carrier: {Name}", carrier.Name);
        return View(carrier);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var carrier = await _service.GetSingleAsync(c => c.Id == id);
        if (carrier == null)
        {
            _logger.LogWarning("Carrier with ID {Id} not found for deletion", id);
            return NotFound();
        }
        return View(carrier);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        _logger.LogInformation("Deleting carrier with ID {Id}", id);
        try
        {
            await _service.DeleteAsync(c => c.Id == id);
            return RedirectToAction(nameof(Index));
        }
        catch (KeyNotFoundException)
        {
            _logger.LogWarning("Carrier with ID {Id} not found for deletion", id);
            return NotFound();
        }
    }
}