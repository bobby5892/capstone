import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import Webix from '../webix';

class CourseContent extends Component {
  constructor(props) {
    super(props);
    this.state = {
      currentUser: props.currentUser,
      role: props.role,
      viewingCourse: props.viewingCourse,
      addingStudent: false
    }
    this.handleCourseViewer = props.handleCourseViewer;
    this.handleMenuClick = props.handleMenuClick;
    window.webix.protoUI({
      name: "react",
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
  addStudentWindow() {
    window.webix.ui({
      view: "window",
      id: "addStudentWindow",
      width: 500,
      height: 500,
      move: true,
      position: "center",
      head: {
        type: "space",
        cols: [
          { view: "label", label: "Add Student to Course" },
          {
            view: "button", id: "addStudtentButton", label: "Close", width: 70, left: 250,
            click: function () {
              this.setState({ addingStudent: false });
              window.webix.$$("addStudentWindow").close();
            }.bind(this)
          }
        ]
      },
      body: {
        type: "space",
        rows: [
          {
            view: "form",
            id: "addStudentForm",
            width: 400,
            elements: [
              { view: "text", label: "Email", name: "Email", labelWidth: 100, invalidMessage: "Must be valid email address" },
              {
                margin: 5,
                cols: [
                  {
                    view: "button",
                    value: "Add Student",
                    type: "form",
                    id: "AddStudentFormButton"
                  }
                ]
              }
            ],
            rules: {
              "Email": window.webix.rules.isEmail
            }
          }
        ]
      }

    }).show();

    console.log("Does button have click: " + window.webix.$$("AddStudentFormButton").hasEvent("onItemClick"));
    // If its not already bound lets do it
    if (window.webix.$$("AddStudentFormButton").hasEvent("onItemClick")=== false) {
      console.log("binding function to button");
      window.webix.$$("AddStudentFormButton").attachEvent("onItemClick", function (i) {

        let validResponse = window.webix.$$("addStudentForm").validate();
        if (validResponse) {
            let email = window.webix.$$("addStudentForm").elements["Email"].config.value;
            this.addStudent(email,this.state.viewingCourse);
        }
      }.bind(this));
    }

  }
  addStudent(email,courseID) {
        fetch("/Course/AddStudentToCourse?courseID=" + courseID + "&studentEmail=" + email, {
          method: 'POST', // or 'PUT'
          headers:{
            'Content-Type': 'application/json'
          },
          credentials: "include",
          mode:"no-cors"
        }).then(res => res.json())
        .then(response => {
          if(response.success){
            // Should reload student list
            console.log("Added Student");
            window.webix.$$("addStudentWindow").close();
            this.handleMenuClick("LiveFeed");

          }
        })
        .catch(error => console.error('Error:', error));

  }
  componentWillReceiveProps(props) {
    this.setState(props);
  }
  removeStudent(studentID){
     fetch("/Course/RemoveStudentToCourse?courseID=" + this.state.viewingCourse + "&studentID=" + studentID, {
          method: 'DELETE', // or 'PUT'
          headers:{
            'Content-Type': 'application/json'
          },
          credentials: "include"
          
        }).then(res => res.json())
        .then(response => {
          if(response.success){
            // Should reload student list
            this.handleMenuClick("LiveFeed");
          }
        })
        .catch(error => console.error('Error:', error));
  }
  render() {
    console.log("render course content");
    let ui = {
      rows: [
        {
          header: "Course Settings (ID: " + this.state.viewingCourse + ")", body: " "
        },
        {
          view: "tabview",
          autoheight: true,
          header: "Course",
          cells: [
            {
              autoheight: true,
              header: "Assignments",
              body: {
                autoheight: true,
                rows: [
                  {
                    autoheight: true,
                    view: "datatable",
                    columns: [
                      { id: "rank", header: "", width: 50 },
                      { id: "firstName", header: "First Name", width: 200 },
                      { id: "lastName", header: "Last Name", width: 80 },

                    ],
                    url: "/Course/GetStudents?courseID=" + this.state.viewingCourse
                  }
                ]
              }
            },
            {
              header: "Groups",
              body: {
                autoheight: true,
                rows: [
                  {
                    autoheight: true,
                    view: "datatable",
                    columns: [
                      { id: "rank", header: "", width: 50 },
                      { id: "firstName", header: "First Name", width: 200 },
                      { id: "lastName", header: "Last Name", width: 80 },

                    ],
                    url: "/Course/GetStudents?courseID=" + this.state.viewingCourse

                    /* data: [
                         { id:1, title:"The Shawshank Redemption", year:1994, votes:678790, rank:1},
                         { id:2, title:"The Godfather", year:1972, votes:511495, rank:2}
                     ]*/
                  }
                ]
              }
            },
            {
              header: "Students",
              body: {
                autoheight: true,
                rows: [
                  {
                    view: "button",
                    value: "Add Student",
                    css: "webix_primary",
                    inputWidth: 100,
                    on: {
                      'onItemClick': function (i) {
                        if (!this.state.addingStudent) {
                          this.setState({ addingStudent: true });
                          this.addStudentWindow();
                        }

                      }.bind(this)
                    }
                  },
                  {
                    autoheight: true,
                    view: "datatable",
                    columns: [
                      { id: "rank", header: "", width: 50 },
                      { id: "firstName", header: "First Name", width: 200 },
                      { id: "lastName", header: "Last Name", width: 80 },
                      { header: "Enrollment", width: 100 ,   template:"<input type='button' value='Remove' class='remove_button'>"}
                        
                      
                    ],
                    onClick : {
                      remove_button:function (i,ev){
                             // Don't remove the comment below - its actually functional.
                             //eslint-disable-next-line
                             let confirmCheck = confirm("Are you sure you want to remove this student?");
                             if(confirmCheck){
                                console.log("I want to remove " + ev.row);
                                this.removeStudent(ev.row);
                             }
                          }.bind(this)
                    },
                    url: "/Course/GetStudents?courseID=" + this.state.viewingCourse
                  }
                ]
              }
            }
          ]
        }
      ]
    };
    let data = null;

    return (
      <div id="CourseContent">
        <Webix ui={ui} data={data} />

      </div>
    );
  }
}
export default CourseContent;