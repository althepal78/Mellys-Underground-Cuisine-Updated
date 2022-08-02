using AutoMapper;
using DAL.Context;
using Mellys_Underground_Cuisine.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Mellys_Underground_Cuisine.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CartController(AppDbContext Db,
            IMapper mapper,
            IWebHostEnvironment webHostEnvironment)
        {
            _db = Db;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

    }
}
