using System.Collections.Generic;
using System.Linq;
using CSharp_ProductsCategories.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSharp_ProductsCategories.Controllers
{
    public class ProductController : Controller
    {        
        private ProCatContext _db; 
        public ProductController(ProCatContext context) 
        { 
            _db = context; 
        } 

        [HttpGet("products")]
        public IActionResult AllProducts()
        {
            List<Product> products= _db.Products.Include(p => p.AssociatedCategories).ThenInclude(a => a.Product).ToList();

            CreateAndAllProducts info = new CreateAndAllProducts{AllProducts = products};
            return View(info);
        }

        [HttpPost("create")]
        public IActionResult CreateProduct(CreateAndAllProducts newCreateAndAllProducts)
        {
            _db.Products.Add(newCreateAndAllProducts.Product);
            _db.SaveChanges();

            return RedirectToAction("AllProducts");
        }

        [HttpGet("product/{ProName}/{ProId}")]
        public IActionResult ViewOneProduct(int ProId)
        {
            Product toViewPro = GetAProduct(ProId);

            ViewBag.thisProduct =  _db.Products.Include(pro => pro.AssociatedCategories).ThenInclude(a => a.Category).Where(p => p.ProductId == ProId);

            List<Category> allUnassociated =  _db.Categories.Include(cat => cat.AssociatedProducts).Where(c => c.AssociatedProducts.All(a => a.ProductId != ProId )).ToList();

            ProductDetails details = new ProductDetails{Product = toViewPro,UnAssociatedCategories = allUnassociated};
            return View(details);
        }

        public Product GetAProduct(int ProId)
        {
            Product RetrievedProduct = _db.Products.FirstOrDefault(p => p.ProductId == ProId);

            return RetrievedProduct;
        }

        [HttpPost("AddCat")]
        public IActionResult AddCatToPro(ProductDetails newProductDetails)
        {

            int ProId = newProductDetails.Association.ProductId;
            Product CurrProduct = _db.Products.FirstOrDefault(p => p.ProductId == ProId);
            
            int CatId = newProductDetails.Association.CategoryId;

            if(CatId == 0)
            {
                TempData["Message"] = "<p style='color:red;'>Please choose a category.</p>";

                return RedirectToAction("ViewOneProduct", new{ProId = ProId, ProName = CurrProduct.Name});

            }
            else{

            Association newAssociation = new Association{
                ProductId = ProId,
                CategoryId = CatId
            };

            _db.Associations.Add(newAssociation);
            _db.SaveChanges();
            }

            return RedirectToAction("ViewOneProduct", new{ProId = ProId, ProName = CurrProduct.Name});
        }


    }
}