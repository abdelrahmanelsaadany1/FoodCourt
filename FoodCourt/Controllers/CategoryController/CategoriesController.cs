
using AutoMapper;
using Domain.Dtos.CategoryDto;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace FoodCourt.Controllers.CategoryController
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly FoodCourtDbContext _context;
        readonly IMapper _mapper;
        public CategoriesController(FoodCourtDbContext context , IMapper Mapper)
        {
            _context = context;
            _mapper = Mapper;
        }

        [EnableQuery(
      MaxTop = 20,
      MaxSkip = 100,
      MaxExpansionDepth = 1,
      AllowedQueryOptions = AllowedQueryOptions.Filter |
                           AllowedQueryOptions.Skip|
                           AllowedQueryOptions.OrderBy,
      AllowedOrderByProperties = "Name"
  )]
        [HttpGet("getAll")]
        public IActionResult getAll()
        {
            IQueryable allData = _context.Categories.AsQueryable();
            return Ok(allData);
        }
        [HttpPost("getByName")]
        public async Task<IActionResult> getByName(CategoryCreateDto dto)
        {
            try
            {
                var result = await _context.Categories.FirstOrDefaultAsync(c=>c.Name == dto.Name);    
                var finalResult = _mapper.Map<CategoryCreateDto>(result);   
                return Ok(finalResult);
            }
            catch (Exception ex) { 
            
                 return BadRequest(ex.Message); 
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CategoryCreateDto dto)
        {
            // Map DTO to Entity
            var category =  _mapper.Map<CategoryCreateDto,Category>(dto);
            var createdCategory = _context.Categories.Add(category);

            await _context.SaveChangesAsync();

            return Ok("Category added successfully!");
        }
    }
}
