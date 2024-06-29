import React from 'react';
import { BrowserRouter as Router, Link } from 'react-router-dom';
import './App.css';
import QuoteList from './components/QuoteList';
import QuoteSearch from './components/QuoteSearch';


const App = () => {
  return (
    <div>
      <h1 className='Insiprational-Quotes-heading'>Inspirational Quotes</h1>
      <div className="app-container">
        <div className="app-header">
          <nav>
            <ul>
              <button> <li><Link to="/add-quote">Add Quote</Link></li> </button>
            </ul>
          </nav>
        </div>
        <QuoteSearch />
        <QuoteList />
      </div>
    </div>
  );
};

export default App;
