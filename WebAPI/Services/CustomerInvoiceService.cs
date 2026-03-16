using WebAPI.Models;
using Microsoft.EntityFrameworkCore;
using WebAPI.Dtos;
using WebAPI.Data;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Helpers;
using System.Globalization;
using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;


namespace WebAPI.Services
{
    public interface ICustomerInvoiceService
    {
        Task<CustomerInvoiceDto> GetCustomerInvoiceByIdAsync(Guid id);
        Task<IEnumerable<CustomerInvoiceDto>> GetAllCustomerInvoicesAsync();
        Task CreateCustomerInvoiceAsync(CustomerInvoiceDto dto);
        Task<bool> UpdateCustomerInvoiceAsync(Guid id, CustomerInvoiceDto dto);
        Task DeleteCustomerInvoiceAsync(Guid id);
        Task<byte[]> GenerateInvoicePdfAsync(Guid invoiceId);
        Task SendInvoiceEmailAsync(Guid invoiceId);
    }

    public class CustomerInvoiceService : ICustomerInvoiceService
    {
        private readonly InvoicikaDbContext _context;
        private readonly IConfiguration _configuration;

        public CustomerInvoiceService(InvoicikaDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<CustomerInvoiceDto> GetCustomerInvoiceByIdAsync(Guid id)
        {
            var invoice = await _context.CustomerInvoices
                .Include(c => c.Customer)
                .Include(c => c.User)
                .Include(c => c.VAT)
                .Include(c => c.CustomerInvoiceLines)
                .ThenInclude(l => l.Item)
                .Include(c => c.CustomerInvoiceGroupLines)
                .ThenInclude(g => g.GroupItemLines)
                .ThenInclude(gi => gi.Item)
                .FirstOrDefaultAsync(c => c.CustomerInvoiceId == id);

            if (invoice == null) return null;

            // Map to DTO (consider using AutoMapper)
            return new CustomerInvoiceDto
            {
                CustomerInvoiceId = invoice.CustomerInvoiceId,
                Customer_id = invoice.Customer_id,
                User_id = invoice.User_id,
                InvoiceDate = invoice.InvoiceDate,
                CreationDate = invoice.CreationDate,
                UpdateDate = invoice.UpdateDate,
                SubTotalAmount = invoice.SubTotalAmount,
                VatAmount = invoice.VatAmount,
                TotalAmount = invoice.TotalAmount,
                Status = (int)invoice.Status,
                Vat_id = invoice.Vat_id,
                CustomerInvoiceLines = invoice.CustomerInvoiceLines.Select(l => new CustomerInvoiceLineDto
                {
                    InvoiceLineId = l.InvoiceLineId,
                    CustomerInvoice_id = l.CustomerInvoice_id,
                    Item_id = l.Item_id,
                    ItemName = l.Item.Name,
                    ItemDescription = l.Item.Description,
                    Quantity = l.Quantity,
                    Price = l.Price
                }).ToList(),
                CustomerInvoiceGroupLines = invoice.CustomerInvoiceGroupLines.Select(g =>
                    new CustomerInvoiceGroupLineDto
                    {
                        InvoiceGroupLineId = g.InvoiceGroupLineId,
                        CustomerInvoice_id = g.CustomerInvoice_id,
                        Title = g.Title,
                        Description = g.Description,
                        SubTotalAmount = g.SubTotalAmount,
                        VatAmount = g.VatAmount,
                        TotalAmount = g.TotalAmount,
                        GroupItemLines = g.GroupItemLines.Select(gi => new CustomerInvoiceGroupItemLineDto
                        {
                            GroupItemLineId = gi.GroupItemLineId,
                            CustomerInvoiceGroupLine_id = gi.CustomerInvoiceGroupLine_id,
                            Item_id = gi.Item_id,
                            ItemName = gi.Item.Name,
                            ItemDescription = gi.Item.Description,
                            Quantity = gi.Quantity,
                            Price = gi.Price
                        }).ToList()
                    }).ToList()
            };
        }

        public async Task<IEnumerable<CustomerInvoiceDto>> GetAllCustomerInvoicesAsync()
        {
            var invoices = await _context.CustomerInvoices
                .Include(c => c.Customer)
                .Include(c => c.User)
                .Include(c => c.VAT)
                .Include(c => c.CustomerInvoiceLines)
                .ThenInclude(l => l.Item)
                .Include(c => c.CustomerInvoiceGroupLines)
                .ThenInclude(g => g.GroupItemLines)
                .ThenInclude(gi => gi.Item)
                .ToListAsync();

            // Map to DTOs (consider using AutoMapper)
            return invoices.Select(invoice => new CustomerInvoiceDto
            {
                CustomerInvoiceId = invoice.CustomerInvoiceId,
                Customer_id = invoice.Customer_id,
                User_id = invoice.User_id,
                InvoiceDate = invoice.InvoiceDate,
                CreationDate = invoice.CreationDate,
                UpdateDate = invoice.UpdateDate,
                SubTotalAmount = invoice.SubTotalAmount,
                VatAmount = invoice.VatAmount,
                TotalAmount = invoice.TotalAmount,
                Status = (int)invoice.Status,
                Vat_id = invoice.Vat_id,
                CustomerInvoiceLines = invoice.CustomerInvoiceLines.Select(l => new CustomerInvoiceLineDto
                {
                    InvoiceLineId = l.InvoiceLineId,
                    CustomerInvoice_id = l.CustomerInvoice_id,
                    Item_id = l.Item_id,
                    ItemName = l.Item.Name,
                    ItemDescription = l.Item.Description,
                    Quantity = l.Quantity,
                    Price = l.Price
                }).ToList(),
                CustomerInvoiceGroupLines = invoice.CustomerInvoiceGroupLines.Select(g =>
                    new CustomerInvoiceGroupLineDto
                    {
                        InvoiceGroupLineId = g.InvoiceGroupLineId,
                        CustomerInvoice_id = g.CustomerInvoice_id,
                        Title = g.Title,
                        Description = g.Description,
                        SubTotalAmount = g.SubTotalAmount,
                        VatAmount = g.VatAmount,
                        TotalAmount = g.TotalAmount,
                        GroupItemLines = g.GroupItemLines.Select(gi => new CustomerInvoiceGroupItemLineDto
                        {
                            GroupItemLineId = gi.GroupItemLineId,
                            CustomerInvoiceGroupLine_id = gi.CustomerInvoiceGroupLine_id,
                            Item_id = gi.Item_id,
                            ItemName = gi.Item.Name,
                            ItemDescription = gi.Item.Description,
                            Quantity = gi.Quantity,
                            Price = gi.Price
                        }).ToList()
                    }).ToList()
            });
        }

        public async Task CreateCustomerInvoiceAsync(CustomerInvoiceDto dto)
        {
            InvoiceStatus invoiceStatus = dto.Status.HasValue ? (InvoiceStatus)dto.Status : InvoiceStatus.Draft;

            var invoice = new CustomerInvoice
            {
                CustomerInvoiceId = dto.CustomerInvoiceId == Guid.Empty ? Guid.NewGuid() : dto.CustomerInvoiceId,
                Customer_id = dto.Customer_id,
                User_id = dto.User_id,
                Vat_id = dto.Vat_id,
                InvoiceDate = dto.InvoiceDate,
                CreationDate = dto.CreationDate,
                UpdateDate = dto.UpdateDate,
                SubTotalAmount = dto.SubTotalAmount,
                VatAmount = dto.VatAmount,
                TotalAmount = dto.TotalAmount,
                Status = invoiceStatus,
                CustomerInvoiceLines = dto.CustomerInvoiceLines.Select(l => new CustomerInvoiceLine
                {
                    InvoiceLineId = l.InvoiceLineId == Guid.Empty ? Guid.NewGuid() : l.InvoiceLineId,
                    CustomerInvoice_id = l.CustomerInvoice_id,
                    Item_id = l.Item_id,
                    Quantity = l.Quantity,
                    Price = l.Price
                }).ToList(),
                CustomerInvoiceGroupLines = dto.CustomerInvoiceGroupLines.Select(g => new CustomerInvoiceGroupLine
                {
                    InvoiceGroupLineId = g.InvoiceGroupLineId == Guid.Empty ? Guid.NewGuid() : g.InvoiceGroupLineId,
                    CustomerInvoice_id = g.CustomerInvoice_id,
                    Title = g.Title,
                    Description = g.Description,
                    SubTotalAmount = g.SubTotalAmount,
                    VatAmount = g.VatAmount,
                    TotalAmount = g.TotalAmount,
                    GroupItemLines = g.GroupItemLines.Select(gi => new CustomerInvoiceGroupItemLine
                    {
                        GroupItemLineId = gi.GroupItemLineId == Guid.Empty ? Guid.NewGuid() : gi.GroupItemLineId,
                        CustomerInvoiceGroupLine_id = gi.CustomerInvoiceGroupLine_id,
                        Item_id = gi.Item_id,
                        Quantity = gi.Quantity,
                        Price = gi.Price
                    }).ToList()
                }).ToList()
            };

            _context.CustomerInvoices.Add(invoice);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Handle exception or log detailed error information here
                throw;
            }
        }

