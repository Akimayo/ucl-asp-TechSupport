using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechSupport.Backend.Models;
using TechSupport.Backend.Models.Entities;

namespace TechSupport.Backend.Hubs
{
    public class ReportsHub : Hub<IResolveHubClient>
    {
        private readonly IResolvingService _service;
        public ReportsHub(IResolvingService resolvingService) : base()
        {
            this._service = resolvingService;
        }
        public async override Task OnConnectedAsync()
        {
            (int w, int r) = await this._service.GetCountsForClient();
            await this.Clients.Caller.StartupAsync(await this._service.GetReportsForClient(), w, r); // Not very safe, but whatever
            await base.OnConnectedAsync();
        }
        // Useless...
        public async Task NotifyReportAsync(Report reportData)
        {
            await this.Clients.All.NewReportAsync(reportData.Id, reportData.Product.Name, reportData.Message, reportData.AttachmentType != null, reportData.Timestamp, reportData.IsAwaitingResolution);
        }
        public async Task NotifyReportOpenedAsync(long reportId)
        {
            await this.Clients.All.ReportResolvingAsync(reportId);
        }
        public async Task NotifyReportClosedAsync(long reportId)
        {
            await this.Clients.All.ReportReopenedAsync(reportId);
        }
        public async Task NotifyReportResolvedAsync(long reportId, Report nextReportData)
        {
            if (nextReportData is null) await this.Clients.All.ReportResolvedAsync(reportId);
            else await this.Clients.All.ReportResolvedAsync(reportId, nextReportData.Id, nextReportData.Product.Name, nextReportData.Message, nextReportData.AttachmentType != null, nextReportData.Timestamp);
        }
    }
}
