using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CSharp_ProductsCategories.Models
{
    public class Product
    {
        [Key]
        public int ProductId {get;set;}
        public string Name {get;set;}
        public string Description {get;set;}
        public decimal Price {get;set;}
        public List<Association> AssociatedCategories {get;set;} 
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
    }
}