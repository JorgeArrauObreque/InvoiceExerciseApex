using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Json
{
    public class InvoiceJsonDto
    {
        public int invoice_number { get; set; }
        public DateTime invoice_date { get; set; }
        public string invoice_status { get; set; }
        public decimal total_amount { get; set; }
        public int days_to_due { get; set; }
        public DateTime payment_due_date { get; set; }
        public string payment_status { get; set; }

        public List<InvoiceItemJsonDto> invoice_detail { get; set; }
        public InvoicePaymentJsonDto invoice_payment { get; set; }
        public List<CreditNoteJsonDto> invoice_credit_note { get; set; }
        public CustomerJsonDto customer { get; set; }
    }
}
