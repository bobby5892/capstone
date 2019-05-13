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
import CourseContent from '../containers/CourseContent';
class Portal extends Component {

  constructor(props) {
      super(props);
      this.state = {
        currentUser : props.currentUser,
        role : props.role,
        data : null,
        account : null,
        viewingCourse: props.viewingCourse,
        currentContentWidgets : ["LiveFeed"],
        seed : props.seed
      };
    this.redrawAll = props.redrawAll;
    this.handleCourseViewer = props.handleCourseViewer;
    // Used to change user state from App.js
    this.handleLogin = props.handleLogin;
    // Logout Function
    this.logout.bind(this);

      //Account menu click
      this.accountClick.bind(this);
      this.fetchAccountFormData.bind(this);

      //Upload a review
      this.uploadReview.bind(this);
    // Handle Menu Users Click
    this.handleMenuClick.bind(this);
    this.renderUploadAssignmentWindow.bind(this);
  }
  renderPortal() {
    //https://forum.webix.com/discussion/31137/reactjs-layout-components
    window.webix.protoUI({
      name: "portal",
      defaults: {
        borderless: true
      },
      $init: function (config) {
        this.$ready.push(function () {
          ReactDOM.render(
            this.config.app,
            this.$view
          );
        });
      }
    }, window.webix.ui.view)

  }

 componentWillReceiveProps(props){
  this.setState(props);
 }
  /* Listed Render - this is so we can control what gets passed to what widget/container */
  renderLiveFeed() {
    if (this.state.currentContentWidgets.includes("LiveFeed")) {
      return <LiveFeed currentUser={this.state.currentUser} role={this.state.role} logout={this.logout.bind(this)}  redrawAll={this.redrawAll} seed={this.state.seed}/>

    }
  }
  renderAdminToolbar() {
    if (this.state.role === "Administrator") {
      return <AdminToolbar currentUser={this.state.currentUser}
        role={this.state.role}
        logout={this.logout.bind(this)}
        handleMenuClick={this.handleMenuClick.bind(this)}
        viewingCourse={this.state.viewingCourse}
        handleCourseViewer={this.handleCourseViewer.bind(this)}
        accountClick={this.accountClick.bind(this)}
        redrawAll={this.redrawAll} seed={this.state.seed}
        />
    }
  }
  renderInstructorToolbar() {
    if (this.state.role === "Instructor") {
      return <InstructorToolbar currentUser={this.state.currentUser}
        role={this.state.role}
        logout={this.logout.bind(this)}
        handleCourseViewer={this.handleCourseViewer.bind(this)}
        handleMenuClick={this.handleMenuClick.bind(this)}
        viewingCourse={this.state.viewingCourse}
        accountClick={this.accountClick.bind(this)}
        redrawAll={this.redrawAll} seed={this.state.seed}
        />
    }
  }
  renderStudentToolbar() {
    if (this.state.role === "Student") {
      return <StudentToolbar 
      currentUser={this.state.currentUser} 
      role={this.state.role} 
      logout={this.logout.bind(this)}  
      handleMenuClick={this.handleMenuClick.bind(this)} 
      viewingCourse={this.state.viewingCourse}
      accountClick={this.accountClick.bind(this)}
      redrawAll={this.redrawAll} seed={this.state.seed}
      />
    }
  }
  renderCourseContent() {
    if (
        ((this.state.role === "Instructor") ||  (this.state.role === "Administrator")) 
        && (this.state.currentContentWidgets.includes("CourseContent"))
      )
      {
        return <CourseContent 
        currentUser={this.state.currentUser} 
        role={this.state.role} 
        handleMenuClick={this.handleMenuClick.bind(this)} 
        viewingCourse={this.state.viewingCourse}
        handleCourseViewer={this.handleCourseViewer.bind(this)}
        redrawAll={this.redrawAll} seed={this.state.seed}
        />
    }
  }
  renderAdminManageUsers() {
    if ((this.state.role === "Administrator") && (this.state.currentContentWidgets.includes("ManageUsers"))) {
      return <ManageUsers currentUser={this.state.currentUser} role={this.state.role} />
    }
  }
  renderAdminManageCourses() {
    if ((this.state.role === "Administrator") && (this.state.currentContentWidgets.includes("ManageCourses"))) {
      return <ManageCourses currentUser={this.state.currentUser} role={this.state.role} />
    }
  }
  renderAdminSettings() {
    if ((this.state.role === "Administrator") && (this.state.currentContentWidgets.includes("AdminSettings"))) {
      return <AdminSettings currentUser={this.state.currentUser} role={this.state.role} />
    }
  }

