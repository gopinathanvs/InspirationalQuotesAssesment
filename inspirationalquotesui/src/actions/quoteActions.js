import {
    fetchQuotes as fetchQuotesApi,
    createQuote as createQuoteApi,
    updateQuote as updateQuoteApi,
    deleteQuote as deleteQuoteApi,
    searchQuote as searchQuoteApi
  } from '../services/quotesApi';
  
  export const FETCH_QUOTES_REQUEST = 'FETCH_QUOTES_REQUEST';
  export const FETCH_QUOTES_SUCCESS = 'FETCH_QUOTES_SUCCESS';
  export const FETCH_QUOTES_FAILURE = 'FETCH_QUOTES_FAILURE';
  export const DELETE_QUOTE_SUCCESS = 'DELETE_QUOTE_SUCCESS';
  export const SEARCH_QUOTES_REQUEST = 'SEARCH_QUOTES_REQUEST';
  export const SEARCH_QUOTES_SUCCESS = 'SEARCH_QUOTES_SUCCESS';
  export const SEARCH_QUOTES_FAILURE = 'SEARCH_QUOTES_FAILURE';
  
  export const fetchQuotesRequest = () => ({
    type: FETCH_QUOTES_REQUEST,
  });
  
  export const fetchQuotesSuccess = (quotes) => ({
    type: FETCH_QUOTES_SUCCESS,
    payload: quotes,
  });
  
  export const fetchQuotesFailure = (error) => ({
    type: FETCH_QUOTES_FAILURE,
    payload: error,
  });
  
  
  export const deleteQuoteSuccess = (id) => ({
    type: DELETE_QUOTE_SUCCESS,
    payload: id,
  });
  
  export const searchQuotesRequest = () => ({
    type: SEARCH_QUOTES_REQUEST,
  });
  
  export const searchQuotesSuccess = (quotes) => ({
    type: SEARCH_QUOTES_SUCCESS,
    payload: quotes,
  });
  
  export const searchQuotesFailure = (error) => ({
    type: SEARCH_QUOTES_FAILURE,
    payload: error,
  });
  
  export const fetchQuotesAsync = () => {
    return async (dispatch) => {
      dispatch(fetchQuotesRequest());
      try {
        const quotes = await fetchQuotesApi();
        dispatch(fetchQuotesSuccess(quotes));
      } catch (error) {
        dispatch(fetchQuotesFailure(error));
      }
    };
  };
  
  export const createQuoteAsync = (quoteDto) => {
    return async (dispatch) => {
      try {
        await createQuoteApi(quoteDto);
        dispatch(fetchQuotesAsync());
      } catch (error) {
        console.error('Error creating quote:', error);
        throw error;
      }
    };
  };
  
  export const updateQuoteAsync = (id, quoteDto) => {
    return async (dispatch) => {
      try {
        await updateQuoteApi(id, quoteDto);
        dispatch(fetchQuotesAsync());
      } catch (error) {
        console.error('Error updating quote:', error);
        throw error;
      }
    };
  };
  
  export const deleteQuoteAsync = (id) => {
    return async (dispatch) => {
      try {
        await deleteQuoteApi(id);
        dispatch(deleteQuoteSuccess(id));
      } catch (error) {
        console.error('Error deleting quote:', error);
        throw error;
      }
    };
  };
  
  export const searchQuotesAsync = (searchParams) => {
    return async (dispatch) => {
      dispatch(searchQuotesRequest());
      try {
        const quotes = await searchQuoteApi(searchParams);
        console.log(quotes);
        console.log("Action",searchParams);
        dispatch(searchQuotesSuccess(quotes));
      } catch (error) {
        dispatch(searchQuotesFailure(error));
      }
    };
  };
  