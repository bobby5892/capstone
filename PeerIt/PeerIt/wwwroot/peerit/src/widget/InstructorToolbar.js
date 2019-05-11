
import React, { Component } from 'react';
import Webix from '../webix';
import Courses from '../containers/courses.js';
import CreateCourse from '../widget/CreateCourse.js';

class InstructorToolbar extends Component {

  constructor(props) {
    super(props);
    this.state = {
      viewingCourse: null,
      currentUser: props.currentUser,
      role: props.role,
      data: null
    };
    this.logout = props.logout;
    this.handleCreateCourseWindow = props.handleCreateCourse;
    this.handleMenuClick = props.handleMenuClick;
    this.showCreateCourse = props.handleCreateCourse;

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
    />
  }
  renderCreateCourseButton() {
    return <CreateCourse 
    currentUser={this.state.currentUser} 
    role={this.state.role} 
    showCreateCourse={this.showCreateCourse} />
  }

  render() {
    //let scope = this;
    let data = null;
    let ui =
    {
      type: "space",
      padding: 0,
      //responsive: "a1",
      height: window.innerHeight,
      //width: window.innerWidth,
      rows:
        [
          {
            view: "list",
            data: ["Instructor", "Reports", "Settings", "Logout"],
            ready: function () {
              this.select(this.getFirstId());
            },
            click: function (a) {
              if (a === "Logout") {
                //Attempt to call the logout chain
                this.logout();
              }
            }.bind(this),
            select: true,
            scroll: false,
            //width: 350,
            height: 150
          },
          // This is where we would render courses        
          {

            view: "template",
            scroll: true,
            template: "right",
            content: "Courses",
            align: "right"
          },
          {
            gravity: 1,
            view: "template",
            scroll: true,
            template: "right",
            content: "CreateCourse",
            align: "right"
          }
        ]
    };


    console.log("attempted render");
    return (<div id="InstructorToolbar" >
      {this.renderCreateCourseButton()}
      {this.renderCourses()}
      <Webix ui={ui} data={data} />
    </div>
    );
  }
}
export default InstructorToolbar;

