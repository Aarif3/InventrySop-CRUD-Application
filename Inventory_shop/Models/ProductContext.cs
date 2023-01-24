using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace InventryShop.Models
{
    public class ProductContext : DbContext
    {
        public DbSet<Product> Products { get; set;}

        public DbSet<Category> Categories { get;set;}

        public DbSet<CategoryList> CategoriesList { get; set;}
    }
}