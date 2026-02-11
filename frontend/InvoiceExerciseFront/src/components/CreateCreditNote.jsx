export default function CreateCreditNote({ formData, onFormChange, onSubmit, isSubmitting, message }) {
  return (
    <div className="bg-white p-6 rounded-lg shadow-md border border-slate-200 h-fit">
      <h2 className="text-lg font-semibold text-slate-800 mb-4 flex items-center gap-2">
          Nueva Nota de Crédito
      </h2>
      <form onSubmit={onSubmit} className="space-y-4">
        <div>
          <label htmlFor="ncInvoiceNumber" className="block text-sm font-medium text-slate-700 mb-1">
            Factura Asociada
          </label>
          <input
            type="text"
            name="invoiceNumber"
            id="ncInvoiceNumber"
            value={formData.invoiceNumber}
            onChange={onFormChange}
            className="w-full rounded-md border border-gray-300 bg-white px-3 py-2 text-sm shadow-sm focus:border-blue-500 focus:outline-none focus:ring-1 focus:ring-blue-500"
            placeholder="Buscar número de factura..."
            required
          />
        </div>

        <div>
          <label htmlFor="creditNoteNumber" className="block text-sm font-medium text-slate-700 mb-1">
            Número de Nota de Crédito
          </label>
          <input
            type="number"
            name="creditNoteNumber"
            id="creditNoteNumber"
            value={formData.creditNoteNumber}
            onChange={onFormChange}
            className="w-full rounded-md border border-gray-300 bg-white px-3 py-2 text-sm shadow-sm focus:border-blue-500 focus:outline-none focus:ring-1 focus:ring-blue-500"
            placeholder="Ej: 12345"
            required
          />
        </div>

        <div>
          <label htmlFor="amount" className="block text-sm font-medium text-slate-700 mb-1">
            Monto
          </label>
          <div className="relative">
            <div className="pointer-events-none absolute inset-y-0 left-0 flex items-center pl-3">
              <span className="text-gray-500 sm:text-sm">$</span>
            </div>
            <input
              type="number"
              name="amount"
              id="amount"
              value={formData.amount}
              onChange={onFormChange}
              className="w-full rounded-md border border-gray-300 bg-white pl-7 pr-3 py-2 text-sm shadow-sm focus:border-blue-500 focus:outline-none focus:ring-1 focus:ring-blue-500"
              placeholder="0.00"
              step="0.01"
              required
            />
          </div>
          <p className="mt-1 text-xs text-gray-500">
            * El monto no puede superar el saldo pendiente.
          </p>
        </div>

        <div className="pt-2">
          <button
            type="submit"
            disabled={isSubmitting}
            className={`w-full px-4 py-2 rounded-md text-white font-medium transition-colors focus:outline-none focus:ring-2 focus:ring-offset-2 ${
              isSubmitting 
                ? 'bg-gray-400 cursor-not-allowed' 
                : 'bg-green-600 hover:bg-green-700 focus:ring-green-500 cursor-pointer'
            }`}
          >
            {isSubmitting ? 'Procesando...' : 'Generar Nota de Crédito'}
          </button>
        </div>

        {message && (
          <div className={`mt-4 p-3 rounded-md text-sm text-center ${message.includes('Error') ? 'bg-red-100 text-red-700' : 'bg-green-100 text-green-700'}`}>
            {message}
          </div>
        )}
      </form>
    </div>
  );
}