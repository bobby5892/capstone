import React, { Component } from 'react';
//import logo from './logo.svg';
import './App.css';
import Portal from './containers/portal.js';
import Login from './containers/Login.js';
class App extends Component {
constructor(props) {
    super(props);
    this.state = {
    	currentUser : null
    };
    
    // Remember to use this.setState({currentUser : something}); 
  }
 // / <Login  currentUser={this.state.currentUser}/>
  renderPortal(){
  	if(this.state.currentUser != null){
  		return <Portal currentUser={this.state.currentUser}/>
  	}
  }
  renderLogin(){
  	if(this.state.currentUser == null){
  		return <Login currentUser={this.state.currentUser}/>
  	}
  }
  render() {
   return (
   <div className="appContainer">
      {this.renderLogin()}
      {this.renderPortal()} 
    </div>
    );
  }
}

export default App;
