using Application.Dtos;
using Domain.Enums;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IInvoiceRepository
    {
        Task<bool> AddRangeAsync(IEnumerable<Invoice> invoices);
        Task<Invoice> GetInvoiceByNumber(long invoice_number);
        Task<List<Invoice>> GetInvoiceByStatus(InvoiceStatus status);
        Task<List<Invoice>> GetInvoiceByStatusPayments(PaymentStatus status_payments);
        Task<List<Invoice>> GetOverdue30NoPayNoCn();
        Task<List<PayStatusSummaryItem>> GetPayStatusSummary();
        Task<List<Invoice>> GetInconsistent();
    }
}
