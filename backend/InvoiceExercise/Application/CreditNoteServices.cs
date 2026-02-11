using Application.Dtos;
using Application.Interfaces;
using Domain.Models;


namespace Application
{
    public class CreditNoteServices
    {
        private ICreditNoteRepository _repository;
        private IInvoiceRepository _invoiceRepository;
        public CreditNoteServices(ICreditNoteRepository repository,IInvoiceRepository invoiceRepository) 
        {
            _repository = repository;
            _invoiceRepository = invoiceRepository;
        }
        public async Task<bool> AddCreditNote(CreditNoteDto note)
        {
            //verificar existencia de la factura
            var result_invoice = await _invoiceRepository.GetInvoiceByNumber(note.InvoiceId);
            if (result_invoice == null) return false;
            //obtener total de crédito actual
            decimal credit_total = result_invoice.CreditNotes.Count >0 ?  result_invoice.CreditNotes.Sum(r => r.CreditNoteAmount):0;
            //si el total de créditos ya esta completo, no añade otra por que el pago fue completo
            if (credit_total == result_invoice.TotalAmount) return false;
            //suma de las notas de crédito actuales y el monto ingresado para la nueva NC
            decimal total_sum = note.CreditNoteAmount + credit_total;
            if (total_sum > result_invoice.TotalAmount) return false;
            return (await _repository.AddCreditNoteInvoice(new CreditNote() {CreditNoteAmount = note.CreditNoteAmount, InvoiceId = note.InvoiceId})) == null? false:true;
        }
    }
}
