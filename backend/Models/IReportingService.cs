using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechSupport.Backend.Models.Entities;

namespace TechSupport.Backend.Models
{
    public interface IReportingService
    {
        Task<IEnumerable<Category>> ListCategoriesAsync();
        Task<IEnumerable<Product>> ListProductsForAsync(long category);
        Task SaveReportAsync(Report report);
    }
}
