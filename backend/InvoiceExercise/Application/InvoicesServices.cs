using Application.Dtos;
using Application.Interfaces;
using Domain.Models;

namespace Application
{
    public class InvoicesServices
    {
        private readonly IInvoicesJson _jsonReader; 
        private readonly IInvoiceRepository _invoiceRepository;

        public InvoicesServices(IInvoicesJson jsonreader, IInvoiceRepository invoiceRepository)
        {
            _jsonReader = jsonreader;
            _invoiceRepository = invoiceRepository;
        }

        public async Task<bool> ImportFromJsonInvoices(string path)
        {
            var json_invoices = _jsonReader.ReadInvoices(path);
            var facturasEntidad = json_invoices.Select(dto => new Invoice
            {
                InvoiceNumber = dto.invoice_number,
                InvoiceDate = dto.invoice_date,
                TotalAmount = dto.total_amount,
                DaysToDue = dto.days_to_due,
                PaymentDueDate = dto.payment_due_date,

                // Calcular consistencia: (Suma de items == Total declarado)
                // Usamos Math.Abs por si hay decimales pequeños, o simple ==
                IsConsistent = dto.invoice_detail != null &&
                            dto.invoice_detail.Sum(i => i.subtotal) == dto.total_amount,

                // Cliente (Flattened / Desnormalizado)
                CustomerRun = dto.customer?.customer_run ?? "Sin RUN",
                CustomerName = dto.customer?.customer_name ?? "Sin Nombre",
                CustomerEmail = dto.customer?.customer_email ?? "",

                // Mapeo de Items (Productos)
                Items = dto.invoice_detail?.Select(item => new InvoiceItem
                {
                    ProductName = item.product_name,
                    UnitPrice = item.unit_price,
                    Quantity = item.quantity,
                    Subtotal = item.subtotal
                }).ToList() ?? new List<InvoiceItem>(),

                // Mapeo de Notas de Crédito
                CreditNotes = dto.invoice_credit_note?.Select(cn => new CreditNote
                {
                    CreditNoteNumber = cn.credit_note_number, // Ya es string gracias al Converter
                    CreditNoteDate = cn.credit_note_date,
                    CreditNoteAmount = cn.credit_note_amount
                }).ToList() ?? new List<CreditNote>(),

                // Mapeo de Pago (si existe info)
                Payment = (dto.invoice_payment != null &&
                       (dto.invoice_payment.payment_date.HasValue || !string.IsNullOrEmpty(dto.invoice_payment.payment_method)))
                 ? new Payment
                 {
                     PaymentMethod = dto.invoice_payment.payment_method,
                     PaymentDate = dto.invoice_payment.payment_date
                 }
                 : null

            }).ToList();
            bool data = await _invoiceRepository.AddRangeAsync(facturasEntidad);
            return data;
        }

    }
}
