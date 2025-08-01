using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BookStore.Api.DTOs.CategoryDto;
using Data_Access.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;

namespace BookStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CategoryController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/Category
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _unitOfWork.Categories.GetAllCategoriesAsync();
            var result = _mapper.Map<IEnumerable<CategoryResponseDto>>(categories);
            return Ok(result);
        }

        // GET: api/Category/{id}
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

        // POST: api/Category
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
        {
            if (dto == null)
                return BadRequest("Category data is required.");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var category = _mapper.Map<Category>(dto);
            await _unitOfWork.Categories.AddCategoryAync(category);
            await _unitOfWork.SaveAsync();
            var result = _mapper.Map<CategoryResponseDto>(category);
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, result);
        }

        // PUT: api/Category/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryDto dto)
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
            await _unitOfWork.SaveAsync();
            return NoContent();
        }

        // DELETE: api/Category/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid category id.");
            var category = await _unitOfWork.Categories.GetCategoryByIdAsync(id);
            if (category == null)
                return NotFound();
            _unitOfWork.Categories.DeleteCategory(category);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}
