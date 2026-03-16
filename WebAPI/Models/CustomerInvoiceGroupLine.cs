using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebAPI.Models
{
    public class CustomerInvoiceGroupLine
    {
        [Key]
        public Guid InvoiceGroupLineId { get; set; } = Guid.NewGuid();

        [Required]
        public Guid CustomerInvoice_id { get; set; }

        [ForeignKey(nameof(CustomerInvoice_id))]
        [JsonIgnore]
        public virtual CustomerInvoice CustomerInvoice { get; set; }

        [Required]
        [StringLength(256)]
        public string Title { get; set; }

        [StringLength(1024)]
        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal SubTotalAmount { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal VatAmount { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }

        public virtual ICollection<CustomerInvoiceGroupItemLine> GroupItemLines { get; set; } = new List<CustomerInvoiceGroupItemLine>();
    }
}