        public async Task<bool> UpdateCustomerInvoiceAsync(Guid id, CustomerInvoiceDto updatedInvoice)
        {
            var existingInvoice = await _context.CustomerInvoices
                .Include(i => i.CustomerInvoiceLines)
                .Include(i => i.CustomerInvoiceGroupLines)
                .ThenInclude(g => g.GroupItemLines)
                .FirstOrDefaultAsync(i => i.CustomerInvoiceId == id);

            if (existingInvoice == null) return false;

            existingInvoice.Customer_id = updatedInvoice.Customer_id;
            existingInvoice.User_id = updatedInvoice.User_id;
            existingInvoice.InvoiceDate = updatedInvoice.InvoiceDate;
            existingInvoice.UpdateDate = DateTime.UtcNow; //
            existingInvoice.SubTotalAmount = updatedInvoice.SubTotalAmount;
            existingInvoice.VatAmount = updatedInvoice.VatAmount;
            existingInvoice.TotalAmount = updatedInvoice.TotalAmount;
            existingInvoice.Vat_id = updatedInvoice.Vat_id;
            existingInvoice.Status = (InvoiceStatus)updatedInvoice.Status;

            // Update invoice lines
            existingInvoice.CustomerInvoiceLines.Clear(); //
            foreach (var line in updatedInvoice.CustomerInvoiceLines)
            {
                existingInvoice.CustomerInvoiceLines.Add(new CustomerInvoiceLine
                {
                    InvoiceLineId = line.InvoiceLineId == Guid.Empty ? Guid.NewGuid() : line.InvoiceLineId,
                    CustomerInvoice_id = line.CustomerInvoice_id,
                    Item_id = line.Item_id,
                    Quantity = line.Quantity,
                    Price = line.Price
                });
            }

            // Update group lines
            existingInvoice.CustomerInvoiceGroupLines.Clear();
            foreach (var groupLineDto in updatedInvoice.CustomerInvoiceGroupLines)
            {
                existingInvoice.CustomerInvoiceGroupLines.Add(new CustomerInvoiceGroupLine
                {
                    InvoiceGroupLineId = groupLineDto.InvoiceGroupLineId == Guid.Empty
                        ? Guid.NewGuid()
                        : groupLineDto.InvoiceGroupLineId,
                    CustomerInvoice_id = groupLineDto.CustomerInvoice_id,
                    Title = groupLineDto.Title,
                    Description = groupLineDto.Description,
                    SubTotalAmount = groupLineDto.SubTotalAmount,
                    VatAmount = groupLineDto.VatAmount,
                    TotalAmount = groupLineDto.TotalAmount,
                    GroupItemLines = groupLineDto.GroupItemLines.Select(gi => new CustomerInvoiceGroupItemLine
                    {
                        GroupItemLineId = gi.GroupItemLineId == Guid.Empty ? Guid.NewGuid() : gi.GroupItemLineId,
                        CustomerInvoiceGroupLine_id = gi.CustomerInvoiceGroupLine_id,
                        Item_id = gi.Item_id,
                        Quantity = gi.Quantity,
                        Price = gi.Price
                    }).ToList()
                });
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task DeleteCustomerInvoiceAsync(Guid id)
        {
            var invoice = await _context.CustomerInvoices.FindAsync(id);

            if (invoice == null) throw new KeyNotFoundException("Invoice not found");

            _context.CustomerInvoices.Remove(invoice);
            await _context.SaveChangesAsync();
        }


        public async Task<byte[]> GenerateInvoicePdfAsync(Guid invoiceId)
        {
            var invoice = await _context.CustomerInvoices
                .Include(c => c.Customer)
                .Include(c => c.User)
                .Include(c => c.VAT)
                .Include(c => c.CustomerInvoiceLines)
                .ThenInclude(l => l.Item)
                .Include(c => c.CustomerInvoiceGroupLines)
                .ThenInclude(g => g.GroupItemLines)
                .ThenInclude(gi => gi.Item)
                .FirstOrDefaultAsync(c => c.CustomerInvoiceId == invoiceId);

            if (invoice == null)
            {
                throw new ArgumentException("Invoice not found", nameof(invoiceId));
            }

            var companyInfo = new
            {
                CompanyName = "Invoicika Inc.",
                Address = "456 Business Road, Metropolis",
                Email = "info@invoicika.com",
                PhoneNumbers = new[] { "555-1010", "555-1020", "555-3030" }
            };

            var customerInfo = new
            {
                Name = invoice.Customer.Name,
                Address = invoice.Customer.Address,
                Email = invoice.Customer.Email,
                Phone = invoice.Customer.PhoneNumber
            };

            var invoiceDetails = new
            {
                InvoiceNumber = invoice.CustomerInvoiceId.ToString(),
                InvoiceDate = invoice.InvoiceDate.ToString("MMMM dd, yyyy", CultureInfo.InvariantCulture)
            };

            var vatPercentage = invoice.VAT.Percentage;
            // Calculations for subtotal, VAT, and total
            var subTotal = invoice.SubTotalAmount;
            var vat = subTotal * (invoice.VatAmount / 100);
            var total = invoice.TotalAmount;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(9));

                    page.Header().Row(row =>
                    {
                        row.ConstantItem(20).Image("wwwroot/uploads/invoicika.png").FitArea();
                        row.RelativeItem().Column(column =>
                        {
                            column.Item().Text(companyInfo.CompanyName).Bold().FontSize(18).AlignLeft();
                        });
                    });

                    page.Content().PaddingVertical(20).Column(column =>
                    {
                        column.Item().Row(row =>
                        {
                            row.RelativeItem().Column(columnLeft =>
                            {
                                columnLeft.Item().Text("From").Bold().FontSize(10).FontColor(Colors.Blue.Darken2);
                                columnLeft.Item().Text($"{companyInfo.CompanyName}").Bold().FontSize(12);
                                columnLeft.Item().Text($"{companyInfo.Address}");
                                columnLeft.Item().Text($"{companyInfo.Email}");
                                foreach (var phone in companyInfo.PhoneNumbers)
                                {
                                    columnLeft.Item().Text($"{phone}");
                                }
                            });

                            row.RelativeItem().Column(columnRight =>
                            {
                                columnRight.Item().Text("To").Bold().FontSize(10).FontColor(Colors.Blue.Darken2);
                                columnRight.Item().Text($"{customerInfo.Name}").Bold().FontSize(12);
                                columnRight.Item().Text($"{customerInfo.Address}");
                                columnRight.Item().Text($"{customerInfo.Email}");
                                columnRight.Item().Text($"{customerInfo.Phone}");
                            });

                            row.RelativeItem().Column(columnRight =>
                            {
                                columnRight.Item().Text("Invoice Number").Bold().FontSize(10)
                                    .FontColor(Colors.Blue.Darken2);
                                columnRight.Item().PaddingBottom(5).Text($"{invoiceDetails.InvoiceNumber}").Bold()
                                    .FontSize(9);
                                columnRight.Item().Text($"{invoiceDetails.InvoiceDate}");
                            });
                        });


                        column.Item().PaddingTop(5).PaddingBottom(15).LineHorizontal(1).LineColor(Colors.Blue.Medium);
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(100);
                                columns.RelativeColumn(180);
                                columns.RelativeColumn(40);
                                columns.RelativeColumn(60);
                                columns.RelativeColumn(70);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Item Name");
                                header.Cell().Element(CellStyle).Text("Description");
                                header.Cell().Element(CellStyle).AlignRight().Text("Quantity");
                                header.Cell().Element(CellStyle).AlignRight().Text("Unit Price");
                                header.Cell().Element(CellStyle).AlignRight().Text("Total");

                                static IContainer CellStyle(IContainer container)
                                {
                                    return container.DefaultTextStyle(x => x.Bold()).PaddingTop(10)
                                        .Background(Colors.Blue.Lighten2);
                                }
                            });

                            foreach (var line in invoice.CustomerInvoiceLines)
                            {
                                table.Cell().Element(CellStyle).Text(line.Item.Name);
                                table.Cell().Element(CellStyle).Text(line.Item.Description);
                                table.Cell().Element(CellStyle).AlignRight()
                                    .Text(line.Quantity.ToString("N0", CultureInfo.InvariantCulture));
                                table.Cell().Element(CellStyle).AlignRight()
                                    .Text($"{line.Price.ToString("F2", CultureInfo.InvariantCulture)}$");
                                table.Cell().Element(CellStyle).AlignRight().Text(
                                    $"{(line.Price * line.Quantity).ToString("F2", CultureInfo.InvariantCulture)}$");

                                static IContainer CellStyle(IContainer container)
                                {
                                    return container.BorderBottom(1).BorderColor(Colors.Blue.Lighten2)
                                        .PaddingVertical(3);
                                }
                            }

                            foreach (var group in invoice.CustomerInvoiceGroupLines)
                            {
                                table.Cell().ColumnSpan(5).Element(GroupHeaderStyle).Text(group.Title).Bold();

                                static IContainer GroupHeaderStyle(IContainer container)
                                {
                                    return container.PaddingTop(5).PaddingBottom(2).BorderBottom(1)
                                        .BorderColor(Colors.Grey.Lighten2);
                                }

                                foreach (var line in group.GroupItemLines)
                                {
                                    table.Cell().Element(GroupCellStyle).Text(line.Item.Name);
                                    table.Cell().Element(GroupCellStyle).Text(line.Item.Description);
                                    table.Cell().Element(GroupCellStyle).AlignRight()
                                        .Text(line.Quantity.ToString("N0", CultureInfo.InvariantCulture));
                                    table.Cell().Element(GroupCellStyle).AlignRight()
                                        .Text($"{line.Price.ToString("F2", CultureInfo.InvariantCulture)}$");
                                    table.Cell().Element(GroupCellStyle).AlignRight().Text(
                                        $"{(line.Price * line.Quantity).ToString("F2", CultureInfo.InvariantCulture)}$");

                                    static IContainer GroupCellStyle(IContainer container)
                                    {
                                        return container.BorderBottom(1).BorderColor(Colors.Blue.Lighten2)
                                            .PaddingVertical(3);
                                    }
                                }

                                table.Cell().ColumnSpan(4).Element(GroupFooterStyle).Text("Group Total").Italic();
                                table.Cell().Element(GroupFooterStyle).AlignRight()
                                    .Text($"{group.TotalAmount.ToString("F2", CultureInfo.InvariantCulture)}$").Bold();

                                static IContainer GroupFooterStyle(IContainer container)
                                {
                                    return container.PaddingVertical(2).BorderBottom(1)
                                        .BorderColor(Colors.Blue.Lighten2);
                                }
                            }
                        });