    handleMenuClick(contentWidget){
      this.setState({'currentContentWidgets' : contentWidget});
    }
    fetchAccountFormData(){
      fetch("/Account/PopulateFormData", {
        method: 'GET',
        headers:{
          'Content-Type': 'application/json'
        },
        credentials: "include",
        mode:"no-cors"
      }).then(res => res.json())
        .then(response => {
        //create an account object and assign its value to the "account" property of the state object.
        if(response.success){
          let account = {
            "FirstName" : response.data[0].firstName,
            "LastName" : response.data[0].lastName,
            "Email" : response.data[0].email
          }
          this.setState({"account":account});
            return true;
        }else{
    
          return false;
        }
      }).then(shouldRender => {
        if(shouldRender){
          this.renderAccountWindow();
        }
      })
      .catch(error => console.error('Error:', error));
    }
    //Click event for the "Account" link in the toolbar.
    accountClick(){
      this.fetchAccountFormData();
    }
    renderAccountWindow(){
       window.webix.ui({
               view:"window",
               id:"accountWindow",
               width: 900,
               height: 600,
               move:true,
               position:"center",
               head:{
                   type:"space",
                   cols:[
                       { view:"label", label: "Edit My Account" },
                       {
                         view:"button", label:"Close", width:70,left:250,
                         click:function(){
                           this.setState({"editUser" : null });
                           window.webix.$$("accountWindow").close();
                         }.bind(this)
                       }
                    ]   
               },
               body:{
                   type:"space",
                   rows:[
                       { 
                         view:"form", 
                         id:"editAccountForm",
                         width:900,
                         elements:[
                           { view:"label", label:"Your Email: "+this.state.account.Email, name:"Email", labelWidth:100,invalidMessage: "Must be valid email address",value:this.state.account.Email },
                             { view:"text", label:"First Name", name:"FirstName", labelWidth:100,invalidMessage: "First Name can not be empty", value:this.state.account.FirstName }, 
                             { view:"text", label:"Last Name", name:"LastName", labelWidth:100,invalidMessage: "Last Name can not be empty",value:this.state.account.LastName},
                             { view:"text", type:"password", label:"New Password", name:"Password1", labelWidth:160, invalidMessage: "Password can not be empty" },
                             { view:"text", type:"password", label:"Confirm Password", name:"Password2", labelWidth:160, invalidMessage: "Password can not be empty" },
                             
                             { margin:5, cols:[
                                 { view:"button", value:"Save Changes" , type:"form", click:function(){
                                   this.saveAccountChanges();
                                 }.bind(this)
                               }
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
    saveAccountChanges(){
      let properties = window.webix.$$("editAccountForm").getValues();
      let newPassword;
      if(properties.Password1 !== "" && properties.Password1 === properties.Password2)
      {
        newPassword = properties.Password2;
      }
      else{
        newPassword = "";
      }

      fetch("/Account/UpdateAccountInfo?email="+properties.Email+"&firstName="+properties.FirstName+
          "&lastName="+properties.LastName+"&password="+newPassword, {
        method: 'POST',
        headers:{
        'Content-Type': 'application/json'
        },
        credentials: "include",
        mode:"no-cors"
        }).then(res => res.json())
        .then(response => {
          if(response.success){
            let account = {
              "FirstName" : response.data[0].firstName,
              "LastName" : response.data[0].lastName,
              "Email" : response.data[0].email,
            }
            this.setState({"account":account});
              return true;
          }else
          {
            return false;
          }
        }).then(isChanged => {
          if(isChanged){
            window.webix.$$("accountWindow").close();
            this.logout();
          }
        })
        .catch(error => console.error('Error:', error));
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
            type: "space", id: "a2", rows:
              [
                {
                  type: "space",
                  padding: 0,
                  responsive: "a2",
                  height: window.innerHeight,
                  width: window.innerWidth * .7,
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
    uploadReview(){
      this.renderUploadReviewWindow();
    }
    
  renderUploadAssignmentWindow() {
    let scope = this;

    var newWindow = window.webix.ui({
      view: "window",
      id: "uploadAssignmentWindow",
      width:600,
      //height: 600,
      move: true,
      position: "center",
      head: {
        type: "space",
        cols: [
          { view: "label", label: "Upload an Assignment" },
          {
            view: "button", label: "Close", 
            width: 70, 
            left: 250,
            click: function () {
              //scope.setState({"editUser" : null });
              window.webix.$$("uploadAssignmentWindow").close();
            }
          }
        ]
      },
      body: {
        type: "space",
        rows: [
          {
            view: "form",
            id: "uploadAssignmentForm",
            //width: "auto",
            elements: [
              { view: "label", label: "Upload your Assignment form here: ", name: "", labelWidth: "auto", value: "" },
              {
                
                view: "uploader", inputName: "files", upload: "/CourseAssignment/CreateAssignment", 
                //urlData:{assignmentname:"new assignment",courseID:13,dueDate:"07/08/2019"} ,
                id: "AssignmentFile", link: "mylist", value: "Upload File", autosend: false
              },
              {
                view: "list", id: "mylist", type: "uploader",
                autoheight: true, borderless: true
              },
              
              { view: "text", label: "Assignment Name", name: "Assignment_Name",labelWidth: 200,invalidMessage:"Please enter Assignment Name" },
              { view: "text", label: "Course", name: "Course", labelWidth: 200,invalidMessage:"What Course is this? " },
              {view:"calendar",
              id:"my_calendar",
              name:"Due_Date",
              date:new Date(
                ),
              weekHeader:true,
              events:window.webix.Date.isHoliday,
              width:300,
              height:250},
              
              //{ view: "text", label: "Due Date", name: "Due_Date", labelWidth: 200,invalidMessage:"Please enter Valid Date" }, 
              {
                view: "button",value:"Upload", type:"form", 
                click: function () {
                  let validResponse = window.webix.$$("uploadAssignmentForm").validate();
                  let FormVal = window.webix.$$("uploadAssignmentForm").getValues();
                  window.webix.$$("AssignmentFile").define({
                    urlData:{assignmentname:FormVal.Assignment_Name,
                    courseID:FormVal.Course,
                    dueDate:(window.webix.$$("my_calendar").config.date).toUTCString()
                    }
                  });
                  window.webix.$$("AssignmentFile").send(function(response) {
                    console.log("upload send: " + JSON.stringify(response));
                    if (response != null){
                      console.log(Date("07/08/2019"));
                      window.webix.message("Succsess");
                      //window.webix.$$("uploadAssignmentWindow").attachEvent("onUploadComplete", function(response){
                        window.webix.$$("uploadAssignmentWindow").close();
                        //window.webix.message("done");
                    //}); 
                    }
                    else {
                    //console.log(Date("07/08/2019"));
                    alert("Nothing to Submit");
                  }})
                }
              }
            ],
            rules: {
              "Course": window.webix.rules.isNotEmpty,
              "Due_Date": window.webix.rules.isNotEmpty,
              "Assignment_Name":window.webix.rules.isNotEmpty
            }
          }
        ]
      }
    }).show();
  }
    renderUploadReviewWindow(){
        window.webix.ui({
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
                            { view:"uploader",inputName:"files",upload:"/Review/UploadReview" ,urlData:{studentAssignmentId:35} ,name:"ReviewFile",value:"Click here to upload your review file"},
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
          headers: {
            'Content-Type': 'application/json'
          },
          credentials: "include",
          mode: "no-cors"
        }).then(res => res.json())
      .then(response => {
        if (response.success) {
          this.handleLogin(null, null);
          // Don't remove the comment below - its actually functional.
          //eslint-disable-next-line
          document.location.reload();
        } else {
            console.log(response.error);
        }
      })
      .catch(error => console.error('Error:', error));
    
  }
  render() {
    /*Portal Container */
    
    let toolbar = function () {
      if (this.state.role === "Administrator") {
        return "AdminToolbar";
      }
      else if (this.state.role === "Instructor") {
        return "InstructorToolbar";
      }
      else if (this.state.role === "Student") {
        return "StudentToolbar";
      }
      else {

      }
    }.bind(this);
    let data = null;
    let ui =
    {
      type: "space", id: "a1", rows:
        [
          {
            type: "space",
            scroll: "auto",
            padding: 0,
            responsive: "a1",
            height: window.innerHeight,
            width: window.innerWidth,
            cols:
              [
                {
                  view: "template",
                  scroll: false,
                  gravity: 1,
                  width:350,
                  template: "left",
                  content: toolbar()
                },
                {
                  view: "template",
                  scroll: false,
                  gravity: 4,
                  template: "right",
                  content: "MultipleContentWidgets"
                }
              ]
          }
        ]
    };

    return (
      <div id="Portal">
        {this.renderAdminToolbar()}
        {this.renderInstructorToolbar()}
        {this.renderStudentToolbar()}
        <Webix ui={ui} data={data} />
        <div id="MultipleContentWidgets">
          {this.renderLiveFeed()}
          {this.renderAdminManageUsers()}
          {this.renderAdminManageCourses()}
          {this.renderAdminSettings()}
          {this.renderCourseContent()}
        </div>

      </div>
    );
  }
}
export default Portal;

