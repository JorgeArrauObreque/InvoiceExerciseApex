import { useState, useEffect } from 'react';
import { getReports } from './api';

export default function Reportes() {
  const [reports, setReports] = useState(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const fetchReports = async () => {
      setIsLoading(true);
      try {
        const data = await getReports();
        setReports(data);
      } catch (error) {
        console.error("Error al obtener reportes:", error);
      } finally {
        setIsLoading(false);
      }
    };
    fetchReports();
  }, []);

  if (isLoading) {
    return <div className="text-center p-10">Cargando reportes...</div>;
  }

  if (!reports) {
    return <div className="text-center p-10 text-red-500">No se pudieron cargar los reportes.</div>;
  }

  return (
    <div className="space-y-8">
      <h1 className="text-3xl font-bold text-slate-800">Reportes</h1>

      <div className="bg-white p-6 rounded-lg shadow-md border border-slate-200">
        <h2 className="text-xl font-semibold text-slate-700 mb-4">Facturas con más de 30 días de vencimiento</h2>
        <ul className="divide-y divide-gray-200">
          {reports.overdueInvoices.map(inv => (
            <li key={inv.invoice_number} className="py-3 flex justify-between items-center">
              <div>
                <p className="font-medium text-slate-800">{inv.invoice_number} ({inv.client_name})</p>
                <p className="text-sm text-slate-500">Monto: ${inv.total_amount.toLocaleString('es-CL')}</p>
              </div>
              <span className="text-sm font-bold text-red-600">{inv.days_overdue} días vencida</span>
            </li>
          ))}
        </ul>
      </div>

      <div className="bg-white p-6 rounded-lg shadow-md border border-slate-200">
        <h2 className="text-xl font-semibold text-slate-700 mb-4">Resumen por Estado de Pago</h2>
        <div className="grid grid-cols-1 sm:grid-cols-3 gap-4 text-center">
          {Object.entries(reports.statusSummary).map(([status, data]) => (
            <div key={status} className="bg-slate-50 p-4 rounded-lg">
              <p className="text-sm font-medium text-slate-500">{status}</p>
              <p className="text-2xl font-bold text-slate-800">{data.total}</p>
              <p className="text-sm text-slate-600">{data.percentage.toFixed(2)}%</p>
            </div>
          ))}
        </div>
      </div>

      <div className="bg-white p-6 rounded-lg shadow-md border border-slate-200">
        <h2 className="text-xl font-semibold text-slate-700 mb-4">Facturas Inconsistentes</h2>
        <p className="text-sm text-slate-500 mb-3">(Total declarado no coincide con la suma de productos)</p>
        <ul className="divide-y divide-gray-200">
          {reports.inconsistentInvoices.map(inv => (
            <li key={inv.invoice_number} className="py-3">
              <p className="font-medium text-slate-800">{inv.invoice_number}</p>
              <p className="text-sm text-slate-500">Declarado: ${inv.declared_total.toLocaleString('es-CL')} | Calculado: ${inv.calculated_total.toLocaleString('es-CL')}</p>
            </li>
          ))}
        </ul>
      </div>
    </div>
  );
}