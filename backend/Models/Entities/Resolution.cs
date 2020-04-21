using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace TechSupport.Backend.Models.Entities
{
    public class Resolution
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [StringLength(500)]
        public string Message { get; set; }
        [BindProperty]
        [NotMapped]
        public IFormFile Attachment
        {
            get
            {
                if (this.AttachmentBytes is null || this.AttachmentBytes.Length <= 0) return null;
                using MemoryStream ms = new MemoryStream(this.AttachmentBytes);
                return new FormFile(ms, 0, ms.Length, "resolution", $"resolved_{this.Report.Id}{this.AttachmentType}");
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
        public virtual Report Report { get; set; }
    }
}
