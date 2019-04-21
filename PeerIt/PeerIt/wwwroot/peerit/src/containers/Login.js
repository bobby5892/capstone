// example of custom component with Webix UI inside
// this one is a static view, not linked to the React data store

import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import Webix from '../webix';

 
class Login extends Component {

  constructor(props) {
      super(props);
      console.log(props);
      this.state = {
        data : null,
      };
      //grab the update Login method
      this.updateLogin = props.handleLogin;
       console.log(this.state);

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
  clearError(){
    console.log("clear error");
     window.webix.$$("loginForm").elements.passwordErrorLabel.setValue("");
  }
 doLogin(){
    let userName = window.webix.$$("loginForm").elements.emailAddress.getValue();
    let password = window.webix.$$("loginForm").elements.password.getValue();
    console.log("Username: " + userName + " Password: " + password + " baseUrl:" + this.state);
   
    fetch("/Account/Login?Email=" + userName + "&Password=" + password + "&returnUrl=", {
      method: 'POST', // or 'PUT'
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
        this.updateLogin(response.data[0].emailAddress,response.data[0].role);
      }else{
        let errors = "";
        response.error.forEach( error => {
          console.log(error);
          errors += error.description
        }); 
        window.webix.$$("loginForm").elements.passwordErrorLabel.setValue(errors);
      }

    })
    .catch(error => console.error('Error:', error));

  }
  render(){
    let data = null;
    let ui = {
       type:"space",
       id:"a1",
       height: window.innerHeight-5,
       width:  window.innerWidth-5,
       minWidth:500,  
       minHeight:500,
      rows:
                [{
                 type:"space", 
                 padding:0, 
                 responsive:"a1", 
                 margin:0,
                
                 cols:[
                        {},
                        { 
                            view:"form", 
                            id:"loginForm",
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
                                  value:"" 
                                },
                                { 
                                 
                                  view:"text", 
                                  labelPosition:"top",
                                  type:"password", 
                                  name:"password", 
                                  label:"Password",
                                  validate:"isNotEmpty", 
                                  validateEvent:"key",
                                  value:"",
                                  KeyPress : function(code,e) {
                                    console.log("KEY PRESSED");
                                    console.log(code);
                                    console.log(e);
                                  }
                                },
                                { 
                                  view:"label", 
                                  name:"passwordErrorLabel", 
                                  label:"" 
                                },

                                 {}, { view:"button", label: 'login', click:this.doLogin.bind(this)}
                          ],
                            width:500,
                            height:500,
                            gravity:0.3
                        },
                        {}
                       ]
                     }
                 ]};

                // Now lets check for returns
               //console.log(window.webix.$$("loginForm").elements);
               
     

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

