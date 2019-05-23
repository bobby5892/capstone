import React, { Component } from 'react';
//import logo from './logo.svg';
import './App.css';
import Portal from './containers/portal.js';
import Login from './containers/Login.js';
//https://facebook.github.io/create-react-app/docs/proxying-api-requests-in-development
class App extends Component {
  constructor(props) {
    super(props);
    this.state = {
      currentUser: null,
      role: null,
      viewingCourse: null,
      viewingAssignment: null,
      seed : (new Date).getTime()
    };
    // Bind handle Login
    this.handleLogin = this.updateLogin.bind(this);
    this.checkIfLoggedIn();
    // Remember to use this.setState({currentUser : something}); 
  }
  // / <Login  currentUser={this.state.currentUser}/>
  checkIfLoggedIn() {//https://stackoverflow.com/questions/38742379/cors-why-my-browser-doesnt-send-options-preflight-request/38746674#38746674
    fetch("/Account/GetCurrentUserRole", {
      method: 'GET',
      credentials: 'same-origin',
      cache: "no-cache",
      mode: 'cors',
      headers: {
        'Content-Type': 'application/json',
        'X-PINGOVER': "because"
      }
    }).then(
      res => {
        let json = res.json();
        return json;
      }
    )
      .then(response => {
        //console.log('Success:', JSON.stringify(response))
        if (response.success) {
          //console.log("show me " +  JSON.stringify(response));
          if ((response.data[0].role.length > 1) && (response.data[0].emailAddress.length > 1)) {
            // Update the state and include the user
            this.setState({ 'currentUser': response.data[0].emailAddress, 'role': response.data[0].role });
          }
        } else {
          response.error.forEach(error => {
            console.log(error);
          });
        }
      })
      .catch(error => console.error('Error:', error));
  }
  redrawAll(){
  	console.log("REDRAW ALL TRIGGERED");
  	this.setState({seed : (new Date).getTime()});
  }
  handleCourseViewer(statechange) {
    console.log("changing state" + JSON.stringify(statechange));
    this.setState(statechange);
  }
  renderPortal() {

    if (this.state.currentUser != null) {
      return <Portal
        currentUser={this.state.currentUser}
        role={this.state.role}
        handleLogin={this.handleLogin}
        viewingCourse={this.state.viewingCourse}
        handleCourseViewer={this.handleCourseViewer.bind(this)}
        redrawAll={this.redrawAll.bind(this)}
        seed={this.state.seed}
        viewingAssignment={this.state.viewingAssignment}

      />
    }
  }
  renderLogin() {

    if (this.state.currentUser == null) {
      return <Login handleLogin={this.handleLogin} />
    }
  }
  updateLogin(user, role) {
    this.setState({ 'currentUser': user, 'role': role });
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
