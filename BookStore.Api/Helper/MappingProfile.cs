using AutoMapper;
using BookStore.Api.DTOs.CategoryDto;
using BookStore.Api.DTOs.ProductDto;
using Models.Entities;

namespace BookStore.Api.Helper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            //Mapping Category
            CreateMap<Category, CategoryResponseDto>();
            CreateMap<CategoryResponseDto, Category>();
            CreateMap<UpdateCategoryDto, Category>();


            //Mapping Product
            CreateMap<Product, ProductResponseDto>();
            CreateMap<CreateProductDto,Product>();
            CreateMap<UpdateProductDto,Product>();

               

        }
    }
}
