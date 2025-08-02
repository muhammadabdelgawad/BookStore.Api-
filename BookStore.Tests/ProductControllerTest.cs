using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BookStore.Api.Controllers;
using BookStore.Api.DTOs.CategoryDto;
using Data_Access.UnitOfWork;
using FakeItEasy;
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

       
    }
}
