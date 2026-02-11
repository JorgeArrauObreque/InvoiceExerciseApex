import axios from 'axios';

const api = axios.create({
  baseURL: 'http://localhost:5234/api',
  headers: {
    'Content-Type': 'application/json',
  },
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

export const searchInvoices = async (filters) => {
  try {
    let result;
    if (filters.invoiceNumber) {
      result = await getInvoicesByNumber(filters.invoiceNumber);
    } else if (filters.invoiceStatus) {
      result = await getInvoicesByStatus(filters.invoiceStatus);
    } else if (filters.paymentStatus) {
      result = await getInvoicesByStatusPayment(filters.paymentStatus);
    } else {
      return [];
    }

    if (Array.isArray(result)) {
      return result;
    }
    return result ? [result] : [];
  } catch (error) {
    console.error("Error fetching invoices:", error);
    throw error;
  }
};

export const getReports = async () => {
  try {
    const response = await api.get('/reports');
    return response.data;
  } catch (error) {
    console.error("Error fetching reports:", error);
    throw error;
  }
};

export const createCreditNote = async (creditNoteData) => {
    try {
        const payload = {
            CreditNoteNumber: Number(creditNoteData.creditNoteNumber),
            Amount: Number(creditNoteData.amount),
            Invoice: Number(creditNoteData.invoiceNumber)
        };
        const response = await api.post('/CreditNotes/create_credit_note', payload);
        return response.data;
    } catch (error) {
        console.error("Error creating credit note:", error);
        throw error;
    }
}