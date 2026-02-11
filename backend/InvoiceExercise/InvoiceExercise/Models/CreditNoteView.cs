using System.ComponentModel.DataAnnotations;

namespace InvoiceExercise.Models
{
    public class CreditNoteView
    {
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public int Invoice { get; set; }
    }
}
