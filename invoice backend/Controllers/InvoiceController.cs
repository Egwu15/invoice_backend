using invoice_backend.DTOs.Invoice;
using invoice_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace invoice_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvoiceController(InvoiceService service) : ControllerBase
{
    [Authorize]
    [HttpPost("invoice")]
    public async Task<IActionResult> CreateInvoice(CreateInvoiceDto dto)
    {
        var invoice = await service.CreateInvoice(dto);

        return Ok(invoice);
    }
}