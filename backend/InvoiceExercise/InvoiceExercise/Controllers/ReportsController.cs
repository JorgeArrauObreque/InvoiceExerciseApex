using Application;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceExercise.Controllers
{
    [Route("api/reports")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly InvoicesServices _invoicesServices;
        public ReportsController(InvoicesServices invoiceService)
        {
            _invoicesServices = invoiceService;
        }

        [HttpGet("overdue-30")]
        public async Task<IActionResult> Overdue30()
        {
            return Ok(await _invoicesServices.GetOverdue());
        }

        [HttpGet("payment-status")]
        public async Task<IActionResult> PaymentStatus()
        {
     
            return Ok(await _invoicesServices.ObtainsPayStatusSummary());
        }

        [HttpGet("inconsistent")]
        public async Task<IActionResult> Inconsistent()
        {
            return Ok(await _invoicesServices.ObtainsInconsistents());
        }
    }
}
