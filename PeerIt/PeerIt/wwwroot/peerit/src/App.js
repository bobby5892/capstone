import React, { Component } from 'react';
//import logo from './logo.svg';
import './App.css';
import Portal from './containers/portal.js';
import Login from './containers/Login.js';
class App extends Component {
constructor(props) {
    super(props);
    this.state = {
    	currentUser : null,
      baseUrl : "http://localhost:8080/",
      role : null
    };
    // Bind handle Login
    this.handleLogin = this.updateLogin.bind(this);
    
    // Remember to use this.setState({currentUser : something}); 
  }
 // / <Login  currentUser={this.state.currentUser}/>
  renderPortal(){
    
  	if(this.state.currentUser != null){
      console.log("render portal");
  		return <Portal baseUrl={this.state.baseUrl}/>
  	}
  }
  renderLogin(){
    
  	if(this.state.currentUser == null){
      console.log("render login");
  		return <Login baseUrl={this.state.baseUrl} handleLogin={this.handleLogin} />
  	}
  }
  updateLogin(user,role){
    console.log("state before: " + this.state.currentUser);
    this.setState({'currentUser':user,'role':role});
    console.log("updated user login");
    console.log("state after:" + this.state.currentUser);
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
