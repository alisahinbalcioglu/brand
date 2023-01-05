using BrandService.Models;
using BrandService.Utils;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BrandService.Controllers;

[ApiController]
[Route("/api/v1/brands")]
public class BrandController : Controller
{
    private readonly AppDbContext _context;

    public BrandController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Brand>), 200)]
    [ProducesResponseType(typeof(void), 404)]
    [Produces("application/json")]
    public async Task<ActionResult<IEnumerable<Brand>>> GetAllBrands()
    {
        var brands = await _context.Brands.ToListAsync();

        if (brands.Count <= 0)
        {
            return NotFound();
        }

        return Ok(brands);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Brand), 200)]
    [ProducesResponseType(typeof(void), 404)]
    [Produces("application/json")]
    public async Task<ActionResult<Brand>> Get(int id)
    {
        var brand = await _context.Brands.FindAsync(id);
        
        if (brand == null)
        {
            return NotFound();
        }

        brand.Parent = _context.Brands.FirstOrDefault(x => x.Id == brand.ParentId);

        brand.Children = _context.Brands.Where(x => x.ParentId == brand.Id).ToArray();

        return Ok(brand);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Brand), 201)]
    [ProducesResponseType(typeof(void), 400)]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Post(Brand brand)
    {
        _context.Brands.Add(brand);

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = brand.Id }, brand);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(Brand), 201)]
    [ProducesResponseType(typeof(void), 400)]
    [ProducesResponseType(typeof(void), 404)]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Put(int id, Brand brand)
    {
        if (id != brand.Id)
        {
            return BadRequest();
        }

        _context.Entry(brand).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BrandExist(id)) return NotFound();
        }

        return Ok(brand);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(void), 204)]
    [ProducesResponseType(typeof(void), 400)]
    [ProducesResponseType(typeof(void), 404)]
    public async Task<IActionResult> Delete(int id)
    {
        var brand = await _context.Brands.FindAsync(id);

        if (brand == null)
        {
            return NotFound();
        }

        if (brand.Children is { Count: >= 1 })
        {
            return BadRequest();
        }

        _context.Brands.Remove(brand);

        await _context.SaveChangesAsync();

        return NoContent();
    }
    
    
    // Private validation methods
    private bool BrandExist(int id)
    {
        return _context.Brands.Any(e => e.Id == id);
    }
}