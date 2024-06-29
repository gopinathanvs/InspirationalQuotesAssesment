import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import App from '../App';
import QuoteForm from '../components/QuoteForm';

const Navigation = () => {
  return (
    <>
    <Routes>
      <Route path="/" element={<App />} />
      <Route path="/add-quote" element={<QuoteForm />} />
    </Routes>
    </>
  );
};

export default Navigation;
