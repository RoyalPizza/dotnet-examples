using FMS.Db;
using FMS.Models;
using FMS.Services;
using Microsoft.AspNetCore.Mvc;

namespace FMS.Controllers;

public class PierController : Controller
{
    private readonly ILogger<PierController> _logger;
    private readonly IService<Pier> _service;

    public PierController(ILogger<PierController> logger, IService<Pier> service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task<IActionResult> Index()
    {
        var piers = await _service.GetAllAsync();
        var viewModel = new PierViewModel() { Piers = piers };
        return View(viewModel);
    }

    public IActionResult Create()
    {
        return View(new Pier());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Pier pier)
    {
        if (ModelState.IsValid)
        {
            _logger.LogInformation("Creating new pier: {Name}", pier.Name);
            await _service.CreateAsync(pier);
            return RedirectToAction(nameof(Index));
        }
        _logger.LogWarning("Invalid model state for creating pier: {Name}", pier.Name);
        return View(pier);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var pier = await _service.GetSingleAsync(p => p.Id == id);
        if (pier == null)
        {
            _logger.LogWarning("Pier with ID {Id} not found for deletion", id);
            return NotFound();
        }
        return View(pier);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        _logger.LogInformation("Deleting pier with ID {Id}", id);
        try
        {
            await _service.DeleteAsync(p => p.Id == id);
            return RedirectToAction(nameof(Index));
        }
        catch (KeyNotFoundException)
        {
            _logger.LogWarning("Pier with ID {Id} not found for deletion", id);
            return NotFound();
        }
    }
}
