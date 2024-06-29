import React from 'react';
import '../styles/QuoteForm.css'

const QuoteInput = ({ quote, onInputChange, onTagChange, onAddTag, onRemoveTag }) => {
  return (
    <div >
      <div>
        <label>
          Author:
          <input
            type="text"
            value={quote.author}
            onChange={(e) => onInputChange('author', e.target.value)}
            required
          />
        </label>

        <label>
          Quote Text:
          <textarea
            value={quote.quoteText}
            onChange={(e) => onInputChange('quoteText', e.target.value)}
            required
          />
        </label>

        <label>
          Tags:
          {quote.tags.map((tag, index) => (
            <div key={index} className="tag-input-container">
              <input
                type="text"
                value={tag}
                onChange={(e) => onTagChange(index, e.target.value)}
                required
              />
              {quote.tags.length > 1 && (
                <button type="button" onClick={() => onRemoveTag(index)}>
                  -
                </button>
              )}
            </div>
          ))}
          <button type="button" onClick={onAddTag}>
            Add Another Tag
          </button>
        </label>
      </div>
    </div>
  );
};

export default QuoteInput;
