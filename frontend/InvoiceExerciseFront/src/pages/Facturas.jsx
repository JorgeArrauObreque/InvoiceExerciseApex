import { useState } from 'react';
import InvoiceSearchPanel from '../components/InvoiceSearchPanel';
import InvoiceResultsTable from '../components/InvoiceResultsTable';
import { searchInvoices } from './api';

export default function Facturas() {
  const [filters, setFilters] = useState({
    invoiceNumber: '',
    paymentStatus: '',
    invoiceStatus: ''
  });
  const [results, setResults] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [hasSearched, setHasSearched] = useState(false);

  const handleFilterChange = (e) => {
    const { name, value } = e.target;
    setFilters(prev => ({ ...prev, [name]: value }));
  };

  const handleSearch = async () => {
    setIsLoading(true);
    setHasSearched(true);
    try {
      const data = await searchInvoices(filters);
      setResults(data);
    } catch (error) {
      console.error("Error al buscar facturas:", error);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="space-y-6">
      <InvoiceSearchPanel 
        filters={filters}
        onFilterChange={handleFilterChange}
        onSearch={handleSearch}
      />
      <div className="bg-white p-6 rounded-lg shadow-md border border-slate-200">
        <h2 className="text-lg font-semibold text-slate-800">Resultados</h2>
        {hasSearched ? (
          <InvoiceResultsTable invoices={results} isLoading={isLoading} />
        ) : (
          <div className="text-center p-10 text-slate-500">
            Utilice los filtros para iniciar una búsqueda.
          </div>
        )}
      </div>
    </div>
  );
}