namespace WebAPI.Dtos
{
    public class CustomerInvoiceDto
    {
        public Guid CustomerInvoiceId { get; set; }
        public Guid Customer_id { get; set; }
        public Guid User_id { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public decimal SubTotalAmount { get; set; }
        public decimal VatAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public int? Status { get; set; } = 0;
        public ICollection<CustomerInvoiceLineDto> CustomerInvoiceLines { get; set; } = new List<CustomerInvoiceLineDto>();
        public ICollection<CustomerInvoiceGroupLineDto> CustomerInvoiceGroupLines { get; set; } = new List<CustomerInvoiceGroupLineDto>();
        public Guid Vat_id { get; set; }
    }

    public class CustomerInvoiceLineDto
    {
        public Guid InvoiceLineId { get; set; }
        public Guid CustomerInvoice_id { get; set; }
        public Guid Item_id { get; set; }

        public string? ItemName { get; set; }
        public string? ItemDescription { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public class CustomerInvoiceGroupLineDto
    {
        public Guid InvoiceGroupLineId { get; set; }
        public Guid CustomerInvoice_id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public decimal SubTotalAmount { get; set; }
        public decimal VatAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public ICollection<CustomerInvoiceGroupItemLineDto> GroupItemLines { get; set; } = new List<CustomerInvoiceGroupItemLineDto>();
    }

    public class CustomerInvoiceGroupItemLineDto
    {
        public Guid GroupItemLineId { get; set; }
        public Guid CustomerInvoiceGroupLine_id { get; set; }
        public Guid Item_id { get; set; }
        public string? ItemName { get; set; }
        public string? ItemDescription { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
