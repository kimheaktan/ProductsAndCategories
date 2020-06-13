using Microsoft.EntityFrameworkCore;

namespace CSharp_ProductsCategories.Models
{
    public class ProCatContext : DbContext
    {
        public ProCatContext(DbContextOptions options) : base(options) {}

        public DbSet<Category> Categories {get;set;}
        public DbSet<Product> Products {get;set;}
        public DbSet<Association> Associations {get;set;}
    }
}