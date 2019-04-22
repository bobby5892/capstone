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
    this.checkLogin();
    console.log(this.state);
    // Bind handle Login
    this.handleLogin = this.updateLogin.bind(this);
    
    // Remember to use this.setState({currentUser : something}); 
  }
 // / <Login  currentUser={this.state.currentUser}/>
 checkLogin(){

      if (true){//this.state.currentUser == null){
      fetch("http://localhost:8080/Account/GetCurrentUserAndRole?" + 
        (new Date()).getTime(), { method : 'GET' , 
        headers:{
          //'Content-Type': 'application/json',
          'Accept': 'application/json'
        }
      }).then(res => {
          console.log(res);
          return res.json();     
        }) 
            .then(response => {
        console.log("CheckkingLogin: " + JSON.stringify(response));
      });
  }
 } 
 
 
 renderPortal(){
    
  	if(this.state.currentUser != null){
      console.log("render portal");
  		return <Portal baseUrl={this.state.baseUrl} currentUser={this.state.currentUser} role={this.state.role}/>
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
    
    this.checkLogin();
    console.log("i got here");
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
