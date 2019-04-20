// example of custom component with Webix UI inside
// this one is a static view, not linked to the React data store

import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import Webix from '../webix';

 
class AdminButtons extends Component {

  constructor(props) {
      super(props);
      console.log(props);
      this.state = {
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
    let ui = {
       view:"button", 
        id:"my_button", 
        value:"Button", 
        type:"form", 
        inputWidth:100 
      };
    let data = null;
     return(
      <div>
   <Webix ui={ui} data={data}/>
      </div>
      );
  
  }
}


export default AdminButtons;

