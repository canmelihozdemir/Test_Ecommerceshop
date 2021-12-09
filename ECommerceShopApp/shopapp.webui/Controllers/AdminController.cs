using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using shopapp.business.Abstract;
using shopapp.entity;
using shopapp.webui.Models;

namespace shopapp.webui.Controllers
{
    public class AdminController : Controller
    {
        IProductService _productService;
        public AdminController(IProductService productService)//burası public olmazsa hata veriyor
        {
            _productService = productService;
        }

        public IActionResult ProductList()
        {
            return View(new ProductListViewModel()
            {
                Products = _productService.GetAll()
            });
        }

        public IActionResult CreateProduct()
        {

            return View();
        }

        [HttpPost]
        public IActionResult CreateProduct(ProductModel productModel)
        {
            var entity = new Product()
            {
                Name = productModel.Name,
                Url = productModel.Url,
                Price = productModel.Price,
                Description = productModel.Description,
                ImageUrl = productModel.ImageUrl,

            };
            _productService.Create(entity);

            // ViewData["message"] = $"{entity.Name} adlı ürün eklendi!";
            //controllerdan viewa gönderirken kullanabiliriz fakat redirect yaptığımız için viewdata  verisi gitmiş olur o yüzden tempdata kullandık
            // TempData["message"] = $"{entity.Name} adlı ürün eklendi!";
            // TempData["buttonColor"] = "success";

            var msg = new AlertMessage()
            {
                Message = $"{entity.Name} adlı ürün eklendi!",
                AlertType = "success"
            };
            TempData["message"] = JsonConvert.SerializeObject(msg);

            return RedirectToAction("ProductList");
        }

        [HttpGet]
        public IActionResult EditProduct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = _productService.GetById((int)id);

            if (entity == null)
            {
                return NotFound();
            }

            var model = new ProductModel()
            {
                ProductId = entity.ProductId,
                Name = entity.Name,
                Description = entity.Description,
                ImageUrl = entity.ImageUrl,
                Price = entity.Price,
                Url = entity.Url,
                IsApproved = entity.IsApproved,
                IsHome = entity.IsHome
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult EditProduct(ProductModel productModel)
        {
            var entity = _productService.GetById(productModel.ProductId);
            if (entity == null)
            {
                return NotFound();
            }

            entity.Name = productModel.Name;
            entity.Description = productModel.Description;
            entity.ImageUrl = productModel.ImageUrl;
            entity.Price = productModel.Price;
            entity.Url = productModel.Url;
            entity.IsApproved = productModel.IsApproved;

            _productService.Update(entity);

            // TempData["buttonColor"] = "primary";
            // TempData["message"] = $"{entity.Name} adlı ürün güncellendi!";


            var msg = new AlertMessage()
            {
                Message = $"{entity.Name} adlı ürün güncellendi!",
                AlertType = "primary"
            };
            TempData["message"] = JsonConvert.SerializeObject(msg);

            return RedirectToAction("ProductList");
        }

        [HttpPost]
        public IActionResult DeleteProduct(int? ProductId)
        {
            if (ProductId == null)
            {
                return NotFound();
            }
            var entity = _productService.GetById((int)ProductId);
            if (entity == null)
            {
                return NotFound();
            }

            _productService.Delete(entity);

            // TempData["buttonColor"] = "danger";
            // TempData["message"] = $"{entity.Name} adlı ürün silindi!";

            var msg = new AlertMessage()
            {
                Message = $"{entity.Name} adlı ürün silindi!",
                AlertType = "danger"
            };
            TempData["message"] = JsonConvert.SerializeObject(msg);

            return RedirectToAction("Products");
        }
    }
}