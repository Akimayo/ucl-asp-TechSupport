using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechSupport.Backend.Models.Entities;

namespace TechSupport.Backend.Models
{
    public interface IResolvingService
    {
        Task<IEnumerable<Report>> GetAwaitingReportsAsync();
        Task<Resolution> OpenReportAsync(long report);
        Task CloseReportAsync(long report);
        Task SaveResolutionAsync(Resolution resolution);
        Task<(byte[] data, string type)> GetReportAttachmentAsync(long report);
        Task<int> CountAwaiting();
        Task<int> CountResolving();
        Task<object[]> GetReportsForClient();
        Task<(int waiting, int resolving)> GetCountsForClient();
    }
}
