import axios from 'axios';

const api = axios.create({
  baseURL: 'http://localhost:5234/api',
  headers: {
    'Content-Type': 'application/json',
  },
});

// Interceptor para agregar el Token JWT a cada petición
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export const getInvoicesByNumber = async (invoiceNumber) => {
  const response = await api.get('/Invoices/get_invoices_by_number', { params: { invoice_number: invoiceNumber } });
  return response.data;
};

export const getInvoicesByStatus = async (status) => {
  const response = await api.get('/Invoices/get_invoices_by_status', { params: { status } });
  return response.data;
};

export const getInvoicesByStatusPayment = async (statusPayment) => {
  const response = await api.get('/Invoices/get_invoices_by_status_payment', { params: { status: statusPayment } });
  return response.data;
};

export const findInvoiceByNumber = async (invoiceNumber) => {
  if (!invoiceNumber) return [];
  try {
    const result = await getInvoicesByNumber(invoiceNumber);
    if (Array.isArray(result)) {
      return result;
    }
    return result ? [result] : [];
  } catch (error) {
    if (error.response && error.response.status === 404) {
        return [];
    }
    console.error("Error al obtener la factura por número:", error);
    throw error;
  }
};

export const findInvoicesByStatus = async ({ paymentStatus, invoiceStatus }) => {
  try {
    if (invoiceStatus) {
        return await getInvoicesByStatus(invoiceStatus);
    }
    if (paymentStatus) {
        return await getInvoicesByStatusPayment(paymentStatus);
    }
    return [];
  } catch (error) {
    console.error("Error al obtener facturas por estado:", error);
    throw error;
  }
};

export const getOverdueReport = async () => {
  const response = await api.get('/reports/overdue-30');
  return response.data;
};

export const getPaymentStatusSummary = async () => {
  const response = await api.get('/reports/pay-status-summary');
  return response.data;
};

export const getInconsistentReport = async () => {
  const response = await api.get('/reports/inconsistent');
  return response.data;
};

export const createCreditNote = async (creditNoteData) => {
    try {
        const payload = {
            Amount: Number(creditNoteData.amount),
            Invoice: Number(creditNoteData.invoiceNumber)
        };
        const response = await api.post('/CreditNotes/create_credit_note', payload);
        return response.data;
    } catch (error) {
        console.error("Error al crear la nota de crédito:", error);
        throw error;
    }
};

export const importInvoices = async (file) => {
    const formData = new FormData();
    formData.append('file', file);
    const response = await api.post('/Invoices/Import', formData, {
        headers: {
            'Content-Type': 'multipart/form-data'
        }
    });
    return response.data;
};

export const login = async (credentials) => {
  const response = await api.post('/Auth/login', credentials);
  return response.data;
};