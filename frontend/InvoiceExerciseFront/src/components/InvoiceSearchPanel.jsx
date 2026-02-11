export default function InvoiceSearchPanel({ filters, onFilterChange, onSearch }) {
  const handleSearch = (e) => {
    e.preventDefault();
    if (onSearch) {
      onSearch();
    }
  };

  return (
    <div className="bg-white p-6 rounded-lg shadow-md border border-slate-200">
      <h2 className="text-lg font-semibold text-slate-800 mb-4 flex items-center gap-2">
        🔍 Búsqueda de Facturas
      </h2>
      <form onSubmit={handleSearch} className="grid grid-cols-1 md:grid-cols-4 gap-4 items-end">
        <div>
          <label htmlFor="invoiceNumber" className="block text-sm font-medium text-slate-700 mb-1">
            Número de Factura
          </label>
          <input
            type="text"
            name="invoiceNumber"
            id="invoiceNumber"
            value={filters.invoiceNumber}
            onChange={onFilterChange}
            className="w-full rounded-md border border-gray-300 bg-white px-3 py-2 text-sm shadow-sm focus:border-blue-500 focus:outline-none focus:ring-1 focus:ring-blue-500"
            placeholder="Ej: INV-001"
          />
        </div>

        <div>
          <label htmlFor="paymentStatus" className="block text-sm font-medium text-slate-700 mb-1">
            Estado de Pago
          </label>
          <select
            name="paymentStatus"
            id="paymentStatus"
            value={filters.paymentStatus}
            onChange={onFilterChange}
            className="w-full rounded-md border border-gray-300 bg-white px-3 py-2 text-sm shadow-sm focus:border-blue-500 focus:outline-none focus:ring-1 focus:ring-blue-500"
          >
            <option value="">Todos</option>
            <option value="Paid">Pagada (Paid)</option>
            <option value="Pending">Pendiente (Pending)</option>
            <option value="Overdue">Vencida (Overdue)</option>
          </select>
        </div>

        <div>
          <label htmlFor="invoiceStatus" className="block text-sm font-medium text-slate-700 mb-1">
            Estado Comercial
          </label>
          <select
            name="invoiceStatus"
            id="invoiceStatus"
            value={filters.invoiceStatus}
            onChange={onFilterChange}
            className="w-full rounded-md border border-gray-300 bg-white px-3 py-2 text-sm shadow-sm focus:border-blue-500 focus:outline-none focus:ring-1 focus:ring-blue-500"
          >
            <option value="">Todos</option>
            <option value="Issued">Emitida (Issued)</option>
            <option value="Partial">Parcial (Partial)</option>
            <option value="Cancelled">Cancelada (Cancelled)</option>
          </select>
        </div>

        <div>
          <button
            type="submit"
            className="w-full bg-blue-600 text-white px-4 py-2 rounded-md hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2 transition-colors font-medium cursor-pointer"
          >
            Buscar
          </button>
        </div>
      </form>
    </div>
  );
}