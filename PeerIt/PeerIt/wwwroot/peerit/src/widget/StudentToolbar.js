
import React, { Component } from 'react';
import Webix from '../webix';
import Courses from '../containers/courses.js';
class StudentToolbar extends Component {


	constructor(props) {
	      super(props);
	      this.state = {
          currentUser: props.currentUser,
          role: props.role,
	        data : null,
          seed : props.seed
	      };
    this.logout = props.logout;
    this.handleMenuClick = props.handleMenuClick;
    this.renderAccountWindow = props.renderAccountWindow;
    this.uploadReview = props.uploadReview;
    this.accountClick = props.accountClick;
    this.redrawAll = props.redrawAll;
    this.renderUploadStudentAssignmentWindow.bind(this);
  }
  componentWillReceiveProps(props) {
      this.setState(props);
  }
  handleCourseViewer(statechange) {
    this.setState(statechange);
  }
  renderCourses() {
    return <Courses
      currentUser={this.state.currentUser}
      role={this.state.role}
      handleCourseViewer={this.handleCourseViewer.bind(this)}
      viewingCourse={this.state.viewingCourse}
      handleMenuClick={this.handleMenuClick}
      accountClick={this.accountClick.bind(this)}
      redrawAll={this.redrawAll} seed={this.state.seed}
    />
  }
  render() {
    let data = null;
    let ui =
    {
      type: "space",
      scroll: "auto",
      height: window.innerHeight,
      width: 350,
      padding: 0,
      responsive: "a1",
      rows: [
        {
          view: "list",
          data: ["Student", "My Account","Upload an Assignment", "Logout"],
          ready: function () {
            this.select(this.getFirstId());
          },
          click: function (a) {
            if( a === "My Account"){
              this.accountClick();
            }
            else if( a === "Upload an Assignment"){
              this.renderUploadStudentAssignmentWindow();
            }
            else if (a === "Logout") {
              //Attempt to call the logout chain
              this.logout();
            }
          }.bind(this),
          select: true,
          scroll: false,
          height: 200
        },
        {
          view: "template",
          scroll: true,
          template: "right",
          content: "Courses",
          align: "right"
        }
      ]
    }
    
    return (<div id="StudentToolbar">
      <Webix ui={ui} data={data} />
      {this.renderCourses()}
    </div>
    );
  }
  renderUploadStudentAssignmentWindow() {
    let scope = this;

    var newWindow = window.webix.ui({
      view: "window",
      id: "uploadStudentAssignmentWindow",
      width:600,
      //height: 600,
      move: true,
      position: "center",
      head: {
        type: "space",
        cols: [
          { view: "label", label: "Upload a student Assignment" },
          {
            view: "button", label: "Close", 
            width: 70, 
            left: 250,
            click: function () {
              //scope.setState({"editUser" : null });
              window.webix.$$("uploadStudentAssignmentWindow").close();
            }
          }
        ]
      },
      body: {
        type: "space",
        rows: [
          {
            view: "form",
            id: "uploadStudentAssignmentForm",
            elements: [
              { view: "label", label: "Upload your student assignment here: ", name: "", labelWidth: "auto", value: "" },
              {  
                view: "uploader", inputName: "files", upload: "/StudentAssignment/UploadStudentAssignment", 
                id: "studentAssignmentFile", link: "mylist", value: "Upload File", autosend: false
              },
              {
                view: "list", id: "mylist", type: "uploader",
                autoheight: true, borderless: true
              },  
              //{ view: "text", label: "Assignment Name", name: "Assignment_Name",labelWidth: 200,invalidMessage:"Please enter Assignment Name" },
              {
                view: "button",value:"Upload", type:"form", 
                click: function () {
                  let validResponse = window.webix.$$("uploadStudentAssignmentForm").validate();
                  let FormVal = window.webix.$$("uploadStudentAssignmentForm").getValues();
                  window.webix.$$("studentAssignmentFile").define({
                    urlData:{courseAssignmentId:33}
                  });
                  window.webix.$$("studentAssignmentFile").send(function(response) {
                    if (response != null){
                      window.webix.message("Succsess");
                        window.webix.$$("uploadAssignmentWindow").close();
                    }
                    else {
                    alert("Nothing to Submit");
                  }})
                }
              }
            ],
            rules: {
              //No rules defined yet!!!
            }
          }
        ]
      }
    }).show();
    window.webix.$$("uploadStudentAssignmentForm").setValues(
         { Course:this.state.viewingCourse}
     );
  }
}
export default StudentToolbar;

