import { useState, useEffect } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import CreateCreditNoteComponent from '../components/CreateCreditNote';
import { createCreditNote } from './api';

function useQuery() {
  return new URLSearchParams(useLocation().search);
}

export default function CrearNotaCredito() {
  const query = useQuery();
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    invoiceNumber: '',
    amount: ''
  });
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [submitMessage, setSubmitMessage] = useState('');

  const invoiceNumberFromQuery = query.get('invoiceNumber');

  useEffect(() => {
    if (invoiceNumberFromQuery) {
      setFormData(prev => ({ ...prev, invoiceNumber: invoiceNumberFromQuery }));
    }
  }, [invoiceNumberFromQuery]);

  const handleFormChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setIsSubmitting(true);
    setSubmitMessage('');
    try {
      const response = await createCreditNote(formData);
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
      formData={formData}
      onFormChange={handleFormChange}
      onSubmit={handleSubmit}
      isSubmitting={isSubmitting}
      message={submitMessage}
    />
  );
}