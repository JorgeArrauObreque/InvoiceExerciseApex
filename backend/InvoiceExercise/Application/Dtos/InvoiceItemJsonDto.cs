using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class InvoiceItemJsonDto
    {
        public string product_name { get; set; }
        public decimal unit_price { get; set; }
        public decimal quantity { get; set; }
        public decimal subtotal { get; set; }
    }
}
