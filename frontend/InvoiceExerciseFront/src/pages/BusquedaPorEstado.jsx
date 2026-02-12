import { useState } from 'react';
import { useForm } from 'react-hook-form';
import InvoiceStatusSearch from '../components/InvoiceStatusSearch';
import InvoiceResultsTable from '../components/InvoiceResultsTable';
import { findInvoicesByStatus } from './api';

export default function BusquedaPorEstado() {
  const { register, handleSubmit, setError, clearErrors, formState: { errors } } = useForm();
  const [results, setResults] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [hasSearched, setHasSearched] = useState(false);

  const handleStatusSearch = async (data) => {
    if (!data.paymentStatus && !data.invoiceStatus) {
      setError('root', { type: 'manual', message: 'Debe seleccionar al menos un criterio de búsqueda.' });
      return;
    }
    clearErrors('root');

    setIsLoading(true);
    setHasSearched(true);
    try {
      const resultData = await findInvoicesByStatus(data);
      setResults(resultData);
    } catch (error) {
      console.error("Error al buscar facturas:", error);
      setResults([]);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="space-y-6">
      <h1 className="text-2xl font-bold text-slate-800">Búsqueda por Estado</h1>
      <InvoiceStatusSearch
        register={register}
        errors={errors}
        onSearch={handleSubmit(handleStatusSearch)}
      />
      <div className="bg-white p-6 rounded-lg shadow-md border border-slate-200">
        <h2 className="text-lg font-semibold text-slate-800">Resultados</h2>
        {hasSearched ? (
          <InvoiceResultsTable invoices={results} isLoading={isLoading} />
        ) : (
          <div className="text-center p-10 text-slate-500">
            Seleccione filtros para buscar.
          </div>
        )}
      </div>
    </div>
  );
}