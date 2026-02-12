using Application;
using Application.Dtos;
using Application.Dtos.Json;
using Application.Exceptions;
using Domain.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace InvoiceExercise.Controllers
{
    /// <summary>
    /// Endpoints de facturas.
    /// </summary>
    /// 
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly InvoicesServices _invoicesServices;
        public InvoicesController(InvoicesServices invoicesServices) 
        {
            _invoicesServices = invoicesServices;
        }
        /// <summary>Importa facturas desde un archivo JSON.</summary>
        /// <remarks>
        /// Recibe un archivo (multipart/form-data) y lo procesa para insertar facturas.
        /// </remarks>
        /// <param name="file">Archivo JSON con facturas.</param>
        [HttpPost("Import")]
       
        public async Task<IActionResult> ImportInvoicesFromJson(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0) return BadRequest("Archivo vacío");

                using var stream = file.OpenReadStream();
                await _invoicesServices.ImportFromJson(stream);
                return Ok();
            }
            catch (DbUpdateException ex)
            {
                return BadRequest("ha ocurrido un error al insertar los datos");
            }
            catch (SqliteException ex)
            {
                return BadRequest("no se ha podido conectar con la base de datos");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>Obtiene facturas por número.</summary>
        /// <remarks>Devuelve una lista. Si no existe, devuelve 404 not found</remarks>
        /// <param name="invoice_number">Número de factura.</param>
        [HttpGet("get_invoices_by_number")]
        public async Task<IActionResult> InvoicesNumber([FromQuery]long invoice_number)
        {
            try
            {
                return Ok(await _invoicesServices.InvoicesByNumber(invoice_number));
            }
            catch (NotFoundAppException e)
            {
                return NotFound(e.Message);
            }
            catch (SqliteException ex)
            {
                return BadRequest("no se ha podido conectar con la base de datos");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        /// <summary>Obtiene facturas por estado (InvoiceStatus).</summary>
       /// <remarks>Devuelve una lista. Si no existe, devuelve 404 not found</remarks>
        /// <param name="status">Estado de la factura.</param>
        [HttpGet("get_invoices_by_status")]
        public async Task<IActionResult> InvoicesStatus([FromQuery] InvoiceStatus status)
        {
            try
            {
                return Ok(await _invoicesServices.InvoiceByStatus(status));
            }
            catch (NotFoundAppException e)
            {
                return NotFound(e.Message);
            }
            catch (SqliteException ex)
            {
                return BadRequest("no se ha podido conectar con la base de datos");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        /// <summary>Obtiene facturas por estado de pago (PaymentStatus).</summary>
        /// <remarks>Devuelve una lista. Si no existe, devuelve 404 not found</remarks>
        /// <param name="status">Estado de pago.</param>
        [HttpGet("get_invoices_by_status_payment")]
        public async Task<IActionResult> InvoicesStatusPayment([FromQuery] PaymentStatus status)
        {
            try
            {
                return Ok(await _invoicesServices.InvoiceByStatusPayment(status));
            }
            catch (NotFoundAppException e)
            {
                return NotFound(e.Message);
            }
            catch (SqliteException ex)
            {
                return BadRequest("no se ha podido conectar con la base de datos");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
