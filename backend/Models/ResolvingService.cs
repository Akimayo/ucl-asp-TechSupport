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
    public class ResolvingService : IResolvingService
    {
        private IssuesContext Context { get; }
        private IHubContext<ReportsHub, IResolveHubClient> Signal { get; }
        public ResolvingService(IssuesContext issuesContext, IHubContext<ReportsHub, IResolveHubClient> hub)
        {
            this.Context = issuesContext;
            this.Signal = hub;
        }

        public async Task<IEnumerable<Report>> GetAwaitingReportsAsync()
        {
            //return (await this.Context.Reports.Include(r => r.Product).Where(r => r.Resolution == null).OrderBy(r => r.Timestamp).ToListAsync()).Where(r => r.IsAwaitingResolution);
            return await this.Context.Reports.Include(r => r.Product).Where(r => r.Resolution == null).OrderBy(r => r.Timestamp).ToListAsync();
        }

        public async Task<Resolution> OpenReportAsync(long report)
        {
            Report r = await this.Context.Reports.Include(r => r.Product).Include(r => r.Product.Category).FirstOrDefaultAsync(r => r.Id.Equals(report));
            if (r.Resolution != null) throw new AccessViolationException("This report has already been resolved");
            r.Open = DateTime.Now;
            this.Context.Reports.Update(r);
            await this.Context.SaveChangesAsync();
            //await this.Signal.NotifyReportOpenedAsync(report);
            await this.Signal.Clients.All.ReportResolvingAsync(report);
            return new Resolution { Report = r };
        }

        public async Task CloseReportAsync(long report)
        {
            Report r = await this.Context.Reports.FirstOrDefaultAsync(r => r.Id.Equals(report));
            r.Open = null;
            this.Context.Reports.Update(r);
            await this.Context.SaveChangesAsync();
            //await this.Signal.NotifyReportClosedAsync(report);
            await this.Signal.Clients.All.ReportReopenedAsync(report);
        }

        public async Task SaveResolutionAsync(Resolution resolution)
        {
            Report r = await this.Context.Reports.Include(r => r.Resolution).FirstOrDefaultAsync(r => r.Id.Equals(resolution.Report.Id));
            resolution.Report = r;
            await this.Context.Resolutions.AddAsync(resolution);
            r.Open = DateTime.Now;
            this.Context.Reports.Update(r);
            await this.Context.SaveChangesAsync();
            Report next = (await this.Context.Reports.Include(r => r.Product).Where(r => r.Resolution == null).OrderBy(r => r.Timestamp).Take(10).ToListAsync()).ElementAtOrDefault(9);
            //await this.Signal.NotifyReportResolvedAsync(r.Id, next);
            if (next is null) await this.Signal.Clients.All.ReportResolvedAsync(r.Id);
            else await this.Signal.Clients.All.ReportResolvedAsync(r.Id, next.Id, next.Product.Name, next.Message, next.AttachmentType != null, next.Timestamp, next.IsAwaitingResolution);
        }

        public async Task<(byte[] data, string type)> GetReportAttachmentAsync(long report)
        {
            Report r = await this.Context.Reports.FirstOrDefaultAsync(r => r.Id.Equals(report));
            return (r.AttachmentBytes, r.AttachmentType);
        }

        public async Task<int> CountAwaiting()
        {
            return (await this.Context.Reports.Where(r => r.Resolution == null).ToListAsync()).Count(r => r.IsAwaitingResolution);
        }

        public async Task<int> CountResolving()
        {
            return (await this.Context.Reports.Where(r => r.Resolution == null).ToListAsync()).Count(r => !r.IsAwaitingResolution);
        }

        public async Task<object[]> GetReportsForClient()
        {
            return await this.Context.Reports.Where(r => r.Resolution == null).Include(r => r.Product).Take(10).Select(r => new { reportId = r.Id, productName = r.Product.Name, message = r.Message, hasAttachment = r.AttachmentType != null, timestamp = r.Timestamp, open = r.IsAwaitingResolution }).ToArrayAsync<object>();
        }

        public async Task<(int waiting, int resolving)> GetCountsForClient()
        {
            IList<Report> q = await this.Context.Reports.Where(r => r.Resolution == null).ToListAsync();
            return (q.Count(r => r.IsAwaitingResolution), q.Count(r => !r.IsAwaitingResolution));
        }
    }
}
