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
      currentUser: props.currentUser,
      role: props.role,
      data: null,
      currentContentWidgets: ["LiveFeed"]
    };
    console.log(props.role);
    // Used to change user state from App.js
    this.handleLogin = props.handleLogin;

    // Logout Function
    this.logout.bind(this);

    // Handle Menu Users Click
    this.handleMenuClick.bind(this);

    // handle create course window
    this.handleCreateCourse = this.renderCreateCourseWindow;



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
  /* Listed Render - this is so we can control what gets passed to what widget/container */
  renderLiveFeed() {
    if (this.state.currentContentWidgets.includes("LiveFeed")) {
      return <LiveFeed currentUser={this.state.currentUser} role={this.state.role} logout={this.logout.bind(this)} />
    }
  }
  renderAdminToolbar() {
    if (this.state.role === "Administrator") {
      return <AdminToolbar currentUser={this.state.currentUser}
        role={this.state.role}
        logout={this.logout.bind(this)}
        handleMenuClick={this.handleMenuClick.bind(this)}
        handleCreateCourse={this.renderCreateCourseWindow.bind(this)} />
    }
  }
  renderInstructorToolbar() {
    if (this.state.role === "Instructor") {
      return <InstructorToolbar currentUser={this.state.currentUser}
        role={this.state.role}
        logout={this.logout.bind(this)}
        handleCreateCourse={this.renderCreateCourseWindow.bind(this)} />
    }
  }
  renderStudentToolbar() {
    if (this.state.role === "Student") {
      return <StudentToolbar currentUser={this.state.currentUser} role={this.state.role} logout={this.logout.bind(this)} />
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
  handleMenuClick(contentWidget) {
    this.setState({ 'currentContentWidgets': contentWidget });
  }
  // Handles the sub content Widgts 
  renderMultipleContentWidgets() {
    if (this.state.currentContentWidgets != null) {
      let output = [];
      // Lets go thru each of the requested widgets
      for (let i = 0; i <= this.state.currentContentWidgets.Length; i++) {
        // lets build the webix template that will get stacked in the portal
        output.push(
          {
            view: "template",
            scroll: false,
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
  renderCreateCourseWindow() {
    let scope = this;
    if (window.webix.$$("newCourseWindow") == null) {
      var newWindow = window.webix.ui({
        view: "window",
        id: "newCourseWindow",
        width: 500,
        height: 500,
        move: true,
        position: "center",
        head: {
          type: "space",
          cols: [
            { view: "label", label: "Create New Course" },
            {
              view: "button", label: "Close", width: 70, left: 250,
              click: function () {
                window.webix.$$("newCourseWindow").close();
              }
            }
          ]
        },
        body: {
          type: "space",
          rows: [
            {
              view: "form",
              id: "newCourseForm",
              width: 400,
              elements: [

                { view: "text", label: "Course Name", name: "CourseName", labelWidth: 100, invalidMessage: "Course Name can not be empty" },
                // { view:"text", label:"Last Name", name:"LastName", labelWidth:100,invalidMessage: "Last Name can not be empty" },
                // { view:"text", label:"Email", name:"Email", labelWidth:100,invalidMessage: "Must be valid email address" },
                // { view:"text", type:"password", label:"Password", name:"Password", labelWidth:100, invalidMessage: "Password can not be empty" },
                // { view:"label", label:"", name:"newUserErrorLabel", labelWidth:100},
                {
                  margin: 5, cols: [
                    {
                      view: "button", value: "Create Course", type: "form", click: function () {
                        scope.createCourse();
                      }
                    }
                  ]
                }
              ],
              rules: {
                "CourseName": window.webix.rules.isNotEmpty
              }
            }
          ]
        }

      }).show();
    }
  }
  // Portal additional methods
  createCourse() {
    let formData = window.webix.$$("newCourseForm").getValues();
    alert(formData.CourseName);

    fetch("Course/CreateCourse?courseName=" + formData.CourseName, {
      method: 'POST', // or 'PUT'
      // body: JSON.stringify({"Email":userName,"Password":password,"returnUrl":null}), // data can be `string` or {object}!
      headers: {
        'Content-Type': 'application/json'
      },
      credentials: "include",
      mode: "cors"
    }).then(res => res.json())
      .then(response => {
        if (response.success) {
          this.setState({});
          window.webix.$$("newCourseForm").close();
        } else {
          let errors = "";
          response.error.forEach(error => {
            errors += error.description
          });

        }
        console.log(response);

      })
      .catch(error => console.error('Error:', error));


  }
  logout() {
    fetch("/Account/Logout", {
      method: 'GET', // or 'PUT'
      // body: JSON.stringify({"Email":userName,"Password":password,"returnUrl":null}), // data can be `string` or {object}!
      headers: {
        'Content-Type': 'application/json'
      },
      credentials: "include",
      mode: "cors"
    }).then(res => res.json())
      .then(response => {
        console.log('Success:', JSON.stringify(response))

        if (response.success) {


        } else {
          let errors = "";
          response.error.forEach(error => {
            console.log(error);
            errors += error.description
          });

        }

      })
      .catch(error => console.error('Error:', error));

    //
    this.handleLogin(null, null);
  }
  render() {
    /*Portal Container */
    let scope = this;
    let toolbar = function () {
      if (scope.state.role == "Administrator") {
        return "AdminToolbar";
      }
      else if (scope.state.role == "Instructor") {
        return "InstructorToolbar";
      }
      else if (scope.state.role == "Student") {
        return "StudentToolbar";
      }
      else {

      }
    }
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
        </div>
      </div>
    );
  }
}
export default Portal;

