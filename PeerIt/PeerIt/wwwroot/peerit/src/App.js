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
    this.checkIfLoggedIn();
    // Remember to use this.setState({currentUser : something}); 
  }
 // / <Login  currentUser={this.state.currentUser}/>
 checkIfLoggedIn(){
   fetch(this.state.baseUrl+"Account/GetCurrentUserRole", {
        method: 'GET' // or 'PUT'
       // body: JSON.stringify({"Email":userName,"Password":password,"returnUrl":null}), // data can be `string` or {object}!
        

        


      }).then(
        res => {
          let json = res.json();
          console.log(res);
          return json;
        }
      )
      .then(response => {
        //console.log('Success:', JSON.stringify(response))
        if(response.success){
          console.log("show me " +  JSON.stringify(response));
        }else{
          let errors = "";
          response.error.forEach( error => {
            console.log(error);
            errors += error.description
          }); 
          
        }

      })
      .catch(error => console.error('Error:', error));


 }
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
    this.setState({'currentUser':user,'role':role});
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
