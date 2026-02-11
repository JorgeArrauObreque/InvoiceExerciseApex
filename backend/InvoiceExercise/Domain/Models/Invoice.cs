using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Invoice
    {
        [Key] 
        public int InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int DaysToDue { get; set; }
        public DateTime? PaymentDueDate { get; set; }

        public string CustomerRun { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;

        public bool IsConsistent { get; set; }

        // FKs
        public ICollection<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();
        public ICollection<CreditNote> CreditNotes { get; set; } = new List<CreditNote>();
        public Payment? Payment { get; set; }
        public string Status {get;set;}
        public string StatusPayment { get; set;}
        public string CalculateStatus()
        {
            if (CreditNotes == null || !CreditNotes.Any())
                return InvoiceStatus.Issued.ToString();

            decimal totalNC = CreditNotes.Sum(cn => cn.CreditNoteAmount);
            return totalNC == TotalAmount ? InvoiceStatus.Cancelled.ToString() : InvoiceStatus.Partial.ToString();
        }

        public string CalculatePaymentStatus()
        {
            if (Payment != null) return Enums.PaymentStatus.Paid.ToString();

            DateTime limitDate = (PaymentDueDate.HasValue && PaymentDueDate.Value != DateTime.MinValue)
                ? PaymentDueDate.Value
                : InvoiceDate.AddDays(DaysToDue);

            return DateTime.Now > limitDate
                ? Enums.PaymentStatus.Overdue.ToString()
                : Enums.PaymentStatus.Pending.ToString();
        }
    }
}