                        // Add Subtotal, VAT, and Total section at the bottom
                        column.Item().Table(table =>
                        {
                            // Define two columns: Labels and Values
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(100);
                                columns.RelativeColumn(180);
                                columns.RelativeColumn(40);
                                columns.RelativeColumn(60);
                                columns.RelativeColumn(70);
                            });

                            table.Cell().ColumnSpan(4).Element(LabelCellStyle).Text("Subtotal");
                            table.Cell().Element(ValueCellStyle).AlignRight()
                                .Text($"{subTotal.ToString("F2", CultureInfo.InvariantCulture)}$");

                            table.Cell().ColumnSpan(4).Element(LabelCellStyle).Text($"VAT ({vatPercentage}%)");
                            table.Cell().Element(ValueCellStyle).AlignRight()
                                .Text($"{vat.ToString("F2", CultureInfo.InvariantCulture)}$");

                            table.Cell().ColumnSpan(4).Element(LabelCellStyle).Text("Grand Total")
                                .FontColor(Colors.Blue.Darken2).Bold().FontSize(12);
                            table.Cell().Element(ValueCellStyle).AlignRight()
                                .Text($"{total.ToString("F2", CultureInfo.InvariantCulture)}$")
                                .FontColor(Colors.Blue.Darken2).Bold().FontSize(12);

