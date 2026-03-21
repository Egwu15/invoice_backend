using invoice_backend.Data;
using invoice_backend.DTOs.Invoice;
using invoice_backend.DTOs.Items;
using invoice_backend.Entities;
using invoice_backend.Enums;
using Microsoft.EntityFrameworkCore;

namespace invoice_backend.Services;

public class InvoiceService(ApplicationDbContext context)
{
    public async Task<InvoiceSummaryDto> CreateInvoice(CreateInvoiceDto dto, int userId)
    {
        var uuId = await GetInvoiceNumber();

        var invoiceItems = dto.Items.Select(item => new InvoiceItem
            { Name = item.Name, Amount = item.Amount, Quantity = item.Quantity }).ToList();


        var invoice = new Invoice
        {
            UserId = userId,
            ClientEmail = dto.ClientEmail,
            BillTo = dto.BillTo,
            ClientName = dto.ClientName,
            DueDate = dto.DueDate,
            SenderAddress = dto.SenderAddress,
            Status = InvoiceStatus.Draft,
            Description = dto.Description,
            InvoiceNumber = uuId,
            SendTo = dto.SendTo,
            Items = invoiceItems
        };

        await context.Invoices.AddAsync(invoice);
        await context.SaveChangesAsync();


        return new InvoiceSummaryDto
        {
            Id = invoice.Id,
            InvoiceNumber = invoice.InvoiceNumber,
            BillTo = invoice.BillTo,
            SendTo = invoice.SendTo,
            ClientName = invoice.ClientName,
            ClientEmail = invoice.ClientEmail,
            Status = invoice.Status,
            CreatedAt = invoice.CreatedAt,
            DueDate = invoice.DueDate,
            Description = invoice.Description,
            SenderAddress = invoice.SenderAddress
        };
    }

    private async Task<string> GetInvoiceNumber()
    {
        var idDoesNotExist = true;
        var random = new Random();
        var invoicePrefix = "INV-" + DateTime.UtcNow.ToString("yy-MM-dd") + "-";
        var newUid = random.GetHexString(4, true);
        while (idDoesNotExist)
        {
            var invoice =
                await context.Invoices.FirstOrDefaultAsync(invoice => invoice.InvoiceNumber == invoicePrefix + newUid);
            if (invoice is null) idDoesNotExist = false;
            else newUid = random.GetHexString(4, true);
        }

        return invoicePrefix + newUid.ToUpper();
    }

    public async Task<List<InvoiceSummaryDto>> GetUserInvoices(int userId, InvoiceStatus? status = null)
    {
        var invoices = context.Invoices
            .Where(invoice => invoice.UserId == userId);


        if (status.HasValue) invoices = invoices.Where(invoice => invoice.Status == status);

        var formatedInvoices = invoices.Select(invoice =>
            new InvoiceSummaryDto
            {
                Id = invoice.Id,
                CreatedAt = invoice.CreatedAt,
                Description = invoice.Description,
                SenderAddress = invoice.SenderAddress,
                BillTo = invoice.BillTo,
                ClientEmail = invoice.ClientEmail,
                ClientName = invoice.ClientName,
                DueDate = invoice.DueDate,
                InvoiceNumber = invoice.InvoiceNumber,
                SendTo = invoice.SendTo,
                Status = invoice.Status
            }
        );
        return await formatedInvoices.ToListAsync();
    }


    public async Task UpdateInvoice(UpdateInvoiceDto invoiceDto, int invoiceId, int userId)
    {
        var invoice = await context.Invoices.Include(invoice => invoice.Items)
            .FirstOrDefaultAsync(invoice => invoice.Id == invoiceId);
        if (invoice is null) throw new KeyNotFoundException("Invoice not found");
        if (invoice.UserId != userId)
            throw new UnauthorizedAccessException("You are not authorized to update this invoice");


        invoice.ClientEmail = invoiceDto.ClientEmail;
        invoice.BillTo = invoiceDto.BillTo;
        invoice.ClientName = invoiceDto.ClientName;
        invoice.DueDate = invoiceDto.DueDate;
        invoice.Description = invoiceDto.Description;
        invoice.SenderAddress = invoiceDto.SenderAddress;
        invoice.SendTo = invoiceDto.SendTo;
        invoice.Status = invoiceDto.Status;

        context.InvoiceItems.RemoveRange(invoice.Items);

        invoice.Items = invoiceDto.Items.Select(item => new InvoiceItem
        {
            Name = item.Name,
            Amount = item.Amount,
            Quantity = item.Quantity
        }).ToList();

        await context.SaveChangesAsync();
    }

    public async Task<InvoiceDetailDto> GetInvoice(int userId, int invoiceId)
    {
        var invoice = await context.Invoices.Include(invoice => invoice.Items)
            .FirstOrDefaultAsync(invoice => invoice.Id == invoiceId);
        if (invoice is null) throw new KeyNotFoundException("Invoice not found");
        if (invoice.UserId != userId)
            throw new UnauthorizedAccessException("You are not authorized to view this invoice");

        var invoiceItems = invoice.Items.Select(item => new ItemResponseDto
            { Name = item.Name, Amount = item.Amount, Quantity = item.Quantity });
        var invoiceDetails = new InvoiceDetailDto
        {
            Id = invoice.Id,
            Description = invoice.Description,
            Items = invoiceItems.ToList(),
            SenderAddress = invoice.SenderAddress,
            CreatedAt = invoice.CreatedAt,
            InvoiceNumber = invoice.InvoiceNumber,
            BillTo = invoice.BillTo,
            ClientEmail = invoice.ClientEmail,
            ClientName = invoice.ClientName,
            DueDate = invoice.DueDate,
            SendTo = invoice.SendTo,
            Status = invoice.Status
        };

        return invoiceDetails;
    }

    public async Task DeleteInvoice(int userId, int invoiceId)
    {
        var invoice = await context.Invoices.FirstOrDefaultAsync(invoice => invoice.Id == invoiceId);
        if (invoice is null) throw new KeyNotFoundException("Invoice not found");
        if (invoice.UserId != userId) throw new UnauthorizedAccessException();

        context.Invoices.Remove(invoice);
        await context.SaveChangesAsync();
    }
}