import React, { useEffect, useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { fetchQuotesAsync, deleteQuoteAsync, updateQuoteAsync } from '../actions/quoteActions';
import { getQuotes, getQuotesLoading, getQuotesError } from '../selectors/quoteSelectors';
import '../styles/QuoteList.css';
import QuoteSearch from './QuoteSearch';
import QuoteEditForm from './QuoteEditForm';


const QuoteList = () => {
    const dispatch = useDispatch();
    const quotes = useSelector(getQuotes);
    const loading = useSelector(getQuotesLoading);
    const error = useSelector(getQuotesError);

    const [editMode, setEditMode] = useState({});
    const [quoteToEdit, setQuoteToEdit] = useState(null);

    useEffect(() => {
        dispatch(fetchQuotesAsync());
    }, [dispatch]);

    const handleEditClick = (quote) => {
        setEditMode((prevState) => ({ ...prevState, [quote.id]: true }));
        setQuoteToEdit(quote);
    };

    const handleCancelClick = (id) => {
        setEditMode((prevState) => ({ ...prevState, [id]: false }));
        setQuoteToEdit(null);
    };

    const handleSave = async (updatedQuote) => {
        await dispatch(updateQuoteAsync(updatedQuote.id, updatedQuote));
        setEditMode((prevState) => ({ ...prevState, [updatedQuote.id]: false }));
        setQuoteToEdit(null);
    };

    const handleDelete = async (id) => {
        try {
            if (window.confirm('Are you sure you want to delete this quote?')) {
                await dispatch(deleteQuoteAsync(id));
            }
        } catch (error) {
            console.error('Error deleting quote: refreshing in 10 seconds', error);
        }
    };

    if (loading) return <p>Loading quotes...</p>;
    if (error) return <p>Error fetching quotes: {error.message}</p>;

    return (
        <div className="quote-list-container">
            <ul>
                {quotes.length !== 0 ? (
                    quotes.map((quote) => (
                        <li key={quote.id} className="quote-item">
                            {editMode[quote.id] ? (
                                <QuoteEditForm
                                    initialQuote={quoteToEdit}
                                    onSave={handleSave}
                                    onCancel={() => handleCancelClick(quote.id)}
                                />
                            ) : (
                                <>
                                    <p className='quote-text'>{quote.quoteText}</p>
                                    <p className='quote-author'>- {quote.author}</p>
                                    <div className='quote-tags-edit'>
                                        <div className='tags-container'>
                                            <strong className='tags-heading'>Tags</strong>
                                            <ul className='tagList-ul'>
                                                {quote.tags.map((tag, index) => (
                                                    <li key={index}>{tag}</li>
                                                ))}
                                            </ul>
                                        </div>
                                        <div className='edit-buttons'>
                                            <button className="edit-button" onClick={() => handleEditClick(quote)}>Edit</button>
                                            <button className="delete-button" onClick={() => handleDelete(quote.id)}>Delete</button>
                                        </div>
                                    </div>
                                </>
                            )}
                        </li>
                    ))
                ) : (
                    <p> No inspiring words at the moment—how about contributing one yourself? </p>
                )}
            </ul>
        </div>
    );
};

export default QuoteList;
