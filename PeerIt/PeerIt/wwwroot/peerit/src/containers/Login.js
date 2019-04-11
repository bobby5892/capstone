// example of custom component with Webix UI inside
// this one is a static view, not linked to the React data store

import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import Webix from '../webix';
import LoginForm from '../widget/LoginForm';
 
class Login extends Component {

  constructor(props) {
      super(props);
      this.state = {
        data : null
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
  doLogin(){
    alert("do login");
  }
  render(){
    let data = null;
    let ui = {
       type:"space",
       id:"a1",
       height: window.innerHeight,
       width:  window.innerWidth,
       minWidth:500,
       minHeight:500,
      rows:
                [{
                 type:"space", 
                 padding:50, 
                 responsive:"a1", 
                 margin:0,
                
                 cols:[
                        { view:"form", 
                            elements:[
                                { 
                                  type:"header",
                                  template:"Please Login"
                                },
                                { 
                                  view:"text",
                                  labelAlign:"top",
                                  labelPosition:"top", 
                                  name:"emailAddress",
                                  label:"Email Address",
                                  validate:"isNotEmpty",
                                  validateEvent:"key",
                                  value:"" ,
                                  
                                },
                                { 
                                  view:"text", 
                                  labelPosition:"top", 
                                  name:"password", 
                                  label:"Password",
                                  validate:"isNotEmpty", 
                                  validateEvent:"key",
                                  value:"" ,

                                }, {}, { view:"button", label: 'login', click:this.doLogin }
                          ]},

                       ], 
                       maxWidth:600,
                       minWidth:300 
                     }
                 ]};
     return(
      <div>
        <Webix ui={ui} data={data}/>
      </div>
               
             
      );
  }
}

//const Portal = ({ data, save }) => (
  
//)
export default Login;

