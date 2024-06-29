import axios from 'axios';

const BASE_URL = 'https://localhost:7159/api/quotes';

const axiosInstance = axios.create({
  baseURL: BASE_URL,
});

export const fetchQuotes = async () => {
  try {
    const response = await axiosInstance.get('/');
    return response.data;
  } catch (error) {
    console.error('Error fetching quotes:', error);
    throw error;
  }
};

export const createQuote = async (quoteDto) => {
  try {
    const response = await axiosInstance.post('/', quoteDto);
    return response.data;
  } catch (error) {
    console.error('Error creating quote:', error);
    throw error;
  }
};

export const updateQuote = async (id, quoteDto) => {
  try {
    const response = await axiosInstance.put(`/${id}`, quoteDto);
    return response.data;
  } catch (error) {
    console.error('Error updating quote:', error);
    throw error;
  }
};

export const deleteQuote = async (id) => {
  try {
    await axiosInstance.delete(`/${id}`);
  } catch (error) {
    console.error('Error deleting quote:', error);
    throw error;
  }
};

export const searchQuote = async (searchParams) => {
  try {
    console.log(searchParams);
    const response = await axiosInstance.post('/search', searchParams);
    return response.data;
  } catch (error) {
    console.error('Error searching quotes:', error);
    throw error;
  }
};


