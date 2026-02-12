import ImportInvoices from '../components/ImportInvoices';

export default function CargarFacturas() {
  return (
    <div className="space-y-6">
      <h1 className="text-2xl font-bold text-slate-800 text-center">Importación de Datos</h1>
      <ImportInvoices />
    </div>
  );
}
