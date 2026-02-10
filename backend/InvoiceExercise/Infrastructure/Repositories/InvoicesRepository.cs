using Application.Interfaces;
using Domain.Models;
using InvoiceExercise.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
