import { useState, useEffect } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import { useForm } from 'react-hook-form';
import CreateCreditNoteComponent from '../components/CreateCreditNote';
import { createCreditNote, findInvoiceByNumber } from './api';

function useQuery() {
  return new URLSearchParams(useLocation().search);
}

export default function CrearNotaCredito() {
  const query = useQuery();
  const navigate = useNavigate();
  const { register, handleSubmit, setValue, setError, formState: { errors } } = useForm();
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [submitMessage, setSubmitMessage] = useState('');

  const invoiceNumberFromQuery = query.get('invoiceNumber');

  useEffect(() => {
    if (invoiceNumberFromQuery) {
      setValue('invoiceNumber', invoiceNumberFromQuery);
    }
  }, [invoiceNumberFromQuery, setValue]);

  const onSubmit = async (data) => {
    setIsSubmitting(true);
    setSubmitMessage('');

    const amountVal = Number(data.amount);
    if (amountVal <= 0) {
      setError('amount', { type: 'manual', message: 'El monto debe ser mayor a 0.' });
      setIsSubmitting(false);
      return;
    }

    try {
      // 1. Validar que la factura existe
      const invoices = await findInvoiceByNumber(data.invoiceNumber);
      if (!invoices || invoices.length === 0) {
        setError('invoiceNumber', { type: 'manual', message: 'El número de factura no existe.' });
        setIsSubmitting(false);
        return;
      }

      // 2. Validar que el monto no exceda el total de la factura
      const invoice = invoices[0];
      if (amountVal > invoice.totalAmount) {
        setError('amount', { type: 'manual', message: `El monto excede el total de la factura ($${invoice.totalAmount}).` });
        setIsSubmitting(false);
        return;
      }

      const response = await createCreditNote(data);
      setSubmitMessage(response.message || 'Operación exitosa.');
      setTimeout(() => navigate('/facturas'), 2000);
    } catch (error) {
      setSubmitMessage('Error al crear la nota de crédito.');
      console.error(error);
      setIsSubmitting(false);
    }
  };

  return (
    <CreateCreditNoteComponent
      register={register}
      errors={errors}
      onSubmit={handleSubmit(onSubmit)}
      isSubmitting={isSubmitting}
      message={submitMessage}
    />
  );
}