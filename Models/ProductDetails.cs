using System.Collections.Generic;

namespace CSharp_ProductsCategories.Models
{
    public class ProductDetails
    {
        public Product Product {get;set;}
        public List<Category> AssociatedCategories {get;set;}

        public List<Category> UnAssociatedCategories{get;set;}

        public Association Association {get;set;}
    }
}