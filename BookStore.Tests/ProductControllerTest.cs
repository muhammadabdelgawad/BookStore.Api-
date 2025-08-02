using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BookStore.Api.Controllers;
using BookStore.Api.DTOs.CategoryDto;
using BookStore.Api.DTOs.ProductDto;
using Data_Access.Repositories;
using Data_Access.UnitOfWork;
using FakeItEasy;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Models.Entities;
using Moq;

namespace BookStore.Tests
{
    public class ProductControllerTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ProductController _controller;

        public ProductControllerTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _controller = new ProductController(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        #region Get Tests 

        [Fact]
        public async Task GetAll_ReturnListOfProduct()
        {
            var products = new List<Product> { new Product { Id = 1, Title = "Book1", Category = new Category { CatName = "Fiction" } } };
            _unitOfWorkMock.Setup(u => u.Products.GetAllProductsAsync()).ReturnsAsync(products);

            var result = await _controller.GetAllProductsV1();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<ProductResponseDto>>(okResult.Value);
        }

        [Fact]
        public async Task GetById_ReturnsProduct_WhenFoundProduct()
        {
            var product = new Product { Id = 1, Title = "Test", Category = new Category { CatName = "TestCat" } };
            _unitOfWorkMock.Setup(u => u.Products.GetProductByIdAsync(It.IsAny<int>())).ReturnsAsync(product);

            var result = await _controller.GetProductById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<ProductResponseDto>(okResult);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenNotFoundProduct()
        {
            _unitOfWorkMock.Setup(u => u.Products.GetProductByIdAsync(It.IsAny<int>())).ReturnsAsync((Product)null);

            var result = await _controller.GetProductById(99);

            Assert.IsType<NotFoundResult>(result);
        }
        #endregion

        #region Create Tests
        [Fact]
        public async Task Create_ReturnsCreatedProduct()
        {
            // Arrange
            var dto = new CreateProductDto { Title = "New Product", Description = "Description", Author = "Author", Price = 10, CategoryId = 1 };
            var entity = new Product { Id = 1, Title = "New Product", Description = "Description", Author = "Author", Price = 10, CategoryId = 1 };
            var readDto = new ProductResponseDto {  Title = "New Product", Description = "Description", Author = "Author", Price = 10, CategoryId = 1, CategoryName = "Category" };

            var productRepoMock = new Mock<IProductRepository>();
            _unitOfWorkMock.Setup(u => u.Products).Returns(productRepoMock.Object);

            _mapperMock.Setup(m => m.Map<Product>(dto)).Returns(entity);
            _mapperMock.Setup(m => m.Map<ProductResponseDto>(entity)).Returns(readDto);

            productRepoMock.Setup(r => r.AddProductAync(It.IsAny<Product>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            // Act
            var result = await _controller.CreateProduct(dto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.IsType<ProductResponseDto>(createdResult.Value);
            Assert.Equal("New Product", ((ProductResponseDto)createdResult.Value).Title);
        }
        #endregion

        #region Update Tests


        [Fact]
        public async Task Update_ReturnsNoContent_WhenSuccess()
        {
            var dto = new UpdateProductDto { Title = "Updated Title" };
            var entity = new Product { Id = 1, Title = "Old Title" };

            _unitOfWorkMock.Setup(u => u.Products.GetProductByIdAsync(1)).ReturnsAsync(entity);

            var result = await _controller.Update(1, dto);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenProductMissing()
        {
            _unitOfWorkMock.Setup(u => u.Products.GetProductByIdAsync(99)).ReturnsAsync((Product)null);

            var result = await _controller.Update(99, new UpdateProductDto());

            Assert.IsType<NotFoundResult>(result);
        }
        #endregion

        #region Patch Tests
        [Fact]
        public async Task PatchProduct_ReturnsNoContent_WhenPatched()
        {
            var product = new Product { Id = 1, Title = "Title", Description = "Desc", Author = "A", Price = 10, CategoryId = 1 };
            _unitOfWorkMock.Setup(u => u.Products.GetProductByIdAsync(1)).ReturnsAsync(product);

            var patchDoc = new JsonPatchDocument<UpdateProductDto>();
            patchDoc.Replace(p => p.Title, "Updated Title");

            var result = await _controller.ProductPatch(1, patchDoc);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PatchProduct_ReturnsNotFound_WhenNotExist()
        {
            _unitOfWorkMock.Setup(u => u.Products.GetProductByIdAsync(999)).ReturnsAsync((Product)null);

            var result = await _controller.ProductPatch(999, new JsonPatchDocument<UpdateProductDto>());

            Assert.IsType<NotFoundResult>(result);
        }
        #endregion

        #region Delete Tests
        [Fact]
        public async Task Delete_ReturnsNoContent_WhenDeleted()
        {
            var product = new Product { Id = 1 };
            _unitOfWorkMock.Setup(u => u.Products.GetProductByIdAsync(1)).ReturnsAsync(product);

            var result = await _controller.Delete(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenMissing()
        {
            _unitOfWorkMock.Setup(u => u.Products.GetProductByIdAsync(999)).ReturnsAsync((Product)null);

            var result = await _controller.Delete(999);

            Assert.IsType<NotFoundResult>(result);
        }

        #endregion
    }
}
