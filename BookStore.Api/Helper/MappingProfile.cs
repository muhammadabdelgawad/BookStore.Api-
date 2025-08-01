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
            CreateMap<Category, CategoryResponseDto>()
                .ForMember(CR => CR.Name, O => O.MapFrom(C => C.CatName))
                .ForMember(CR => CR.CategoryOrder, O => O.MapFrom(C => C.CatOrder));

            CreateMap<CreateCategoryDto, Category>();
            CreateMap<UpdateCategoryDto, Category>().ReverseMap();
            // .ForMember(PR=>PR.Brand, O=>O.MapFrom(P=>P.Brand.Name))
            //.ForMember(PR => PR.Type, O => O.MapFrom(P => P.Type.Name))


            //Mapping Product
            CreateMap<Product, ProductResponseDto>()
                .ForMember(PR => PR.Author, O => O.MapFrom(P=> P.Author))
                .ForMember(PR => PR.CategoryName, O => O.MapFrom(P=> P.Category))
                .ForMember(PR => PR.CategoryId, O => O.MapFrom(P=> P.Id))
                .ForMember(PR => PR.Price, O => O.MapFrom(P=> P.Price))
                .ForMember(PR => PR.Title, O => O.MapFrom(P=> P.Title))
                .ForMember(PR => PR.Description, O => O.MapFrom(P=> P.Description));
            CreateMap<CreateProductDto,Product>();
            CreateMap<UpdateProductDto,Product>().ReverseMap();

               

        }
    }
}
