using AutoMapper;
using BookStore.Api.DTOs.ProductDto;
using Data_Access.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;

namespace BookStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductController(IUnitOfWork unitOfWork, IMapper mapper)
        {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
        }


        [HttpGet("id")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _unitOfWork.Products.GetProductByIdAsync(id);
            return product is null ? NotFound() : Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto dto)
        {
            if (dto == null)
                return BadRequest("Product data is required.");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var product = _mapper.Map<Product>(dto);
            await _unitOfWork.Products.AddProductAync(product);
            await _unitOfWork.SaveAsync();
            
            var result = _mapper.Map<ProductResponseDto>(product);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, result);
        }


    }
}
