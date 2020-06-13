using System;
using System.Collections.Generic;
using System.Linq;
using CSharp_ProductsCategories.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSharp_ProductsCategories.Controllers
{
    public class CategoryController : Controller
    {
        private ProCatContext _db; 
        public CategoryController(ProCatContext context) 
        { 
            _db = context; 
        } 

        [HttpGet("categories")]
        public IActionResult AllCategories()
        {
            List<Category> allCategories = _db.Categories.Include(cat => cat.AssociatedProducts).ThenInclude(a => a.Product).ToList();

            return View(new ViewAndCreateCategory{Categories = allCategories});
        }

        // [HttpPost("")]
        public IActionResult CreateCategory(ViewAndCreateCategory newViewAndCreateCategoryCat)
        {
            _db.Categories.Add(newViewAndCreateCategoryCat.Category);
            _db.SaveChanges();

            return RedirectToAction("AllCategories");
        }

        [HttpGet("{CatName}/{CatId}")]
        //these params were passed from  asp-route-[] , the variable names have to match asp-route-[whatever]
        public IActionResult ViewSingleCategory(int CatId)
        //the int was passed from  asp-route-[] , the variable name has to match asp-route-[whatever]
        {   
            Category toView = GetCompleteCategory(CatId);
            // _db.Categories.Include(c => c.AssociatedProducts).ThenInclude(a => a.Category).FirstOrDefault(cat => cat.CategoryId == CatId);

            ViewCategoryDetails toViewDetails = new ViewCategoryDetails{ CurrentCategory = toView};

            ViewBag.AssociatedPros = _db.Categories.Include(cat => cat.AssociatedProducts).ThenInclude(a => a.Product).Where(c => c.CategoryId == CatId).ToList();
            
            ViewBag.UnAssociatedPros = _db.Products.Include(p => p.AssociatedCategories).ThenInclude(a => a.Category).Where(pro => pro.AssociatedCategories.All(a => a.CategoryId != CatId));

            // ViewBag.UnAssciatedPros = 
            return View(toViewDetails);
        }

        [HttpPost("AddProToCat")]
        public IActionResult AddProToCat(ViewCategoryDetails newViewCategoryDetail)
        {
            int ProductId = newViewCategoryDetail.Association.ProductId;
            int CategoryId = newViewCategoryDetail.Association.CategoryId;

            Category CurrCat = _db.Categories.FirstOrDefault(c => c.CategoryId == CategoryId);

            // Product toAddPro = _db.Products.FirstOrDefault(p => p.ProductId == ProductId);

            if(ProductId == 0)
            {
                TempData["alertMessage"] = "<p style='color:red;'>Please choose a product.</p>";

                return RedirectToAction("ViewSingleCategory", new{CatId = CategoryId, CatName =CurrCat.Name});
            }

            Association newAssociation = new Association{
                ProductId = ProductId ,
                CategoryId = CategoryId
            };
            
            _db.Associations.Add(newAssociation);
            _db.SaveChanges();


            return RedirectToAction("ViewSingleCategory", new{CatId = CategoryId, CatName =CurrCat.Name});

        }

        public Category GetCompleteCategory (int CatId) {
            Category retrievedCategory = _db.Categories
                .FirstOrDefault(cat => cat.CategoryId == CatId);
            HttpContext.Session.SetInt32("CategoryId", CatId);

            return retrievedCategory;
        }

    }
}