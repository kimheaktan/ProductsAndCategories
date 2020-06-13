using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CSharp_ProductsCategories.Models
{
    public class Category
    {
        [Key]
        public int CategoryId {get;set;}
        public string Name {get;set;}
        public List<Association> AssociatedProducts {get;set;} 
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
        
    }
}
