import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { importInvoices } from '../pages/api';

export default function ImportInvoices() {
  const { register, handleSubmit, formState: { errors }, reset } = useForm();
  const [message, setMessage] = useState('');
  const [isSubmitting, setIsSubmitting] = useState(false);

  const onSubmit = async (data) => {
    if (!data.file[0]) return;

    setIsSubmitting(true);
    setMessage('');
    try {
      await importInvoices(data.file[0]);
      setMessage('Archivo importado exitosamente.');
      reset();
    } catch (error) {
      console.error("Error importing invoices:", error);
      setMessage('Error al importar el archivo.');
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="bg-white p-6 rounded-lg shadow-md border border-slate-200 max-w-md mx-auto">
      <h2 className="text-lg font-semibold text-slate-800 mb-4">
        Cargar Facturas (JSON)
      </h2>
      <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
        <div>
          <label htmlFor="file" className="block text-sm font-medium text-slate-700 mb-1">
            Seleccionar Archivo
          </label>
          <input
            type="file"
            id="file"
            accept=".json"
            {...register("file", { required: "El archivo es obligatorio" })}
            className="block w-full text-sm text-slate-500
              file:mr-4 file:py-2 file:px-4
              file:rounded-md file:border-0
              file:text-sm file:font-semibold
              file:bg-blue-50 file:text-blue-700
              hover:file:bg-blue-100"
          />
          {errors.file && <p className="text-red-500 text-xs mt-1">{errors.file.message}</p>}
        </div>

        <button
          type="submit"
          disabled={isSubmitting}
          className={`w-full px-4 py-2 rounded-md text-white font-medium transition-colors focus:outline-none focus:ring-2 focus:ring-offset-2 ${
            isSubmitting 
              ? 'bg-gray-400 cursor-not-allowed' 
              : 'bg-blue-600 hover:bg-blue-700 focus:ring-blue-500 cursor-pointer'
          }`}
        >
          {isSubmitting ? 'Cargando...' : 'Subir JSON'}
        </button>

        {message && (
          <div className={`mt-4 p-3 rounded-md text-sm text-center ${message.includes('Error') ? 'bg-red-100 text-red-700' : 'bg-green-100 text-green-700'}`}>
            {message}
          </div>
        )}
      </form>
    </div>
  );
}
