using Application.Dtos;
using Application.Dtos.Json;
using Application.Exceptions;
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
            if (invoice == null) throw new NotFoundAppException("La factura no existe");
            return new InvoiceDto() { InvoiceNumber = invoice.InvoiceNumber, TotalAmount = invoice.TotalAmount, Status = invoice.Status, StatusPayment = invoice.StatusPayment, CustomerName = invoice.CustomerName, CustomerRun = invoice.CustomerRun, InvoiceDate = invoice.InvoiceDate, PaymentDueDate = invoice.PaymentDueDate };
        }

        public async Task<List<InvoiceDto>> InvoiceByStatus(InvoiceStatus invoice_status)
        {
            var invoice = await _invoiceRepository.GetInvoiceByStatus(invoice_status);
            if (invoice.Count == 0) throw new NotFoundAppException($"No se han encontrado facturas con el estado {invoice_status.ToString()} ");

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
            if (invoice.Count == 0) throw new NotFoundAppException($"No se han encontrado facturas con el estado de pago {invoice_status.ToString()}");
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
            if (invoices.Count == 0) throw new NotFoundAppException($"no se encontraron documentos vencidos 30 días, sin pago registrado y sin nota de crédito");

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
        public async Task<List<PayStatusSummaryItem>> ObtainsPayStatusSummary()
        {
            return await _invoiceRepository.GetPayStatusSummary();
        }
        public async Task<List<InvoiceDto>> ObtainsInconsistents()
        {
            var invoice = await _invoiceRepository.GetInconsistent();
            if (invoice.Count == 0) throw new NotFoundAppException($"no se encontro documentos inconsistentes");
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
        public async Task ImportFromJson(Stream file)
        {
            var invoices = await _jsonReader.ReadInvoicesJsonFile(file);
            if (invoices.Count == 0) throw new ImportInvoicesException("el json no se encontraron registros o el formato no es el correcto");
            var import = await _invoiceRepository.AddRangeAsync(MapInvoices(invoices));
            if (!import) throw new ImportInvoicesException("ha ocurrido un error, los registros no fueron insertados");
        }
        public static List<Invoice> MapInvoices(IEnumerable<InvoiceJsonDto> json_invoices)
        {
            var invoices = json_invoices.Select(dto => new Invoice
            {
                InvoiceNumber = dto.invoice_number,
                InvoiceDate = dto.invoice_date,
                TotalAmount = dto.total_amount,
                DaysToDue = dto.days_to_due,
                PaymentDueDate = dto.payment_due_date,

                IsConsistent = dto.invoice_detail != null &&
                               Math.Abs(dto.invoice_detail.Sum(i => i.subtotal) - dto.total_amount) < 0.01m,

                CustomerRun = dto.customer?.customer_run ?? "Sin RUN",
                CustomerName = dto.customer?.customer_name ?? "Sin Nombre",
                CustomerEmail = dto.customer?.customer_email ?? "",

                Items = dto.invoice_detail?.Select(item => new InvoiceItem
                {
                    ProductName = item.product_name,
                    UnitPrice = item.unit_price,
                    Quantity = item.quantity,
                    Subtotal = item.subtotal
                }).ToList() ?? new List<InvoiceItem>(),

                CreditNotes = dto.invoice_credit_note?.Select(cn => new CreditNote
                {
                    CreditNoteNumber = long.Parse(cn.credit_note_number),
                    CreditNoteDate = cn.credit_note_date,
                    CreditNoteAmount = cn.credit_note_amount
                }).ToList() ?? new List<CreditNote>(),

                Payment = (dto.invoice_payment != null &&
                           (dto.invoice_payment.payment_date.HasValue ||
                            !string.IsNullOrEmpty(dto.invoice_payment.payment_method)))
                    ? new Payment
                    {
                        PaymentMethod = dto.invoice_payment.payment_method,
                        PaymentDate = dto.invoice_payment.payment_date
                    }
                    : null

            }).ToList();

            foreach (var item in invoices)
            {
                item.StatusPayment = item.CalculatePaymentStatus();
                item.Status = item.CalculateStatus();
            }

            return invoices;
        }

    }
}
