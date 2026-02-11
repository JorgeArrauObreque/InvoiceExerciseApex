using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class CreditNote
    {
        [Key]
        public long CreditNoteNumber { get; set; }
        public int InvoiceId { get; set; }
       
        public DateTime CreditNoteDate { get; set; }
        public decimal CreditNoteAmount { get; set; }

        // Navegación
        public Invoice Invoice { get; set; }
    }
}

