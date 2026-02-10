using Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
       public IActionResult ImportJsonInvoices()
        {
            return Ok(_invoicesServices.ImportFromJsonInvoices("C:\\Users\\nacho\\Downloads\\bd_exam_invoices.json"));
        }
    }
}
