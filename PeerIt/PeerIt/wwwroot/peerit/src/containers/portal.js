// example of custom component with Webix UI inside
// this one is a static view, not linked to the React data store

import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import Webix from '../webix';
import AdminToolbar from '../widget/AdminToolbar';
import InstructorToolbar from '../widget/InstructorToolbar';
import StudentToolbar from '../widget/StudentToolbar';
import ManageUsers from '../widget/ManageUsers';
import ManageCourses from '../widget/ManageCourses';
import AdminSettings from '../widget/AdminSettings';
import LiveFeed from '../widget/LiveFeed';
class Portal extends Component {

  constructor(props) {
      super(props);
      this.state = {
        currentUser : props.currentUser,
        role : props.role,
        data : null,
        currentContentWidgets : ["LiveFeed"]
      };
      console.log(props.role);
      // Used to change user state from App.js
      this.handleLogin = props.handleLogin;
      
      // Logout Function
      this.logout.bind(this);

      // Handle Menu Users Click
      this.handleMenuClick.bind(this);

      //Upload a review
      this.uploadReview.bind(this);
  }
renderPortal(){
  //https://forum.webix.com/discussion/31137/reactjs-layout-components
    window.webix.protoUI({
      name:"portal",
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
/* Listed Render - this is so we can control what gets passed to what widget/container */
renderLiveFeed(){
  if(this.state.currentContentWidgets.includes("LiveFeed")){
    return   <LiveFeed currentUser={this.state.currentUser} role={this.state.role} logout={this.logout.bind(this)}/>
  }
}
renderAdminToolbar(){
  if(this.state.role === "Administrator"){
     return   <AdminToolbar currentUser={this.state.currentUser} role={this.state.role} logout={this.logout.bind(this)} handleMenuClick={this.handleMenuClick.bind(this)}/>
  }
}
renderInstructorToolbar(){
  if(this.state.role ==="Instructor"){
   return <InstructorToolbar currentUser={this.state.currentUser} role={this.state.role} logout={this.logout.bind(this)}/>
  }
}
renderStudentToolbar(){
  if(this.state.role === "Student"){
    return <StudentToolbar currentUser={this.state.currentUser} uploadReview={this.uploadReview.bind(this)} role={this.state.role} logout={this.logout.bind(this)}/>
  }
}
renderAdminManageUsers(){
  if((this.state.role === "Administrator") && (this.state.currentContentWidgets.includes("ManageUsers"))){
    return <ManageUsers currentUser={this.state.currentUser} role={this.state.role} />
  }
}
renderAdminManageCourses(){
  if((this.state.role === "Administrator") && (this.state.currentContentWidgets.includes("ManageCourses"))){
      return <ManageCourses currentUser={this.state.currentUser} role={this.state.role} />
  }
}
renderAdminSettings(){
  if((this.state.role === "Administrator") && (this.state.currentContentWidgets.includes("AdminSettings"))){
      return <AdminSettings currentUser={this.state.currentUser} role={this.state.role} />
  }
}
handleMenuClick(contentWidget){
  this.setState({'currentContentWidgets' : contentWidget});
}
// Handles the sub content Widgts 
renderMultipleContentWidgets(){
  if(this.state.currentContentWidgets != null){
    let output = [];
// Lets go thru each of the requested widgets
    for(let i=0; i<= this.state.currentContentWidgets.Length; i++){
      // lets build the webix template that will get stacked in the portal
      output.push( 
          {  
            view:"template", 
            scroll:false,
            content: this.state.currentContentWidgets[i]
         });
    }
     let ui = 
        {
          type:"space", id:"a2", rows:
            [
              {
                 type:"space", 
                 padding:0, 
                 responsive:"a2", 
                 height: window.innerHeight,
                 width: window.innerWidth*.7,
                 cols:
                 [
                    output
                 ]
              }
            ]
        };
    return ui;
  }
}
// Portal additional methods

//Upload a review
uploadReview(){
  console.log("upload a revew");
  this.renderUploadReviewWindow();
}

renderUploadReviewWindow(){
    let scope = this;

    var newWindow = window.webix.ui({
            view:"window",
            id:"uploadReviewWindow",
            width: 900,
            height: 600,
            move:true,
            position:"center",
            head:{
                type:"space",
                cols:[
                    { view:"label", label: "Upload a Review" },
                    {
                      view:"button", label:"Close", width:70,left:250,
                      click:function(){
                        //scope.setState({"editUser" : null });
                        window.webix.$$("uploadReviewWindow").close();
                      } 
                    }
                 ]   
            },
            body:{
                type:"space",
                rows:[
                    { 
                      view:"form", 
                      id:"uploadReviewForm",
                      width:900,
                      elements:[
                        { view:"label", label:"Upload your review form here: ", name:"", labelWidth:100,value:"" },
                        {view:"uploader",inputName:"files",upload:"/PFile/UploadReview" ,urlData:{studentAssignmentId:35} ,name:"ReviewFile",value:"Click here to upload your review file"},
                        { view:"text", label:"Course", name:"Course", labelWidth:100, value:""}, 
                        { view:"text", label:"Assignment", name:"Assignment", labelWidth:100, value:""},
                          
                          // { margin:5, cols:[
                          //     { view:"button", value:"Upload" , type:"form", click:function(){
                          //       scope.uploadTheReviewDoc();
                          //     }}
                          // ]}
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
    /*Portal Container */
    let scope = this;
      let toolbar = function(){ 
          if(scope.state.role == "Administrator"){
              return "AdminToolbar";
          }
          else if(scope.state.role == "Instructor"){
            return "InstructorToolbar";
          }
          else if (scope.state.role == "Student"){
            return "StudentToolbar";
          }
          else{
            
          }
        }
      let data = null;
      let ui = 
        {
          type:"space", id:"a1", rows:
            [
              {
                 type:"space", 
                 padding:0, 
                 responsive:"a1", 
                 height: window.innerHeight,
                 width: window.innerWidth,
                 cols:
                 [
                     { 
                        view:"template", 
                        scroll:false,
                        width:200,
                        content: toolbar()
                     },
                     {  view:"template", 
                        scroll:false,
                        content: "MultipleContentWidgets"
                      }
                ]
              }
            ]
        };

     return(
      <div id="Portal">
        {this.renderAdminToolbar()}
        {this.renderInstructorToolbar()}
        {this.renderStudentToolbar()}
        <Webix ui={ui} data={data}/>
        <div id="MultipleContentWidgets">
          {this.renderLiveFeed()}
          {this.renderAdminManageUsers()}
          {this.renderAdminManageCourses()}
          {this.renderAdminSettings()}
        </div>
      </div>
      );
  }
}
export default Portal;

