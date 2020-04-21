using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechSupport.Backend.Hubs
{
    public interface IResolveHubClient
    {
        Task NewReportAsync(long id, string productName, string message, bool hasAttachment, DateTime timestamp, bool open);
        Task ReportResolvingAsync(long id);
        Task ReportReopenedAsync(long id);
        Task ReportResolvedAsync(long id, long? nextId = null, string nextProductName = null, string nextMessage = null, bool? nextHasAttachment = null, DateTime? nextTimestamp = null, bool? open = null);
        Task StartupAsync(object[] reports, int totalWaiting, int totalResolving);
    }
}
