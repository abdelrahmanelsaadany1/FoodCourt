
using Domain.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Persistence.Data; // <-- your DbContext namespace

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly FoodCourtDbContext _context;

        public CategoriesController(FoodCourtDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CategoryCreateDto dto)
        {
            // Map DTO to Entity
            var category = new Category
            {
                Name = dto.Name,
                Items = new HashSet<Item>() // optional: initialize to avoid null issues
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return Ok("Category added successfully!");
        }
    }
}
