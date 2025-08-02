using AutoMapper;
using BookStore.Api.DTOs.CategoryDto;
using BookStore.Api.Helper;
using Data_Access.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Models.Entities;

namespace BookStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        public CategoryController(IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache memoryCache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _memoryCache= memoryCache;
        }


       

        
        [HttpGet(Name = "Get All Categories")]
        public async Task<IActionResult> GetAllCategories([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5)
        {
            const string cacheKey = "categoryList";
            if (!_memoryCache.TryGetValue(cacheKey, out List<CategoryResponseDto> cachedCategories))
            {
                var categories = await _unitOfWork.Categories.GetAllCategoriesAsync();

                cachedCategories = _mapper.Map<List<CategoryResponseDto>>(categories);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));

                _memoryCache.Set(cacheKey, cachedCategories, cacheOptions);

                categories = categories
                    .OrderBy(c => c.CatOrder)
                    .ThenBy(c => c.CatName)
                    .ToList();


            }
            var totalItems = cachedCategories.Count;
            var pagedData = cachedCategories
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();


            var result = new Pagination<CategoryResponseDto>
            {

                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalItems,
                Data= pagedData

            };
            return Ok(result);
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid category id.");
            var category = await _unitOfWork.Categories.GetCategoryByIdAsync(id);
            if (category == null)
                return NotFound();
            var result = _mapper.Map<CategoryResponseDto>(category);
            return Ok(result);
        }

        
        [HttpPost(Name ="Create Category")]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
        {
            if (dto == null)
                return BadRequest("Category data is required.");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var category = _mapper.Map<Category>(dto);
            await _unitOfWork.Categories.AddCategoryAync(category);
            await _unitOfWork.CompleteAsync();
            var result = _mapper.Map<CategoryResponseDto>(category);
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, result);
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDto dto)
        {
            if (dto == null)
                return BadRequest("Category data is required.");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var category = await _unitOfWork.Categories.GetCategoryByIdAsync(id);
            if (category == null)
                return NotFound();
            _mapper.Map(dto, category);
            _unitOfWork.Categories.UpdateCategory(category);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }

       
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid category id.");
            var category = await _unitOfWork.Categories.GetCategoryByIdAsync(id);
            if (category == null)
                return NotFound();
            _unitOfWork.Categories.DeleteCategory(category);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }
    }
}
