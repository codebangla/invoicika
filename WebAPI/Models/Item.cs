using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebAPI.Models
{
    public class Item
    {
        [Key]
        public Guid ItemId { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [StringLength(1024)]
        public string? Description { get; set; } 

        [Required]
        [Column(TypeName = "decimal(10,2)")] 
        public decimal PurchasePrice { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")] 
        public decimal SalePrice { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public Guid User_id { get; set; }
       
        [ForeignKey(nameof(User_id))]
        [JsonIgnore] 
        public virtual User? User { get; set; }

        [Required]
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

        public DateTime? UpdateDate { get; set; }
      
        public virtual ICollection<CustomerInvoiceLine> CustomerInvoiceLines { get; set; } = new List<CustomerInvoiceLine>();
    }
}
