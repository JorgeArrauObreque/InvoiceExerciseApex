using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class InvoicePaymentJsonDto
    {
        public string? payment_method { get; set; }
        public DateTime? payment_date { get; set; }
    }
}
