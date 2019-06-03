// example of custom component with Webix UI inside
// this one is a static view, not linked to the React data store

import React, { Component } from 'react';
import ReactDOM from 'react-dom';
//import Webix from '../webix';

class ManageCourses extends Component {

  constructor(props) {
      super(props);
      this.state = {
        currentUser : props.currentUser,
        role : props.role,
     
      };

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
  Createcourse(){
  let scope = this;
      let validResponse = window.webix.$$("newCourseForm").validate();
      if(validResponse){
        let newCourse = window.webix.$$("newCourseForm").getValues();
        fetch("/Course/CreateCourse?courseName=", {
          method: 'POST', // or 'PUT'
          //body: JSON.stringify({"FirstName":newUser.FirstName,"LastName":newUser.LastName,"Email":newUser.Email,"Password":newUser.password}), // data can be `string` or {object}!
          headers:{
            'Content-Type': 'application/json'
          },
          credentials: "include",
          mode:"no-cors"
        }).then(res => res.json())
        .then(response => {
          if(response.success){
            //console.log("Attempted to Create User: " + JSON.stringify(response));
            // If the new user window is open close it
            if(window.webix.$$("newCourseWindow") != null){
              window.webix.$$("newCourseWindow").close()
            }
            console.log("open new window to:  " + response.data[0].id);
            scope.newEditWindow( response.data[0].id);
          }else{
            let errors = "";
            response.error.forEach( error => {
              errors += error.description
            }); 
            window.webix.$$("newCourseForm").elements.newUserErrorLabel.setValue(errors);
          }

    })
    .catch(error => console.error('Error:', error));
      }
  }
  render(){

   // let data = null;

     return(
      <div id="ManageCourses">
        manage courses
      </div>
               
             
      );
  }
}

export default ManageCourses;