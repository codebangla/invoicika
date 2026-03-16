using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebAPI.Models;
using System.Text.Json.Serialization;

public class CustomerInvoice
{
    [Key]
    public Guid CustomerInvoiceId { get; set; } = Guid.NewGuid();

    [Required]
    public Guid Customer_id { get; set; }

    [ForeignKey(nameof(Customer_id))]
    [JsonIgnore]
    public virtual Customer Customer { get; set; }

    public Guid User_id { get; set; }

    [ForeignKey(nameof(User_id))]
    [JsonIgnore]
    public virtual User User { get; set; }

    [Required]
    public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;

    [Required]
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;

    public DateTime? UpdateDate { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal SubTotalAmount { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal VatAmount { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalAmount { get; set; }

    public virtual ICollection<CustomerInvoiceLine> CustomerInvoiceLines { get; set; } = new List<CustomerInvoiceLine>();

    public virtual ICollection<CustomerInvoiceGroupLine> CustomerInvoiceGroupLines { get; set; } = new List<CustomerInvoiceGroupLine>();

    [Required]
    public Guid Vat_id { get; set; }

    [ForeignKey(nameof(Vat_id))]
    [JsonIgnore]
    public virtual VAT VAT { get; set; }

    [Required]
    public InvoiceStatus Status { get; set; } = InvoiceStatus.Draft;
}
