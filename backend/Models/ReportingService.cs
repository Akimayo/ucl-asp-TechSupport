using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechSupport.Backend.Hubs;
using TechSupport.Backend.Models.Entities;

namespace TechSupport.Backend.Models
{
    public class ReportingService : IReportingService
    {
        private IssuesContext Context { get; }
        private IHubContext<ReportsHub, IResolveHubClient> Signal { get; }
        public ReportingService(IssuesContext issuesContext, IHubContext<ReportsHub, IResolveHubClient> hub)
        {
            this.Context = issuesContext;
            this.Signal = hub;
        }

        public async Task<IEnumerable<Category>> ListCategoriesAsync()
        {
            return await this.Context.Categories.ToListAsync();
        }

        public async Task<IEnumerable<Product>> ListProductsForAsync(long category)
        {
            return await this.Context.Products.Where(p => p.CategoryId.Equals(category)).ToListAsync();
        }

        public async Task SaveReportAsync(Report report)
        {
            report.Product = await this.Context.Products.FirstOrDefaultAsync(p => p.Id.Equals(report.ProductId));
            await this.Context.Reports.AddAsync(report);
            await this.Context.SaveChangesAsync();
            //await this.Signal.NotifyReportAsync(report);
            await this.Signal.Clients.All.NewReportAsync(report.Id, report.Product.Name, report.Message, report.AttachmentType != null, report.Timestamp, report.IsAwaitingResolution);
        }
    }
}
