export default function InvoiceNumberSearch({ onSearch, filter, onFilterChange }) {
  const handleSearch = (e) => {
    e.preventDefault();
    onSearch();
  };

  return (
    <div className="bg-white p-6 rounded-lg shadow-md border border-slate-200">
      <h2 className="text-lg font-semibold text-slate-800 mb-4">
        Búsqueda por Número de Factura
      </h2>
      <form onSubmit={handleSearch} className="grid grid-cols-1 sm:grid-cols-4 gap-4 items-end">
        <div className="sm:col-span-3">
          <label htmlFor="invoiceNumber" className="block text-sm font-medium text-slate-700 mb-1">
            Número de Factura
          </label>
          <input
            type="text"
            name="invoiceNumber"
            id="invoiceNumber"
            value={filter}
            onChange={(e) => onFilterChange(e.target.value)}
            className="w-full rounded-md border border-gray-300 bg-white px-3 py-2 text-sm shadow-sm focus:border-blue-500 focus:outline-none focus:ring-1 focus:ring-blue-500"
            placeholder="Ej: 123"
          />
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