namespace Domain.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public string? PaymentMethod { get; set; }
        public DateTime? PaymentDate { get; set; }

        // Navegación
        public Invoice Invoice { get; set; }
    }
}
