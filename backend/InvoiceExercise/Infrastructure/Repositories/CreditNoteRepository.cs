using Application.Interfaces;
using Domain.Models;
using InvoiceExercise.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CreditNoteRepository: ICreditNoteRepository
    {
        private readonly AppDbContext _context;
        public CreditNoteRepository(AppDbContext context) 
        {
            _context = context;
        }
        public async Task<CreditNote> AddCreditNoteInvoice(CreditNote note)
        {
            note.CreditNoteDate = DateTime.Now;
            _context.Add(note);
            await _context.SaveChangesAsync();
            return note;
        }
    }
}
