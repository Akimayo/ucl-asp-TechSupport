using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TechSupport.Backend.Models;
using TechSupport.Backend.Models.Entities;

namespace TechSupport.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [EnableCors("ApiPolicy")]
    public class ReportingController : ControllerBase
    {
        private readonly IReportingService _service;
        private readonly ILogger<Program> _logger;
        public ReportingController(IReportingService reportingService, ILogger<Program> programLogger)
        {
            this._service = reportingService;
            this._logger = programLogger;
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<Category>> Categories()
        {
            return await this._service.ListCategoriesAsync();
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<Product>> Products([FromQuery]int category)
        {
            return await this._service.ListProductsForAsync(category);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Report([FromForm]Report report)
        {
            if (!ModelState.IsValid) return BadRequest(new { success = false });
            else
            {
                try
                {
                    await this._service.SaveReportAsync(report);
                    return Ok(new { success = true });
                }
                catch (Exception ex)
                {
                    this._logger.LogWarning(ex, "Failed saving report", report);
                    return UnprocessableEntity(new { success = false });
                }
            }
        }
    }
}