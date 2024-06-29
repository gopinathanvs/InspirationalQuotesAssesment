import React, { useState, useEffect } from 'react';
import { useDispatch } from 'react-redux';
import { fetchQuotesAsync, searchQuotesAsync } from '../actions/quoteActions';
import '../styles/QuoteSearch.css';

const QuoteSearch = () => {
    const dispatch = useDispatch();
    const [searchParams, setSearchParams] = useState({ author: '', tags: [''], text: '' });

    useEffect(() => {
        setSearchParams({ author: '', tags: [''], text: '' });
    }, []);

    const handleSearchInputChange = (e) => {
        const { name, value } = e.target;
        setSearchParams(prevState => ({ ...prevState, [name]: value }));
    };

    const handleTagInputChange = (e, index) => {
        const newTags = [...searchParams.tags];
        newTags[index] = e.target.value;
        setSearchParams(prevState => ({ ...prevState, tags: newTags }));
    };

    const handleAddTagInput = () => {
        setSearchParams(prevState => ({ ...prevState, tags: [...prevState.tags, ''] }));
    };

    const handleRemoveTagInput = (index) => {
        if (searchParams.tags.length > 1) {
            const newTags = [...searchParams.tags];
            newTags.splice(index, 1);
            setSearchParams(prevState => ({ ...prevState, tags: newTags }));
        }
    };

    const handleSearch = () => {
        console.log("Check", searchParams,);
        const { author, tags, text } = searchParams;
        console.log('Searching with:', { author, tags, text });
        dispatch(searchQuotesAsync(searchParams));
        console.log("Check", searchParams);
    };

    const handleSearchFieldRemove = () => {
        setSearchParams({ author: '', tags: [''], text: '' });
        dispatch(fetchQuotesAsync());
        console.log('Clearing search');
    };

    return (
        <div className="search-container">
            <div className='search-input-container'>
                <input
                    type="text"
                    name="author"
                    className="search-input"
                    placeholder="Search by author"
                    value={searchParams.author}
                    onChange={handleSearchInputChange}
                />
                {searchParams.tags.map((tag, index) => (
                    <div key={index}>
                        <input
                            type="text"
                            name="tags"
                            className="search-input"
                            placeholder="Search by tag"
                            value={tag}
                            onChange={(e) => handleTagInputChange(e, index)}
                        />
                        {index > 0 ? (
                            <button className="remove-tag-button" onClick={() => handleRemoveTagInput(index)}>-</button>
                        ) : <button className="add-tag-button" onClick={handleAddTagInput}>+</button>}
                    </div>
                ))}

                <input
                    type="text"
                    name="text"
                    className="search-input"
                    placeholder="Search by text"
                    value={searchParams.text}
                    onChange={handleSearchInputChange}
                />
            </div>
            <div className='search-buttons-container'>
                <button className="search-button" onClick={handleSearch}>Search</button>
                <button className="clear-button" onClick={handleSearchFieldRemove}>Clear Search</button>
            </div>
        </div>
    );
};

export default QuoteSearch;
