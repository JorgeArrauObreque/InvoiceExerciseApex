import { useState } from 'react';
import { useForm } from 'react-hook-form';
import InvoiceNumberSearch from '../components/InvoiceNumberSearch';
import InvoiceResultsTable from '../components/InvoiceResultsTable';
import { findInvoiceByNumber } from './api';

export default function BusquedaPorNumero() {
  const { register, handleSubmit, formState: { errors } } = useForm();
  const [results, setResults] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [hasSearched, setHasSearched] = useState(false);

  const handleNumberSearch = async (data) => {
    setIsLoading(true);
    setHasSearched(true);
    try {
      const resultData = await findInvoiceByNumber(data.invoiceNumber);
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
      <h1 className="text-2xl font-bold text-slate-800">Búsqueda por Número</h1>
      <InvoiceNumberSearch
        register={register}
        errors={errors}
        onSearch={handleSubmit(handleNumberSearch)}
      />
      <div className="bg-white p-6 rounded-lg shadow-md border border-slate-200">
        <h2 className="text-lg font-semibold text-slate-800">Resultados</h2>
        {hasSearched ? (
          <InvoiceResultsTable invoices={results} isLoading={isLoading} />
        ) : (
          <div className="text-center p-10 text-slate-500">
            Ingrese un número para buscar.
          </div>
        )}
      </div>
    </div>
  );
}