// example of custom component with Webix UI inside
// this one is a static view, not linked to the React data store

import React, { Component } from 'react';
import ReactDOM from 'react-dom';
//import Webix from '../webix';

class AdminSettings extends Component {

  constructor(props) {
      super(props);
      this.state = {
        currentUser : props.currentUser,
        role : props.role,
        SMTP_Enabled :null,
        SMTP_USERNAME :null,
        SMTP_Port : null,
        SMTP_HOST : null
      };
      this.renderWindow = false;
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
    }, window.webix.ui.view);
    //load the content
    this.loadSettings();
  }


  loadSettings(){
    // This reaches out and grabs the settings
    // in the final then - it sets this.renderWindow = true
    if (this.state.SMTP_Enabled == null){
      this.getData("Settings/GetSettings");
    }
  }


  getData(src){
    let scope = this;
       fetch(src, {
            method: 'GET', // or 'PUT'
            headers:{
              'Content-Type': 'application/json'
            },
            credentials: "include",
            mode:"no-cors"
          }).then(res => res.json())
          .then(response => {
            if(response.success){
              // This would cause a re-render
              return response;
            }
          }).then(response => {
              this.renderWindow = true;
              let stateChange  = {
                   "SMTP_Enabled" :""+ scope.getSetting("SMTP_Enabled",response.data,"numeric"),
                   "SMTP_USERNAME" :""+ scope.getSetting("SMTP_USERNAME",response.data,"string"),
                   "SMTP_Port" :""+ scope.getSetting("SMTP_Port",response.data,"numeric"),
                   "SMTP_HOST" :""+ scope.getSetting("SMTP_HOST",response.data,"string")
                  };
              this.setState( stateChange );
          })
          .catch(error => console.error('Error:', error));
  }
  getSetting(setting,data,column){
    for(let i = 0;i<data.length;i++)
    {
        if(data[i].id === setting){
            if(column === "numeric"){
                return data[i].numericValue;
            }
            else{
                return data[i].stringValue;
            }
        }
    }
  }
  renderEditWindow(){
    if(this.renderWindow){
      window.webix.ui({
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
                          this.renderWindow = false;
                        }.bind(this)
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
                            { view:"checkbox", value: this.state.SMTP_Enabled, label:"Send Emails", name: "IsEnabled", labelWidth:100 },
                            { view:"text", label:"SMTP Server", name:"serverName", labelWidth:100,invalidMessage: "Server cannot be empty", value:this.state.SMTP_HOST},
                            { view:"text", label:"SMTP Port", name:"portNum", width:150, labelWidth:100, invalidMessage:"Port cannot be empty", value:this.state.SMTP_Port},
                            { view:"text", label:"SMTP Login", name:"smtpUsername", labelWidth:100,invalidMessage: "Please login to confirm changes",value: this.state.SMTP_USERNAME},
                            { view:"text", type:"password", label:"SMTP Password", name:"smtpPassword", labelWidth:125, invalidMessage: "Password can not be empty" },
                            { margin:5, cols:[
                                { view:"button", value:"Save Settings" , type:"form", click:function(){
                                  this.saveEditWindow();
                                  window.webix.$$("settingsWindow").close();
                                  this.renderWindow = false;
                                }.bind(this)
                              }
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
  }

  saveEditWindow(){
    
    let formValues = window.webix.$$("configurationForm").getValues();
      // Save the edit window
      let req = ("/Settings/EditSettings?isEnabledId=SMTP_Enabled&isEnabledValue=" + formValues.IsEnabled +
                "&serverId=SMTP_HOST&serverValue=" + formValues.serverName +
                "&portId=SMTP_Port&portValue=" + formValues.portNum +
                "&usernameId=SMTP_USERNAME&usernameValue=" + formValues.smtpUsername +
                "&passwordId=SMTP_Password&passwordValue=" + formValues.smtpPassword);
          console.log("Making Request: " + req);
        fetch(req, {
            method: 'POST', // or 'PUT'
            headers:{
              'Content-Type': 'application/json'
            },
            credentials: "include",
            mode:"no-cors"
          }).then(res => res.json())
          .then(response => {
            if(response.success){
              alert("All Changes Saved Successfully.")
              console.log("We did it!");
            }
          })
          .catch(error => console.error('Error:', error));
  }
  render(){
     return(
      <div id="AdminSettings">
        {this.renderEditWindow()}
      </div>
      );
  }
}

export default AdminSettings;