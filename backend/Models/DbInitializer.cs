using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechSupport.Backend.Models.Entities;

namespace TechSupport.Backend.Models
{
    public class DbInitializer
    {
        public static void Initialize(IssuesContext context)
        {
            context.Database.EnsureCreated();
            if (context.Categories.Any()) return;
            Category[] cats = new Category[]
            {
                new Category {Name = "Alpha"},
                new Category {Name = "Beta"},
                new Category {Name = "Gamma"}
            };
            context.Categories.AddRange(cats);
            context.SaveChanges();
            context.Products.AddRange(
                new Product { Category = cats[0], Name = "A01" },
                new Product { Category = cats[0], Name = "A02" },
                new Product { Category = cats[1], Name = "B01" },
                new Product { Category = cats[1], Name = "B02" },
                new Product { Category = cats[2], Name = "G01" },
                new Product { Category = cats[2], Name = "G02" }
                );
            context.SaveChanges();
        }
    }
}
