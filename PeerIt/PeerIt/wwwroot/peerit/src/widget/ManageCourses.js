// example of custom component with Webix UI inside
// this one is a static view, not linked to the React data store

import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import Webix from '../webix';

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
  render(){

    let data = null;

     return(
      <div id="ManageCourses">
   manage courses 
   
      
      </div>
               
             
      );
  }
}

export default ManageCourses;

