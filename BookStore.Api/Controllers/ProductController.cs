using System.Reflection;
using Asp.Versioning;
using AutoMapper;
using BookStore.Api.DTOs.CategoryDto;
using BookStore.Api.DTOs.ProductDto;
using Data_Access.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;

namespace BookStore.Api.Controllers
{
    [ApiVersion (1)]
    [ApiVersion (2)]
    [Route("api/v{v:apiVersion}/[controller]")]
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


        [MapToApiVersion(1)]
        [HttpGet]
        [ResponseCache(CacheProfileName = "Defualt60")]
        public async Task<IActionResult> GetAllProductsV1()
        {
            var products = await _unitOfWork.Products.GetAllProductsAsync();
            var result = _mapper.Map<IEnumerable<ProductResponseDto>>(products);
            return Ok(result);
        }

        [MapToApiVersion(2)]
        [HttpGet]
        public async Task<IActionResult> GetAllProductsV2()
        {
            var products = await _unitOfWork.Products.GetAllProductsAsync();
            var result = _mapper.Map<IEnumerable<ProductResponseDto>>(products);
            return Ok(result);
        }



        [HttpGet("{id}")]
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
            await _unitOfWork.CompleteAsync();
            
            var result = _mapper.Map<ProductResponseDto>(product);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, result);
        }


        [HttpPut("{id}")]

        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto dto)
        {
            if (dto == null)
                return BadRequest("Product data is required.");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var product = await _unitOfWork.Products.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();
            _mapper.Map(dto, product);
            _unitOfWork.Products.UpdateProduct(product);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid Product id.");
            var product = await _unitOfWork.Products.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();
            _unitOfWork.Products.DeleteProduct(product);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }


    }
}
