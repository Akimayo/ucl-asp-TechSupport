using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TechSupport.Backend.Models;
using TechSupport.Backend.Models.Entities;

namespace TechSupport.Backend.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IResolvingService _service;
        private readonly ILogger<Program> _logger;

        public IndexModel(IResolvingService resolvingService, ILogger<Program> programLogger)
        {
            this._service = resolvingService;
            this._logger = programLogger;
        }

        public IList<Report> AwaitingIssues { get; private set; }
        public int AwaitingCount { get; private set; }
        public int ResolvingCount { get; private set; }


        public async Task OnGetAsync()
        {
            AwaitingIssues = new List<Report>(await this._service.GetAwaitingReportsAsync());
            this.AwaitingCount = await this._service.CountAwaiting();
            this.ResolvingCount = await this._service.CountResolving();
        }
    }
}
