import { createStore, applyMiddleware, combineReducers } from 'redux';
import { thunk } from 'redux-thunk';
import quoteReducer from '../reducers/quoteReducer';


const rootReducer = combineReducers({
  quotes: quoteReducer,
});

const middleware = applyMiddleware(thunk);

const store = createStore(
  rootReducer,
  middleware
);

export default store;
