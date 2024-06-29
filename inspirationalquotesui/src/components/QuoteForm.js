import React, { useState } from 'react';
import { useDispatch } from 'react-redux';
import { createQuoteAsync } from '../actions/quoteActions';
import QuoteInput from './QuoteInput';
import '../styles/QuoteForm.css';
import { Link , useNavigate} from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faArrowLeft } from '@fortawesome/free-solid-svg-icons';

const QuoteForm = () => {
    const history = useNavigate();
    const dispatch = useDispatch();
    const [quotes, setQuotes] = useState([{ author: '', quoteText: '', tags: [''] }]);

    const handleInputChange = (index, field, value) => {
        const newQuotes = [...quotes];
        newQuotes[index][field] = value;
        setQuotes(newQuotes);
    };

    const handleTagChange = (quoteIndex, tagIndex, value) => {
        const newQuotes = [...quotes];
        newQuotes[quoteIndex].tags[tagIndex] = value;
        setQuotes(newQuotes);
    };

    const handleAddTag = (quoteIndex) => {
        const newQuotes = [...quotes];
        newQuotes[quoteIndex].tags.push('');
        setQuotes(newQuotes);
    };

    const handleRemoveTag = (quoteIndex, tagIndex) => {
        const newQuotes = [...quotes];
        newQuotes[quoteIndex].tags = newQuotes[quoteIndex].tags.filter((_, i) => i !== tagIndex);
        setQuotes(newQuotes);
    };

    const handleAddQuote = () => {
        setQuotes([...quotes, { author: '', quoteText: '', tags: [''] }]);
    };

    const handleRemoveQuote = (index) => {
        setQuotes(quotes.filter((_, i) => i !== index));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        const quotesToSubmit = quotes.map(quote => ({
            ...quote,
            tags: quote.tags.filter(tag => tag.trim() !== '')
        }));
        try {
            console.log(quotesToSubmit);
            await dispatch(createQuoteAsync(quotesToSubmit));
            setQuotes([{ author: '', quoteText: '', tags: [''] }]);
            history('/');
            window.alert('Quotes Successfully added');
        } catch (error) {
            console.error('Error creating quotes:', error);
        }
    };
    return (
        <div className='quote-form-container'>
            <div className='back-icon-container'>
                <Link to="/" className='back-icon' >
                <FontAwesomeIcon icon={faArrowLeft}/>
                </Link>
            </div>
        <h1 className='add-quotes-heading'>Add quotes</h1>
            <form onSubmit={handleSubmit}>
                {quotes.map((quote, quoteIndex) => (
                    <div key={quoteIndex} className="quote-form">
                        <QuoteInput
                            quote={quote}
                            onInputChange={(field, value) => handleInputChange(quoteIndex, field, value)}
                            onTagChange={(tagIndex, value) => handleTagChange(quoteIndex, tagIndex, value)}
                            onAddTag={() => handleAddTag(quoteIndex)}
                            onRemoveTag={(tagIndex) => handleRemoveTag(quoteIndex, tagIndex)}
                        />
                        {quotes.length > 1 && (
                            <button type="button" onClick={() => handleRemoveQuote(quoteIndex)}>
                                Remove Quote
                            </button>
                        )}
                    </div>
                ))}
                <button type="button" onClick={handleAddQuote}>
                    Add Another Quote
                </button>
                <button type="submit">Submit All Quotes</button>
            </form>
        </div>
    );
};

export default QuoteForm;
