import React, { useState, useEffect } from 'react';
import { useDispatch } from 'react-redux';
import { createQuoteAsync } from '../actions/quoteActions';
import QuoteInput from './QuoteInput';
import '../styles/QuoteForm.css';

const QuoteEditForm = ({ initialQuote, onSave, onCancel }) => {
    const dispatch = useDispatch();
    const [quote, setQuote] = useState(initialQuote || { author: '', quoteText: '', tags: [''] });

    useEffect(() => {
        if (initialQuote) {
            setQuote(initialQuote);
        }
    }, [initialQuote]);

    const handleInputChange = (field, value) => {
        setQuote((prevQuote) => ({
            ...prevQuote,
            [field]: value,
        }));
    };

    const handleTagChange = (index, value) => {
        const newTags = [...quote.tags];
        newTags[index] = value;
        setQuote((prevQuote) => ({
            ...prevQuote,
            tags: newTags,
        }));
    };

    const handleAddTag = () => {
        setQuote((prevQuote) => ({
            ...prevQuote,
            tags: [...prevQuote.tags, ''],
        }));
    };

    const handleRemoveTag = (index) => {
        setQuote((prevQuote) => ({
            ...prevQuote,
            tags: prevQuote.tags.filter((_, i) => i !== index),
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        const quoteToSubmit = {
            ...quote,
            tags: quote.tags.filter((tag) => tag.trim() !== ''),
        };
        try {
            if (initialQuote) {
                onSave(quoteToSubmit);
            } else {
                await dispatch(createQuoteAsync(quoteToSubmit));
                setQuote({ author: '', quoteText: '', tags: [''] });
            }
        } catch (error) {
            console.error('Error creating quote:', error);
        }
    };

    return (
        <form onSubmit={handleSubmit}>
            <div className="quote-form">
                <QuoteInput
                    quote={quote}
                    onInputChange={handleInputChange}
                    onTagChange={handleTagChange}
                    onAddTag={handleAddTag}
                    onRemoveTag={handleRemoveTag}
                />
                <div className="edit-buttons">
                    <button type="submit">{initialQuote ? 'Save' : 'Submit'}</button>
                    {initialQuote && <button type="button" onClick={onCancel}>Cancel</button>}
                </div>
            </div>
        </form>
    );
};

export default QuoteEditForm;
