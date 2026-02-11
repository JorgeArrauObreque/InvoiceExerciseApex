import { useState } from 'react';

export default function CreateCreditNote() {
  const [formData, setFormData] = useState({
    invoiceNumber: '',
    amount: ''
  });

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    console.log('Creando Nota de Crédito:', formData);
    // Aquí iría la validación de monto <= saldo pendiente
  };

  return (
    <div className="bg-white p-6 rounded-lg shadow-md border border-slate-200 h-fit">
      <h2 className="text-lg font-semibold text-slate-800 mb-4 flex items-center gap-2">
        📄 Nueva Nota de Crédito
      </h2>
      <form onSubmit={handleSubmit} className="space-y-4">
        <div>
          <label htmlFor="ncInvoiceNumber" className="block text-sm font-medium text-slate-700 mb-1">
            Factura Asociada
          </label>
          <input
            type="text"
            name="invoiceNumber"
            id="ncInvoiceNumber"
            value={formData.invoiceNumber}
            onChange={handleChange}
            className="w-full rounded-md border border-gray-300 bg-white px-3 py-2 text-sm shadow-sm focus:border-blue-500 focus:outline-none focus:ring-1 focus:ring-blue-500"
            placeholder="Buscar número de factura..."
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
              onChange={handleChange}
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
            className="w-full bg-green-600 text-white px-4 py-2 rounded-md hover:bg-green-700 focus:outline-none focus:ring-2 focus:ring-green-500 focus:ring-offset-2 transition-colors font-medium cursor-pointer"
          >
            Generar Nota de Crédito
          </button>
        </div>
      </form>
    </div>
  );
}