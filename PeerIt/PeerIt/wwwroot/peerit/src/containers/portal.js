// example of custom component with Webix UI inside
// this one is a static view, not linked to the React data store

import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import Webix from '../webix';
import AdminToolbar from '../widget/AdminToolbar';
import InstructorToolbar from '../widget/InstructorToolbar';
import StudentToolbar from '../widget/StudentToolbar';
 
class Portal extends Component {

  constructor(props) {
      super(props);
      this.state = {
        currentUser : props.currentUser,
        role : props.role,
        data : null
      };
      // Used to change user state from App.js
      this.handleLogin = props.handleLogin;
      this.levelCheck = "here";
      this.logout.bind(this);

    window.webix.protoUI({
      name:"react",
      defaults:{
        borderless:true
      },
      $init:function(config){
        this.$ready.push(function(){    
          ReactDOM.render(
            this.config.app,
            this.$view
          );
        });
      }
    }, window.webix.ui.view)
  }
renderAdminToolbar(){
  if(this.state.role === "Administrator"){
     return   <AdminToolbar currentUser={this.state.currentUser} role={this.state.role} logout={this.logout.bind(this)}/>
  }
}
renderInstructorToolbar(){
  if(this.state.role ==="Instructor"){
    return     <InstructorToolbar currentUser={this.state.currentUser} role={this.state.role} logout={this.logout.bind(this)}/>
  }
}
renderStudentToolbar(){
  if(this.state.role === "Student"){
    return     <StudentToolbar currentUser={this.state.currentUser} role={this.state.role} logout={this.logout.bind(this)}/>
  }
}
logout(){
  fetch("/Account/Logout", {
      method: 'GET', // or 'PUT'
     // body: JSON.stringify({"Email":userName,"Password":password,"returnUrl":null}), // data can be `string` or {object}!
      headers:{
        'Content-Type': 'application/json'
      },
      credentials: "include",
      mode:"cors"
    }).then(res => res.json())
    .then(response => {
      console.log('Success:', JSON.stringify(response))

      if(response.success){
        
          
      }else{
        let errors = "";
        response.error.forEach( error => {
          console.log(error);
          errors += error.description
        }); 
        
      }

    })
    .catch(error => console.error('Error:', error));

    //
    this.handleLogin(null,null);
}
  render(){

    let data = null;

     return(
      <div>
        {this.renderAdminToolbar()}
        {this.renderInstructorToolbar()}
        {this.renderStudentToolbar()}
    
    
      
      </div>
               
             
      );
  }
}

//const Portal = ({ data, save }) => (
  
//)
export default Portal;

