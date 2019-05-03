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
    console.log("Cons State: " + JSON.stringify(this.state));
  }

  loadSettings(){
    // This reaches out and grabs the settings
    // in the final then - it sets this.renderWindow = true
    if (this.state.SMTP_Enabled == null){
      this.getData("Settings/GetSettings");
    }
  }

  getData(src){
    console.log("State in GetData: " + JSON.stringify(this.state));
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
              //console.log("data: " + JSON.stringify(response))
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
                console.log("stateChange: " + JSON.stringify(stateChange));
                this.setState( stateChange );
             
             console.log("SCOPE: " + JSON.stringify(scope.state));

          })
          .catch(error => console.error('Error:', error));
  }
  getSetting(setting,data,column){
    for(let i = 0;i<data.length;i++)
    {
        if(data[i].id == setting){
            if(column == "numeric"){
              console.log(data[i].numericValue);
                return data[i].numericValue;
            }
            else{
              console.log(data[i].stringValue);
                return data[i].stringValue;
            }
        }
    }
  }
 renderEditWindow(){
  if(this.renderWindow){
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
                            { view:"checkbox", value: scope.state.SMTP_Enabled, label:"Send Emails", name: "IsEnabled", labelWidth:100 },
                            { view:"text", label:"SMTP Server", name:"serverName", labelWidth:100,invalidMessage: "Server cannot be empty", value:scope.state.SMTP_HOST},
                            { view:"text", label:"SMTP Port", name:"portNum", width:150, labelWidth:100, invalidMessage:"Port cannot be empty", value:this.state.SMTP_Port},
                            { view:"text", label:"SMTP Login", name:"usernameAdminLogin", labelWidth:100,invalidMessage: "Please login to confirm changes",value: this.state.SMTP_USERNAME},
                            { view:"text", type:"password", label:"SMTP Password", name:"passwordAdminLogin", labelWidth:125, invalidMessage: "Password can not be empty" },
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
  }

    saveEditWindow(){
    let scope = this;
    let formValues = window.webix.$$("settingsWindow").getValues();
      // Save the edit window
      let req = "/Settings/EditSettings";
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
              
              //console.log("attempting to open render window");
              //Dirt Reload Data
              // Start Data Load 
              if(this.state.administrators === null){ this.getData("/Admin/GetAdministrators","administrators"); }
              if(this.state.instructors === null){ this.getData("/Admin/GetInstructors","instructors"); }
              if(this.state.students === null){ this.getData("/Admin/GetStudents","students"); }
              if(this.state.invitedguests === null){this.getData("/Admin/GetInvitedGuests","invitedguests"); }
             
            }
          })
          .catch(error => console.error('Error:', error));
      
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

