// example of custom component with Webix UI inside
// this one is a static view, not linked to the React data store

import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import Webix from '../webix';

class ManageUsers extends Component {

  constructor(props) {
      super(props);
      this.state = {
        currentUser : props.currentUser,
        role : props.role,
        // Lists of users
        administrators : null,
        instructors : null,
        students : null,
        invitedguests : null,
        // If editing a user
        editUser : null
    };
    // Start Data Load 
    if(this.state.administrators === null){ this.getData("/Admin/GetAdministrators","administrators"); }
    if(this.state.instructors === null){ this.getData("/Admin/GetInstructors","instructors"); }
    if(this.state.students === null){ this.getData("/Admin/GetStudents","students"); }
    if(this.state.invitedguests === null){this.getData("/Admin/GetInvitedGuests","invitedguests"); }
   
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
  getData(src,stateName){
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
             scope.setState({[stateName] : response.data});
           
            }

          })
          .catch(error => console.error('Error:', error));
  }
  saveEditWindow(){
    let scope = this;
    let formValues = window.webix.$$("editUserForm").getValues();
    let isEnabled = function() {
      if(scope.state.editUser.isEnabled == 1){
        return true;
      }
      return false;
    };
      // Save the edit window
      let req = "/Admin/Edit?ID=" +this.state.editUser.id + "&FirstName=" + formValues.FirstName + "&LastName=" + 
            formValues.LastName + "&Email=" + formValues.Email + "&Password=" +formValues.Password +
            "&Role=" + formValues.Role + "&IsEnabled=" + isEnabled();
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
              // This would cause a re-render
              scope.setState({"editUser" : null,"administrators" :null, "instructors" : null,"students" : null, "invitedguests" : null });
              // States no longer empty
              window.webix.$$("editUserWindow").close();
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
  newEditWindow(id){
   // console.log("open a window for" + id);
// Lets do a request that will cause this to reload
    let scope = this;
      if(this.state.editUser === null){
        fetch("/Admin/GetUser?id=" +id, {
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
              scope.setState({"editUser" : response.data[0]});
              // States no longer empty
              scope.renderEditWindow();
              //console.log("attempting to open render window");
            }
          })
          .catch(error => console.error('Error:', error));
    }
    
      
  }
  renderEditWindow(){
    console.log("rendering" + JSON.stringify(this.state.editUser));
    let scope = this;
    let isEnabled = function() {
      if(scope.state.editUser.isEnabled){
        return 1;
      }
      return 0;
    };
    var newWindow = window.webix.ui({
            view:"window",
            id:"editUserWindow",
            width: 500,
            height: 500,
            move:true,
            position:"center",
            head:{
                type:"space",
                cols:[
                    { view:"label", label: "Edit User" },
                    {
                      view:"button", label:"Close", width:70,left:250,
                      click:function(){
                        scope.setState({"editUser" : null });
                        window.webix.$$("editUserWindow").close();
                      } 
                    }
                 ]   
            },
            body:{
                type:"space",
                rows:[
                    { 
                      view:"form", 
                      id:"editUserForm",
                      width:400,
                      elements:[
                          { view:"checkbox", value: 0, label:"Active", name: "IsEnabled", value : isEnabled() },
                          { view:"text", label:"First Name", name:"FirstName", labelWidth:100,invalidMessage: "First Name can not be empty", value: this.state.editUser.firstName}, 
                          { view:"text", label:"Last Name", name:"LastName", labelWidth:100,invalidMessage: "Last Name can not be empty",value: this.state.editUser.lastName },
                          { view:"text", label:"Email", name:"Email", labelWidth:100,invalidMessage: "Must be valid email address",value: this.state.editUser.email },
                          { view:"text", type:"password", label:"Password", name:"Password", labelWidth:100, invalidMessage: "Password can not be empty" },
                           { view: "select", label:"Role",name:"Role", labelWidth:100,  value: this.state.editUser.role, options:[
                                                                                {"id" : "Administrator","value" : "Administrator"},
                                                                                {"id" :"Student","value" : "Student"},
                                                                                {"id" :"Instructor","value" :"Instructor" },
                                                                                {"id" :"InvitedGuest","value" : "InvitedGuest"}
                                                                              ]},
                          { margin:5, cols:[
                              { view:"button", value:"Edit User" , type:"form", click:function(){
                                scope.saveEditWindow();
                              }}
                          ]}
                      ],
                      rules:{
                          "Email": window.webix.rules.isEmail,
                          "LastName": window.webix.rules.isNotEmpty,
                          "FirstName": window.webix.rules.isNotEmpty,
                          "Password" :  window.webix.rules.isNotEmpty
                      }
                    }
                ]
            }
            
        }).show();
          }
  createUser(){
      // check for valid response
      let scope = this;
      let validResponse = window.webix.$$("newUserForm").validate();
      if(validResponse){
        let newUser = window.webix.$$("newUserForm").getValues();
        fetch("/Admin/Create?FirstName=" + newUser.FirstName
            +"&LastName= "+ newUser.LastName+"&Email="+ newUser.Email+"&Password="+ newUser.password, {
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
            //console.log("Attempted to Create UseR: " + JSON.stringify(response));
            // If the new user window is open close it
            if(window.webix.$$("newUserWindow") != null){
              window.webix.$$("newUserWindow").close()
            }
            console.log("open new window to:  " + response.data[0].id);
            scope.newEditWindow( response.data[0].id);
          }else{
            let errors = "";
            response.error.forEach( error => {
              errors += error.description
            }); 
            window.webix.$$("newUserForm").elements.newUserErrorLabel.setValue(errors);
          }

    })
    .catch(error => console.error('Error:', error));
      }

      
  }
  renderOnData(){
    // By doing it like this - it skips the first render and only loads webix if data
    /* {
                          view:"button", 
                          id:"my_button", 
                          value:"Button", 
                          type:"form", 
                          inputWidth:100 
                     },   
                     */
    if(
      (this.state.administrators != null) && 
      (this.state.instructors != null) && 
      (this.state.students != null)  && 
      (this.state.invitedguests != null) ){
      let scope = this;
      let ui = 
        {
              type:"space",
              rows:[
                      {      
                        view:"button", 
                        id:"createUser_button", 
                        value:"Create User", 
                        
                        inputWidth:100,
                        click: function (){
                          // Lets only allow once
                          if(window.webix.$$("newUserWindow") == null){
                            var newWindow = window.webix.ui({
                                    view:"window",
                                    id:"newUserWindow",
                                    width: 500,
                                    height: 500,
                                    move:true,
                                    position:"center",
                                    head:{
                                        type:"space",
                                        cols:[
                                            { view:"label", label: "Create New User" },
                                            {
                                              view:"button", label:"Close", width:70,left:250,
                                              click:function(){
                                                window.webix.$$("newUserWindow").close();
                                              } 
                                            }
                                         ]   
                                    },
                                    body:{
                                        type:"space",
                                        rows:[
                                            { 
                                              view:"form", 
                                              id:"newUserForm",
                                              width:400,
                                              elements:[
                                              
                                                  { view:"text", label:"First Name", name:"FirstName", labelWidth:100,invalidMessage: "First Name can not be empty"}, 
                                                  { view:"text", label:"Last Name", name:"LastName", labelWidth:100,invalidMessage: "Last Name can not be empty" },
                                                  { view:"text", label:"Email", name:"Email", labelWidth:100,invalidMessage: "Must be valid email address" },
                                                  { view:"text", type:"password", label:"Password", name:"Password", labelWidth:100, invalidMessage: "Password can not be empty" },
                                                  { view:"label", label:"", name:"newUserErrorLabel", labelWidth:100},
                                                  { margin:5, cols:[
                                                      { view:"button", value:"Create User" , type:"form", click:function(){
                                                        scope.createUser();
                                                      }}
                                                  ]}
                                              ],
                                              rules:{
                                                  "Email": window.webix.rules.isEmail,
                                                  "LastName": window.webix.rules.isNotEmpty,
                                                  "FirstName": window.webix.rules.isNotEmpty,
                                                  "Password" :  window.webix.rules.isNotEmpty
                                              }
                                            }
                                        ]
                                    }
                                    
                                }).show();
                          }
                        }
                      },
                      {
                        id: "ManageUsers",
                        view: "tabview",
                        fillspace:true,
                        cells: 
                          [
                            {
                              header:"Administrators",
                              view: "datatable",
                              columns:
                                [
                                  { id:"firstName", header:"First Name"},
                                  { id:"lastName", header:"Last Name"},
                                  { id:"email", header:"Email",fillspace:true}
                                ],
                                data: this.state.administrators,
                                autoheight:true,
                                scroll: false,
                                on:{ //https://docs.webix.com/api__link__ui.datatable_onclick_config.html
                                    // the default click behavior that is true for any datatable cell
                                    "onItemClick":function(id, e, trg){ 
                                      scope.newEditWindow(id);
                                    }
                                }
                            },
                            {
                              header:"Instructors",
                              view: "datatable",
                              columns:
                                [
                                  { id:"firstName", header:"First Name"},
                                  { id:"lastName", header:"Last Name"},
                                  { id:"email", header:"Email",fillspace:true}
                                ],
                                data: this.state.instructors,
                                autoheight:true,
                                scroll: false,
                                on:{ //https://docs.webix.com/api__link__ui.datatable_onclick_config.html
                                    // the default click behavior that is true for any datatable cell
                                    "onItemClick":function(id, e, trg){ 
                                      scope.newEditWindow(id);
                                    }
                                }
                              },
                              {
                                header:"Students",
                                view: "datatable",
                                columns:
                                  [
                                    { id:"firstName", header:"First Name"},
                                    { id:"lastName", header:"Last Name"},
                                    { id:"email", header:"Email",fillspace:true}
                                  ],
                                  data: this.state.students,
                                  autoheight:true,
                                  scroll: false,
                                  on:{ //https://docs.webix.com/api__link__ui.datatable_onclick_config.html
                                    // the default click behavior that is true for any datatable cell
                                    "onItemClick":function(id, e, trg){ 
                                      scope.newEditWindow(id);
                                    }
                                }
                              },
                              {
                                header:"Invited Guests",
                                view: "datatable",
                                columns:
                                  [
                                    { id:"firstName", header:"First Name"},
                                    { id:"lastName", header:"Last Name"},
                                    { id:"email", header:"Email",fillspace:true}
                                  ],
                                data: this.state.invitedguests,
                                autoheight:true,
                                scroll: false,
                                on:{ //https://docs.webix.com/api__link__ui.datatable_onclick_config.html
                                    // the default click behavior that is true for any datatable cell
                                    "onItemClick":function(id, e, trg){ 
                                      scope.newEditWindow(id);
                                    }
                                }
                              }            
                            ]
                          }
                    ]
                  
      };

     return <Webix ui={ui} />
    }
  }
  render(){
   // console.log(JSON.stringify(this.state.administrators));

     return(
      <div id="ManageUsers">
        {this.renderOnData()}
      </div>
      );
  }
 

  
}

export default ManageUsers;

