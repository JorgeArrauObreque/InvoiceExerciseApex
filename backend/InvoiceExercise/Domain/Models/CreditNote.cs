namespace Domain.Models
{
    public class CreditNote
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public string CreditNoteNumber { get; set; }
        public DateTime CreditNoteDate { get; set; }
        public decimal CreditNoteAmount { get; set; }

        // Navegación
        public Invoice Invoice { get; set; }
    }
}

