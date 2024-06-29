import React from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import { BrowserRouter as Router } from 'react-router-dom';
import { Provider } from 'react-redux';
import store from './store/store';
import Navigation from './routes/routes';

ReactDOM.render(
  <Provider store={store}>
    <Router  >
      <Navigation />
    </Router>
  </Provider>,
  document.getElementById('root')
);
