import {
    FETCH_QUOTES_REQUEST,
    FETCH_QUOTES_SUCCESS,
    FETCH_QUOTES_FAILURE,
    DELETE_QUOTE_SUCCESS,
    SEARCH_QUOTES_REQUEST,
    SEARCH_QUOTES_SUCCESS,
    SEARCH_QUOTES_FAILURE,
} from '../actions/quoteActions';

const initialState = {
    quotes: [],
    loading: false,
    error: null,
};

const quoteReducer = (state = initialState, action) => {
    switch (action.type) {
        case FETCH_QUOTES_REQUEST:
        case SEARCH_QUOTES_REQUEST:
            return {
                ...state,
                loading: true,
                error: null,
            };
        case FETCH_QUOTES_SUCCESS:
        case SEARCH_QUOTES_SUCCESS:
            return {
                ...state,
                quotes: action.payload,
                loading: false,
                error: null,
            };
        case FETCH_QUOTES_FAILURE:
        case SEARCH_QUOTES_FAILURE:
            return {
                ...state,
                loading: false,
                error: action.payload,
            };
        case DELETE_QUOTE_SUCCESS:
            return {
                ...state,
                quotes: state.quotes.filter((quote) => quote.id !== action.payload),
            };
        default:
            return state;
    }
};

export default quoteReducer;
