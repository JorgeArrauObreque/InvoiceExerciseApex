using Application;
using Application.Dtos;
using Application.Dtos.Json;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace InvoiceExercise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly InvoicesServices _invoicesServices;
        public InvoicesController(InvoicesServices invoicesServices) 
        {
            _invoicesServices = invoicesServices;
        }
        [HttpPost("ImportJson")]
        public async Task<IActionResult> ImportJsonInvoices()
        {
            return Ok(await _invoicesServices.ImportFromJsonInvoices("C:\\Users\\nacho\\Downloads\\bd_exam_invoices.json"));
        }
        [HttpPost("Import")]
        public async Task<IActionResult> ImportInvoicesFromJson(IFormFile file)
        {
            return Ok();
        }
        [HttpGet("get_invoices_by_number")]
        public async Task<IActionResult> InvoicesNumber([FromQuery]long invoice_number)
        {
            return Ok(await _invoicesServices.InvoicesByNumber(invoice_number));
        }
        [HttpGet("get_invoices_by_status")]
        public async Task<IActionResult> InvoicesStatus([FromQuery] InvoiceStatus status)
        {
            return Ok(await _invoicesServices.InvoiceByStatus(status));
        }
        [HttpGet("get_invoices_by_status_payment")]
        public async Task<IActionResult> InvoicesStatusPayment([FromQuery] PaymentStatus status)
        {
            return Ok(await _invoicesServices.InvoiceByStatusPayment(status));
        }
    }
}
