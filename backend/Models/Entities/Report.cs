using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TechSupport.Backend.Models.Entities
{
    public class Report
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [StringLength(500)]
        public string Message { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [BindProperty]
        [NotMapped]
        public IFormFile Attachment
        {
            get
            {
                if (this.AttachmentBytes is null || this.AttachmentBytes.Length <= 0) return null;
                using MemoryStream ms = new MemoryStream(this.AttachmentBytes);
                return new FormFile(ms, 0, ms.Length, "attachment", $"attachment_{this.Id}{this.AttachmentType}");
            }
            set
            {
                using MemoryStream ms = new MemoryStream();
                value.CopyTo(ms);
                this.AttachmentBytes = ms.ToArray();
                this.AttachmentType = Path.GetExtension(value.FileName);
            }
        }
        [Column("Attachment")]
        public byte[] AttachmentBytes { get; set; }
        public string AttachmentType { get; set; }
        [Required]
        public DateTime Timestamp { get; set; } = DateTime.Now;

        public DateTime? Open { get; set; }

        [Required]
        public long ProductId { get; set; }
        public virtual Product Product { get; set; }

        public long? ResolutionId { get; set; }
        public virtual Resolution Resolution { get; set; } = null;

        [NotMapped]
        public bool IsAwaitingResolution { get { return this.Resolution is null && !this.Open.HasValue || this.Open?.AddHours(2) < DateTime.Now; } }
    }
}
