using Application.Interfaces;
using Domain.Models;
using Domain.Enums;
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
            // filtrados duplicados
            //var existing_numbers = await _context.Invoices
            //   .Select(i => i.InvoiceNumber)
            //   .ToListAsync();
            //var newInvoices = invoices.Where(i => !existing_numbers.Contains(i.InvoiceNumber));

            await _context.AddRangeAsync(invoices);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<Invoice> GetInvoiceByNumber(long invoice_number)
        {
            var result = await _context.Invoices.AsNoTracking().Where(r => r.InvoiceNumber == invoice_number).Include(r=>r.CreditNotes).Include(r=>r.Payment).FirstOrDefaultAsync();
            return result;
        }
        public async Task<List<Invoice>> GetInvoiceByStatus(InvoiceStatus status)
        {
            var result = await _context.Invoices.AsNoTracking().Where(r => r.Status == status.ToString()).ToListAsync();
            return result;
        }
        public async Task<List<Invoice>> GetInvoiceByStatusPayments(PaymentStatus status_payments)
        {
            var result = await _context.Invoices.AsNoTracking().Where(r => r.StatusPayment == status_payments.ToString()).ToListAsync();
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
        public async Task<List<(string Status, int Count, decimal Percent)>> GetPayStatusSummary()
        {
            var today = DateTime.Now.Date;

            var total = await _context.Invoices.CountAsync();

            var rows = await _context.Invoices
                .Select(i => new
                {
                    Status =
                        i.Payment != null ? "Paid" :
                        today >
                        (
                            (i.PaymentDueDate.HasValue && i.PaymentDueDate.Value != DateTime.MinValue)
                                ? i.PaymentDueDate.Value.Date
                                : i.InvoiceDate.Date.AddDays(i.DaysToDue)
                        )
                        ? "Overdue"
                        : "Pending"
                })
                .GroupBy(x => x.Status).AsNoTracking()
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync();

            return rows
                .Select(r => (r.Status, r.Count, total == 0 ? 0m : (r.Count * 100m) / total))
                .ToList();
        }
        public async Task<List<Invoice>> GetInconsistent()
        {
            return await _context.Invoices.AsNoTracking()
                .Where(i => !i.IsConsistent)
                .ToListAsync();
        }
    }
}
