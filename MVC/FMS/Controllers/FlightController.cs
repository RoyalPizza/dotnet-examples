using FMS.Db;
using FMS.Models;
using FMS.Services;
using Microsoft.AspNetCore.Mvc;

namespace FMS.Controllers;

public class FlightController : Controller
{
    private readonly ILogger<FlightController> _logger;
    private readonly IService<Flight> _service;

    public FlightController(ILogger<FlightController> logger, IService<Flight> service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task<IActionResult> Index()
    {
        var flights = await _service.GetAllAsync();
        var viewModel = new FlightViewModel() { Flights = flights };
        return View(viewModel);
    }
}
