using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProductsManagement.Models;
using ProductsManagement.Repository.Interfaces;
using ProductsManagement.Repository.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ProductsManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductsManagementRepository _productsManagementRepository;
        private readonly IConfiguration Configuration;
        private readonly string _connectionString;

        public HomeController(ILogger<HomeController> logger, IProductsManagementRepository productsManagementRepository, IConfiguration _configuration)
        {
            _logger = logger;
            _productsManagementRepository = productsManagementRepository;
            Configuration = _configuration;
            _connectionString = this.Configuration.GetConnectionString("DefaultConnection");
        }

        public IActionResult Index(ListProductViewModel model)
        {
            List<Product> products = null;

            if (model.CategoryCode == 0)
                products = _productsManagementRepository.GetActiveProductsList(_connectionString);
            else
                products = _productsManagementRepository.GetActiveProductsListByCategory(model.CategoryCode, _connectionString);

            List<ProductViewModel> productsViewModel = new List<ProductViewModel>();
            int i = 0;
            ViewBag.Categories = GetCategories(null);

            foreach (var product in products)
            {
                i++;
                var productViewModel = new ProductViewModel()
                {
                    Sr_No = i,
                    ProdCode = product.ProductCode,
                    ProdName = product.Name,
                    CategoryCode = product.Category.CategoryCode,
                    CategoryName = product.Category.Name,
                    UnitPrice = product.UnitPrice,
                    ManufactureDate = product.ManufactureDate,
                };

                productsViewModel.Add(productViewModel);
            }

            model = new ListProductViewModel()
            {
                Products = productsViewModel,
                Categories = GetCategories(model.CategoryCode),
            };

            return View(model);
        }

        public IActionResult Create()
        {
            ProductViewModel model = new ProductViewModel();
            model.Categories = GetCategories(null);

            return View(model);
        }

        [HttpPost]
        public IActionResult Create([Bind] ProductViewModel model)
        {
            try
            {
                model.Categories = GetCategories(null);

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                Product product = new Product()
                {
                    Name = model.ProdName,
                    Active = model.Active,
                    AddedDate = DateTime.Now,
                    LastEditedDate = DateTime.Now,
                    ManufactureDate = model.ManufactureDate,
                    UnitPrice = model.UnitPrice,
                    Category = new Category() { CategoryCode = 1 }
                };

                _productsManagementRepository.AddOrUpdateProduct(product, "Add", 0, _connectionString);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewData["msg"] = ex.Message;
                model.Categories = GetCategories(null);

                return View(model);
            }            
        }

        public IActionResult Edit(int id)
        {
            var product = _productsManagementRepository.GetProductById(id, _connectionString);
            ProductViewModel model = new ProductViewModel()
            {
                UnitPrice = product.UnitPrice,
                ProdName = product.Name,
                CategoryCode = product.Category.CategoryCode,
                CategoryName = product.Category.Name,
                Active = product.Active,
                ManufactureDate = product.ManufactureDate,
                ProdCode = product.ProductCode,
            };

            model.Categories = GetCategories(product.Category.CategoryCode);

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(int id, [Bind] ProductViewModel model)
        {
            try
            {
                model.Categories = GetCategories(null);

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                Product product = new Product()
                {
                    Name = model.ProdName,
                    Active = model.Active,
                    AddedDate = DateTime.Now,
                    LastEditedDate = DateTime.Now,
                    ManufactureDate = model.ManufactureDate,
                    UnitPrice = model.UnitPrice,
                    Category = new Category() { CategoryCode = model.CategoryCode }
                };

                _productsManagementRepository.AddOrUpdateProduct(product, "Update", id, _connectionString);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewData["msg"] = ex.Message;
                model.Categories = GetCategories(null);

                return View(model);
            }
        }

        public IActionResult Delete(int id)
        {
            try
            {
                _productsManagementRepository.DeleteProductById(id, _connectionString);
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;
            }
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private List<SelectListItem> GetCategories(int? categoryCode)
        {
            var categories = _productsManagementRepository.GetAllCategories(_connectionString);
            var selectCategories = new List<SelectListItem>();

            foreach (var category in categories)
            {
                if(categoryCode.HasValue)
                    selectCategories.Add(new SelectListItem() { Text = category.Name, Value = category.CategoryCode.ToString(), Selected = true });
                else
                    selectCategories.Add(new SelectListItem() { Text = category.Name, Value = category.CategoryCode.ToString() });
            }

            return selectCategories;
        }
    }
}
