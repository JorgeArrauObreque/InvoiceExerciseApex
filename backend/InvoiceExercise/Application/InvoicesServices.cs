using Application.Dtos;
using Application.Interfaces;
using Domain.Enums;
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
        public async Task<InvoiceDto> InvoicesByNumber(long invoice_number)
        {
            var invoice = await _invoiceRepository.GetInvoiceByNumber(invoice_number);
            return new InvoiceDto() {InvoiceNumber = invoice.InvoiceNumber,TotalAmount = invoice.TotalAmount, Status = invoice.Status, StatusPayment = invoice.StatusPayment, CustomerName = invoice.CustomerName, CustomerRun = invoice.CustomerRun, InvoiceDate = invoice.InvoiceDate , PaymentDueDate = invoice.PaymentDueDate  };
        }

        public async Task<List<InvoiceDto>> InvoiceByStatus(InvoiceStatus invoice_status)
        {
            var invoice = await _invoiceRepository.GetInvoiceByStatus(invoice_status);
            return invoice.Select(x => new InvoiceDto()
            {
                InvoiceNumber = x.InvoiceNumber,
                TotalAmount = x.TotalAmount,
                Status = x.Status,
                StatusPayment = x.StatusPayment,
                CustomerName = x.CustomerName,
                CustomerRun = x.CustomerRun,
                InvoiceDate = x.InvoiceDate,
                PaymentDueDate = x.PaymentDueDate
            } ).ToList();
            
        }
        public async Task<List<InvoiceDto>> InvoiceByStatusPayment(PaymentStatus invoice_status)
        {
            var invoice = await _invoiceRepository.GetInvoiceByStatusPayments(invoice_status);
            return invoice.Select(x => new InvoiceDto()
            {
                InvoiceNumber = x.InvoiceNumber,
                TotalAmount = x.TotalAmount,
                Status = x.Status,
                StatusPayment = x.StatusPayment,
                CustomerName = x.CustomerName,
                CustomerRun = x.CustomerRun,
                InvoiceDate = x.InvoiceDate,
                PaymentDueDate = x.PaymentDueDate
            }).ToList();

        }
        public async Task<List<InvoiceDto>> GetOverdue()
        {
            var invoices = await _invoiceRepository.GetOverdue30NoPayNoCn();
            return invoices.Select(x => new InvoiceDto()
            {
                InvoiceNumber = x.InvoiceNumber,
                TotalAmount = x.TotalAmount,
                Status = x.Status,
                StatusPayment = x.StatusPayment,
                CustomerName = x.CustomerName,
                CustomerRun = x.CustomerRun,
                InvoiceDate = x.InvoiceDate,
                PaymentDueDate = x.PaymentDueDate
            }).ToList();
        }
        public async Task<dynamic> ObtainsPayStatusSummary()
        {
            var invoices = await _invoiceRepository.GetPayStatusSummary();
            return invoices;
        }
        public async Task<List<InvoiceDto>> ObtainsInconsistents()
        {
            var invoice = await _invoiceRepository.GetInconsistent();
            return invoice.Select(x => new InvoiceDto()
            {
                InvoiceNumber = x.InvoiceNumber,
                TotalAmount = x.TotalAmount,
                Status = x.Status,
                StatusPayment = x.StatusPayment,
                CustomerName = x.CustomerName,
                CustomerRun = x.CustomerRun,
                InvoiceDate = x.InvoiceDate,
                PaymentDueDate = x.PaymentDueDate
            }).ToList();
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

                // Calcular consistencia
         
                IsConsistent = dto.invoice_detail != null &&
                            dto.invoice_detail.Sum(i => i.subtotal) == dto.total_amount,

                // Cliente 
                CustomerRun = dto.customer?.customer_run ?? "Sin RUN",
                CustomerName = dto.customer?.customer_name ?? "Sin Nombre",
                CustomerEmail = dto.customer?.customer_email ?? "",

                // Mapeo de Items 
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
                    CreditNoteNumber = long.Parse(cn.credit_note_number), 
                    CreditNoteDate = cn.credit_note_date,
                    CreditNoteAmount = cn.credit_note_amount
                }).ToList() ?? new List<CreditNote>(),

                // Mapeo de Pago, si existe 
                Payment = (dto.invoice_payment != null &&
                       (dto.invoice_payment.payment_date.HasValue || !string.IsNullOrEmpty(dto.invoice_payment.payment_method)))
                 ? new Payment
                 {
                     PaymentMethod = dto.invoice_payment.payment_method,
                     PaymentDate = dto.invoice_payment.payment_date
                 }
                 : null              
                
            }).ToList();
            foreach (var item in facturasEntidad)
            {
                item.StatusPayment = item.CalculatePaymentStatus();
                item.Status = item.CalculateStatus();
            }
            bool data = await _invoiceRepository.AddRangeAsync(facturasEntidad);
            return data;
        }

    }
}
