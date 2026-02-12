using Application.Dtos.Json;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IInvoicesJson
    {
        List<InvoiceJsonDto> ReadInvoices(string filePath);
        Task<List<InvoiceJsonDto>> ReadInvoicesJsonFile(Stream file);

    }
}
