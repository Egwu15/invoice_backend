using System.Security.Claims;
using invoice_backend.DTOs.Invoice;
using invoice_backend.Enums;
using invoice_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace invoice_backend.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class InvoiceController(InvoiceService service) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<InvoiceSummaryDto>> Create(CreateInvoiceDto dto)
    {
        var userId = GetUserId();

        if (userId is null) return Unauthorized();

        var invoice = await service.CreateInvoice(dto, userId.Value);

        return Ok(invoice);
    }


    [HttpGet]
    public async Task<ActionResult<List<InvoiceSummaryDto>>> Invoices([FromQuery] InvoiceStatus? status)
    {
        var userId = GetUserId();
        if (userId is null) return Unauthorized();

        var invoices = await service.GetUserInvoices(userId.Value, status);
        return Ok(invoices);
    }

    [HttpPut("{invoiceId:int}")]
    public async Task<IActionResult> UpdateInvoice(UpdateInvoiceDto invoiceDto, int invoiceId)
    {
        var user = GetUserId();
        if (user is null) return Unauthorized();
        await service.UpdateInvoice(invoiceDto, invoiceId, user.Value);

        return Ok();
    }

    [HttpGet("{invoiceId:int}")]
    public async Task<ActionResult<InvoiceDetailDto>> GetInvoice(int invoiceId)
    {
        var user = GetUserId();
        if (user is null) return Unauthorized();
        var invoice = await service.GetInvoice(user.Value, invoiceId);
        return Ok(invoice);
    }

    [HttpDelete("{invoiceId:int}")]
    public async Task<IActionResult> DeleteInvoice(int invoiceId)
    {
        var user = GetUserId();
        if (user is null) return Unauthorized();
        await service.DeleteInvoice(user.Value, invoiceId);
        return Ok();
    }

    [HttpGet("stats")]
    public async Task<ActionResult<InvoiceStatsDto>> GetInvoiceStatus()
    {
        var user = GetUserId();
        if (user is null) return Unauthorized();
        var stats = await service.GetInvoiceStats(user.Value);
        return Ok(stats);
    }

    private int? GetUserId()
    {
        var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (user is null || !int.TryParse(user, out var id)) return null;
        return id;
    }
}