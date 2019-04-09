import React, { Component } from 'react';
import logo from './logo.svg';
import './App.css';
import Portal from './containers/portal.js'
class App extends Component {
  render() {
   return (
   <div className="container">
    
      <div className="container">
        test<Portal/>
      </div>
    </div>
    );
  }
}

export default App;
