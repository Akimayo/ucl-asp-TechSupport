using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TechSupport.Backend.Models;
using TechSupport.Backend.Models.Entities;

namespace TechSupport.Backend.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IResolvingService _service;

        public CreateModel(IResolvingService resolvingService)
        {
            this._service = resolvingService;
        }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            this.Resolution = await this._service.OpenReportAsync(id);
            return Page();
        }

        [BindProperty]
        public Resolution Resolution { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            // Validated manually to prevent deep validation to Report
            if (this.Resolution is null || string.IsNullOrEmpty(this.Resolution.Message) || this.Resolution.Report?.Id is null)
            {
                return Page();
            }
            await this._service.SaveResolutionAsync(this.Resolution);
            return RedirectToPage("Index");
        }

        public async Task<ActionResult> OnGetCloseAsync(long id)
        {
            await this._service.CloseReportAsync(id);
            return RedirectToPage("Index");
        }

        public async Task<ActionResult> OnGetDownloadAttachmentAsync(long id)
        {
            (byte[] data, string type) att = await this._service.GetReportAttachmentAsync(id);
            return File(att.data, "application/octet-stream", $"attachment_{id}{att.type}");
        }
    }
}