                            static IContainer LabelCellStyle(IContainer container)
                            {
                                return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).AlignRight();
                            }

                            static IContainer ValueCellStyle(IContainer container)
                            {
                                return container.DefaultTextStyle(x => x.Bold()).PaddingVertical(5);
                            }
                        });
                    });

                    page.Footer().AlignCenter().Column(column =>
                    {
                        column.Item().PaddingTop(10).LineHorizontal(1).LineColor(Colors.Grey.Medium);
                        column.Spacing(2);
                        column.Item().Text("This computer-generated document is valid without signature.")
                            .AlignCenter();
                    });
                });
            });

            using (var stream = new MemoryStream())
            {
                document.GeneratePdf(stream);
                return await Task.FromResult(stream.ToArray());
            }
        }

        public async Task SendInvoiceEmailAsync(Guid invoiceId)
        {
            var invoiceDto = await GetCustomerInvoiceByIdAsync(invoiceId);

            if (invoiceDto == null)
            {
                throw new Exception("Invoice not found.");
            }

            var customer = await _context.Customers.FindAsync(invoiceDto.Customer_id);

            if (customer == null)
            {
                throw new Exception("Customer not found.");
            }

            var pdfBytes = await GenerateInvoicePdfAsync(invoiceId);

            // Get email configuration from app settings
            var smtpServer = _configuration["Email:SmtpServer"];
            var smtpPort = int.Parse(_configuration["Email:SmtpPort"]);
            var senderEmail = _configuration["Email:SenderEmail"];
            var senderName = _configuration["Email:SenderName"];
            var senderPassword = _configuration["Email:SenderPassword"];

            // Create a new email message
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(senderName, senderEmail));
            message.To.Add(new MailboxAddress("Recipient", customer.Email));
            message.Subject = $"{customer.Name}, your Invoice from Invoicika";

            // Create the email body with both text and HTML
            var bodyBuilder = new BodyBuilder
            {
                TextBody = "Please find the attached invoice.",
                HtmlBody = $@"
            <p>Dear {customer.Name},</p>
            <p>Thank you for your recent transaction with Invoicika. Your invoice is attached to this email for your reference.</p>
            <p>Best regards,</p>
            <strong>Invoicika Team</strong>"
            };

            // Attach the PDF invoice
            using (var stream = new MemoryStream(pdfBytes))
            {
                bodyBuilder.Attachments.Add($"invoice-{invoiceId}.pdf", stream.ToArray(),
                    new ContentType("application", "pdf"));
            }

            // Set the email body content
            message.Body = bodyBuilder.ToMessageBody();

            // Send the email via SMTP
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(senderEmail, senderPassword);

                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error sending email: {ex.Message}");
                }
            }
        }
    }
}