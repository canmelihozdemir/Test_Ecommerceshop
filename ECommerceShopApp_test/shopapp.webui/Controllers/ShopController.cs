using System.Linq;
using Microsoft.AspNetCore.Mvc;
using shopapp.business.Abstract;
using shopapp.entity;
using shopapp.webui.Models;
using shopapp.webui.ViewModels;

namespace shopapp.webui.Controllers
{
    public class ShopController : Controller
    {
        private IProductService _productService;
        public ShopController(IProductService productService)
        {
            this._productService = productService;
        }
        public IActionResult List(string category)
        {
            var productListViewModel = new ProductListViewModel()
            {
                Products = _productService.GetProductsByCategory(category)
            };

            return View(productListViewModel);
        }

        // public IActionResult Details(int? id)
        public IActionResult Details(string urlName)//startup içindeki değişken adı urlName
        {
            // if (id == null)
            // {
            //     return NotFound();
            // }

            if (urlName == null)
            {
                return NotFound();
            }

            // Product product = _productService.GetById((int)id);//nullable değil o yüzden int görmesi için cast ettik
            // Product product = _productService.GetProductDetails((int)id);
            Product product = _productService.GetProductDetails(urlName);



            if (product == null)
            {
                return NotFound();
            }
            // return View(product);
            return View(new ProductDetailModel()
            {
                Product = product,
                Categories = product.ProductCategories.Select(i => i.Category).ToList()
            });

        }
    }
}