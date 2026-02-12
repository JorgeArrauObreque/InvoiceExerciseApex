using Application;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceExercise.Controllers
{
    /// <summary>
    /// Endpoints de reportes.
    /// </summary>
    [Route("api/reports")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly InvoicesServices _invoicesServices;
        public ReportsController(InvoicesServices invoiceService)
        {
            _invoicesServices = invoiceService;
        }
        /// <summary>Obtiene facturas vencidas ~30 días.</summary>
        /// <remarks>Retorna la lista de facturas que cumplen la condición de vencimiento.</remarks>
        [HttpGet("overdue-30")]
        public async Task<IActionResult> Overdue30()
        {
            return Ok(await _invoicesServices.GetOverdue());
        }
        /// <summary>Resumen por estado de pago.</summary>
        /// <remarks>Devuelve conteo y porcentaje para Paid/Overdue/Pending.</remarks>
        [HttpGet("pay-status-summary")]
        public async Task<ActionResult<List<(string Status, int Count, decimal Percent)>>> GetPayStatusSummary()
        {
            var rows = await _invoicesServices.ObtainsPayStatusSummary();
            return Ok(rows);
        }
        /// <summary>Obtiene facturas inconsistentes.</summary>
        /// <remarks>Retorna facturas con datos inválidos/inconsistentes según validaciones del negocio.</remarks>
        [HttpGet("inconsistent")]
        public async Task<IActionResult> Inconsistent()
        {
            return Ok(await _invoicesServices.ObtainsInconsistents());
        }
    }
}
