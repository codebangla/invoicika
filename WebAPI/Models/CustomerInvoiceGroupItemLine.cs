using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebAPI.Models
{
    public class CustomerInvoiceGroupItemLine
    {
        [Key]
        public Guid GroupItemLineId { get; set; } = Guid.NewGuid();

        [Required]
        public Guid CustomerInvoiceGroupLine_id { get; set; }

        [ForeignKey(nameof(CustomerInvoiceGroupLine_id))]
        [JsonIgnore]
        public virtual CustomerInvoiceGroupLine CustomerInvoiceGroupLine { get; set; }

        [Required]
        public Guid Item_id { get; set; }

        [ForeignKey(nameof(Item_id))]
        [JsonIgnore]
        public virtual Item Item { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }
    }
}
