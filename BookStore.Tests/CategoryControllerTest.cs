using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BookStore.Api.Controllers;
using BookStore.Api.DTOs.CategoryDto;
using BookStore.Api.Helper;
using Data_Access.UnitOfWork;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Models.Entities;
using Moq;

namespace BookStore.Tests
{
    public class CategoryControllerTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CategoryController _controller;
        private readonly IMemoryCache _memoryCache;

        public CategoryControllerTest()
        {
             _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _controller = new CategoryController(_unitOfWorkMock.Object, _mapperMock.Object, _memoryCache);

        }
        #region Create Tests

        [Fact]
        public async Task Create_ReturnBadRequest_IfModelStateInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.Create(new CreateCategoryDto());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Create_ReturnCreatedAtAction_WhenIsValid()
        {
            // Arrange
            var createDto = new CreateCategoryDto { CatName = "NewCat", CatOrder = 1 };
            var category = new Category { Id = 4, CatName = "NewCat", CatOrder = 1 };
            var readDto = new CategoryResponseDto { Id = 4, Name = "NewCat", CategoryOrder = 1 };

            _mapperMock.Setup(m => m.Map<Category>(createDto)).Returns(category);
            _mapperMock.Setup(m => m.Map<CategoryResponseDto>(category)).Returns(readDto);
            _unitOfWorkMock.Setup(u => u.Categories.AddCategoryAync(category)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            // Act
            var result = await _controller.Create(createDto);

            // Assert
            var createdAt = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<CategoryResponseDto>(createdAt.Value);
            Assert.Equal(4, returnValue.Id);
        }
        #endregion

        #region Get Tests
        [Fact]
        public async Task GetPagedCategories_ReturnsPagedList()
        {
            // arrange
            var paginationParams = new Pagination<Category> { PageNumber = 1, PageSize = 5 };

            var pagedCategories = new List<Category>
            {
                new Category { Id = 1, CatName = "A", CatOrder = 1 },
                new Category { Id = 2, CatName = "B", CatOrder = 2 },
                new Category { Id = 3, CatName = "C", CatOrder = 3 },
                new Category { Id = 4, CatName = "D", CatOrder = 4 },
                new Category { Id = 5, CatName = "E", CatOrder = 5 }
            };

            var mappedDtos = new List<CategoryResponseDto>
            {
                new CategoryResponseDto { Id = 1, Name = "A", CategoryOrder = 1 },
                new CategoryResponseDto { Id = 2, Name = "B", CategoryOrder = 2 },
                new CategoryResponseDto { Id = 3, Name = "C", CategoryOrder = 3 },
                new CategoryResponseDto { Id = 4, Name = "D", CategoryOrder = 4 },
                new CategoryResponseDto { Id = 5, Name = "E", CategoryOrder = 5 },
            };                                                              

            _unitOfWorkMock.Setup(u => u.Categories.GetAllCategoriesAsync())
                .ReturnsAsync(pagedCategories);

            _mapperMock.Setup(m => m.Map<IEnumerable<CategoryResponseDto>>(pagedCategories))
                .Returns(mappedDtos);

            // Act
            var actionResult = await _controller.GetAllCategories();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<CategoryResponseDto>>(okResult.Value);
            Assert.Equal(5, ((List<CategoryResponseDto>)returnValue).Count);
        }
        [Fact]
        public async Task GetAll_ReturnsOk_WithCachedCategories()
        {
            // Arrange
            var cacheKey = "allCategories";
            var cachedList = new List<CategoryResponseDto>
            {
                new CategoryResponseDto { Id = 1, Name = "Cached", CategoryOrder = 1 }
            };

            _memoryCache.Set(cacheKey, cachedList, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(1)));

            // Act
            var result = await _controller.GetAllCategories();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<CategoryResponseDto>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_FromDatabase_AndSetsCache()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = 2, CatName  = "Database", CatOrder = 2 }
            };

            var categoryDTOs = new List<CategoryResponseDto>
            {
                new CategoryResponseDto { Id = 2, Name = "Database" }
            };

            _unitOfWorkMock.Setup(u => u.Categories.GetAllCategoriesAsync()).ReturnsAsync(categories);
            _mapperMock.Setup(m => m.Map<IEnumerable<CategoryResponseDto>>(categories)).Returns(categoryDTOs);

            // Act
            var result = await _controller.GetAllCategories();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<CategoryResponseDto>>(okResult.Value);
            Assert.Single(returnValue);
        }

        #endregion

        #region Get ById Tests
        // 2. GET BY ID
        [Fact]
        public async Task GetById_ReturnsNotFound_WhenCategoryIsNull()
        {
            // Arrange
            _unitOfWorkMock.Setup(u => u.Categories.GetCategoryByIdAsync(10)).ReturnsAsync((Category)null);

            // Act
            var result = await _controller.GetById(10);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetById_ReturnsCategory_WhenExists()
        {
            // Arrange
            var category = new Category { Id = 3, CatName = "Test" };
            var dto = new CategoryResponseDto { Id = 3, Name = "Test" };

            _unitOfWorkMock.Setup(u => u.Categories.GetCategoryByIdAsync(3)).ReturnsAsync(category);
            _mapperMock.Setup(m => m.Map<CategoryResponseDto>(category)).Returns(dto);

            // Act
            var result = await _controller.GetById(3);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<CategoryResponseDto>(okResult.Value);
            Assert.Equal(3, returnValue.Id);
        }


        #endregion


        #region Update Tests
        [Fact]
        public async Task Update_ReturnBadRequest_WhenModelInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.UpdateCategory(5, new UpdateCategoryDto());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Update_ReturnNotFound_WhenCategoryDoesNotExist()
        {
            // Arrange
            _unitOfWorkMock.Setup(u => u.Categories.GetCategoryByIdAsync(5)).ReturnsAsync((Category)null);

            // Act
            var result = await _controller.UpdateCategory(5, new UpdateCategoryDto());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
        [Fact]
        public async Task Update_ReturnNoContent_WhenSuccessful()
        {
            // Arrange
            var dto = new UpdateCategoryDto { CatName = "Updated", CatOrder = 2 };
            var entity = new Category { Id = 6, CatName = "Old", CatOrder = 1 };

            _unitOfWorkMock.Setup(u => u.Categories.GetCategoryByIdAsync(6)).ReturnsAsync(entity);
            _mapperMock.Setup(m => m.Map(dto, entity));
            _unitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            // Act
            var result = await _controller.UpdateCategory(6, dto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        #endregion


        #region Delete Tests

        [Fact]
        public async Task Delete_ReturnNotFound_WhenCategoryNotExist()
        {
            // Arrange
            _unitOfWorkMock.Setup(u => u.Categories.GetCategoryByIdAsync(7)).ReturnsAsync((Category)null);

            // Act
            var result = await _controller.Delete(7);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnNoContent_WhenDeleted()
        {
            // Arrange
            var entity = new Category { Id = 8, CatName = "DeleteMe" };

            _unitOfWorkMock.Setup(u => u.Categories.GetCategoryByIdAsync(8)).ReturnsAsync(entity);
            _unitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            // Act
            var result = await _controller.Delete(8);

            // Assert
            Assert.IsType<NoContentResult>(result);
        } 
        #endregion

    }
}
