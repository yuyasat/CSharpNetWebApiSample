
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiSample.Data;
using WebApiSample.Models;

namespace WebApiSample.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StoresController : ControllerBase
{
    private readonly ApiDbContext _context;

    public StoresController(ApiDbContext context)
    {
        _context = context;
    }

    // GET: api/Stores
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Store>>> GetStores()
    {
        var stores = await _context.Stores.ToListAsync();
        return stores;
    }
}
