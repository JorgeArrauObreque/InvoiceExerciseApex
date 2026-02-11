using Application;
using Application.Dtos;
using InvoiceExercise.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceExercise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditNotesController : ControllerBase
    {
        private readonly CreditNoteServices _services;
        public CreditNotesController(CreditNoteServices services) 
        {
            _services = services;
        }
        [HttpPost("create_credit_note")]
        public async Task<IActionResult> CreateCreditNote([FromBody] CreditNoteView note)
        {
            try
            {
                var dto = new CreditNoteDto() { InvoiceId = note.Invoice, CreditNoteAmount = note.Amount };
                var result = await _services.AddCreditNote(dto);
                return Ok(result);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }

        }
    }
}
