namespace Domain.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public int InvoiceNumber { get; set; } // Único
        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int DaysToDue { get; set; }
        public DateTime PaymentDueDate { get; set; }

        // Customer info (desnormalizado por simplicidad)
        public string CustomerRun { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }

        // Consistencia calculada al importar
        public bool IsConsistent { get; set; }

        // Relaciones
        public ICollection<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();
        public ICollection<CreditNote> CreditNotes { get; set; } = new List<CreditNote>();
        public Payment? Payment { get; set; }
    }
}
