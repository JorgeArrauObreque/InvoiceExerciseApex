using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class InvoiceDto
    {
        public int InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int DaysToDue { get; set; }
        public DateTime? PaymentDueDate { get; set; }

        public string CustomerRun { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }

        public bool IsConsistent { get; set; }

        public string Status { get; set; }
        public string StatusPayment { get; set; }

        public List<InvoiceItemDto> Items { get; set; } = new();
        public List<CreditNoteDto> CreditNotes { get; set; } = new();
        public PaymentDto? Payment { get; set; }

    }
}
