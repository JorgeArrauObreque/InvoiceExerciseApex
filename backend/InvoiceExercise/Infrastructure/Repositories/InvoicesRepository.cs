using Application.Dtos;
using Application.Interfaces;
using Domain.Enums;
using Domain.Models;
using InvoiceExercise.Infrastructure;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories
{
    public class InvoicesRepository: IInvoiceRepository
    {
        private readonly AppDbContext _context;
        public InvoicesRepository(AppDbContext context) 
        {
            _context = context;
        }
        public async Task<bool> AddRangeAsync(IEnumerable<Invoice> invoices)
        {
            var existingNumbersList = await _context.Invoices
                .AsNoTracking()
                .Select(i => i.InvoiceNumber)
                .ToListAsync();

            var existingNumbers = existingNumbersList.ToHashSet();

            var newInvoices = invoices
                .Where(i => !existingNumbers.Contains(i.InvoiceNumber))
                .ToList();

            if (newInvoices.Count > 0)
            {
                await _context.Invoices.AddRangeAsync(newInvoices);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<Invoice> GetInvoiceByNumber(long invoice_number)
        {
            var result = await _context.Invoices.AsNoTracking().Where(r => r.InvoiceNumber == invoice_number && r.IsConsistent== true).Include(r=>r.CreditNotes).Include(r=>r.Payment).FirstOrDefaultAsync();
            return result;
        }
        public async Task<List<Invoice>> GetInvoiceByStatus(InvoiceStatus status)
        {
            var result = await _context.Invoices.AsNoTracking().Where(r => r.Status == status.ToString() && r.IsConsistent == true).ToListAsync();
            return result;
        }
        public async Task<List<Invoice>> GetInvoiceByStatusPayments(PaymentStatus status_payments)
        {
            var result = await _context.Invoices.AsNoTracking().Where(r => r.StatusPayment == status_payments.ToString() && r.IsConsistent == true).ToListAsync();
            return result;
        }


        public async Task<List<Invoice>> GetOverdue30NoPayNoCn()
        {
            var today = DateTime.Now.Date;

            return await _context.Invoices
                .Include(i => i.CreditNotes)
                .Include(i => i.Payment)
                .Where(i => i.IsConsistent)
                .Where(i => i.Payment == null)
                .Where(i => !i.CreditNotes.Any())
                .Where(i =>
                    today >
                    (
                        (i.PaymentDueDate.HasValue && i.PaymentDueDate.Value != DateTime.MinValue)
                            ? i.PaymentDueDate.Value.Date.AddDays(30)
                            : i.InvoiceDate.Date.AddDays(i.DaysToDue + 30)
                    )
                )
                .ToListAsync();
        }
        public async Task<List<PayStatusSummaryItem>> GetPayStatusSummary()
        {
            var today = DateTime.Now.Date;
            var total = await _context.Invoices.CountAsync();

            var rows = await _context.Invoices
                .AsNoTracking()
                .Select(i => new
                {
                    Status =
                        i.Payment != null ? "Paid" :
                        today >
                        ((i.PaymentDueDate.HasValue && i.PaymentDueDate.Value != DateTime.MinValue)
                            ? i.PaymentDueDate.Value.Date
                            : i.InvoiceDate.Date.AddDays(i.DaysToDue))
                        ? "Overdue"
                        : "Pending"
                })
                .GroupBy(x => x.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync();

            var dict = rows.ToDictionary(x => x.Status, x => x.Count);
            string[] all = ["Paid", "Overdue", "Pending"];

            return all.Select(s =>
            {
                dict.TryGetValue(s, out var count);
                var percent = total == 0 ? 0m : (count * 100m) / total;
                return new PayStatusSummaryItem(s, count, percent);
            }).ToList();
        }
        public async Task<List<Invoice>> GetInconsistent()
        {
            return await _context.Invoices.AsNoTracking()
                .Where(i => !i.IsConsistent)
                .ToListAsync();
        }
    }
}
