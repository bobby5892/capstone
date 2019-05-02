// example of custom component with Webix UI inside
// this one is a static view, not linked to the React data store

import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import Webix from '../webix';

class AdminSettings extends Component {

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
 renderEditWindow(){
    console.log("rendering" + JSON.stringify(this.state.editUser));
    let scope = this;
    var newWindow = window.webix.ui({
            view:"window",
            id:"settingsWindow",
            width: 500,
            height: 500,
            move:true,
            position:"center",
            head:{
                type:"space",
                cols:[
                    { view:"label", label: "Settings" },
                    {
                      view:"button", label:"Close", width:70,left:250,
                      click:function(){
                        //scope.setState({"editUser" : null });
                        window.webix.$$("settingsWindow").close();
                      } 
                    }
                 ]   
            },
            body:{
                type:"space",
                rows:[
                    { 
                      view:"form", 
                      id:"configurationForm",
                      width:400,
                      elements:[
                          { view:"checkbox", value: 0, label:"Send Emails", name: "IsEnabled", labelWidth:100 },
                          { view:"text", label:"Server", name:"serverName", labelWidth:100,invalidMessage: "Server cannot be empty", value:""},
                          { view:"text", label:"Port", name:"portNum", width:150, labelWidth:100, invalidMessage:"Port cannot be empty", value:""},
                          { view:"text", label:"Login", name:"usernameAdminLogin", labelWidth:100,invalidMessage: "Please login to confirm changes",value: ""},
                          { view:"text", type:"password", label:"Password", name:"passwordAdminLogin", labelWidth:100, invalidMessage: "Password can not be empty" },
                          { margin:5, cols:[
                              { view:"button", value:"Save Settings" , type:"form", click:function(){
                               // scope.saveEditWindow();
                               alert("save settings");
                              }}
                          ]}
                      ],
                      rules:{
                          "serverName": window.webix.rules.isNotEmpty,
                          "portNum": window.webix.rules.isNotEmpty,
                          "usernameAdminLogin": window.webix.rules.isEmail,
                          "Password" :  window.webix.rules.isNotEmpty
                      }
                    }
                ]
            }
            
        }).show();
          }

  render(){

    let data = null;

     return(
      <div id="AdminSettings">
        {this.renderEditWindow()}
      
      </div>
               
             
      );
  }
}

export default AdminSettings;

