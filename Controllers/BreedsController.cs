using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Wpm.Management.Api.DataAccess;

namespace Wpm.Management.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BreedsController(ManagementDbContext dbContext, ILogger<BreedsController> logger) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var breed = await dbContext.Breeds.ToListAsync();
        return breed != null ? Ok(breed) : NotFound();
    }

    [HttpGet("{id}", Name = nameof(GetBreedById))]
    public async Task<IActionResult> GetBreedById(int id)
    {
        var breed = await dbContext.Breeds.FindAsync(id);
        return breed == null ? NotFound() : Ok(breed);
    }

    [HttpPost]
    public async Task<IActionResult> Create(NewBreed newBreed)
    {
        try
        {
            var breed = newBreed.GetBreed();
            await dbContext.Breeds.AddAsync(breed);
            await dbContext.SaveChangesAsync();

            return CreatedAtRoute(nameof(GetBreedById), new { id = breed.Id }, breed);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}

public record NewBreed(string Name)
{
    public Breed GetBreed()
    {
        return new Breed(0, Name);
    }
}

