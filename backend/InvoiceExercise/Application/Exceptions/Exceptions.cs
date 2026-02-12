using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public abstract class AppException : Exception
    {
        protected AppException(int statusCode, string message, Exception? inner = null)
            : base(message, inner) => StatusCode = statusCode;

        public int StatusCode { get; }
    }

    public sealed class NotFoundAppException(string message)
        : AppException(404, message);

    public sealed class ImportInvoicesException(string message)
        : AppException(422, message);

    public sealed class CreditNoteCreationException(string message)
        : AppException(409, message);
}
