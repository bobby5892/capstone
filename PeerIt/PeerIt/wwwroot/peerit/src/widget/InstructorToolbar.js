
import React, { Component } from 'react';
import Webix from '../webix';
import Courses from '../containers/courses.js';
import CreateCourse from '../widget/CreateCourse.js';

class InstructorToolbar extends Component {

  constructor(props) {
    super(props);
    this.state = {
      viewingCourse: props.viewingCourse,
      currentUser: props.currentUser,
      viewingAssignment:props.viewingAssignment,
      role: props.role,
      data: null,
      seed : props.seed
    };
    this.logout = props.logout;
    this.handleCreateCourseWindow = props.handleCreateCourse;
    this.handleMenuClick = props.handleMenuClick;
    this.showCreateCourse = props.handleCreateCourse;
    this.accountClick = props.accountClick;
    this.handleCourseViewer = props.handleCourseViewer;
    this.redrawAll = props.redrawAll;
  }
  componentWillReceiveProps(props) {
      this.setState(props);
  }
  renderCourses() {
    return <Courses
      currentUser={this.state.currentUser}
      role={this.state.role}
      handleCourseViewer={this.handleCourseViewer}
      viewingCourse={this.state.viewingCourse}
      handleMenuClick={this.handleMenuClick}
      accountClick={this.accountClick.bind(this)}
      redrawAll={this.redrawAll} seed={this.state.seed}
      viewingAssignment={this.state.viewingAssignment}
    />
  }
  renderCreateCourseButton() {
    return <CreateCourse 
    currentUser={this.state.currentUser} 
    role={this.state.role} 
    showCreateCourse={this.showCreateCourse} 
    accountClick={this.accountClick.bind(this)}
    redrawAll={this.redrawAll} seed={this.state.seed}
    />
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
      width: 350,
      rows:
        [
          {
            view: "list",
            data: ["Instructor", "My Account", "Logout"],
            ready: function () {
              this.select(this.getFirstId());
            },
            click: function (a) {
              if(a == "My Account"){
                this.accountClick();
              }
              else if (a === "Logout") {
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
    return (<div id="InstructorToolbar" >
      {this.renderCreateCourseButton()}
      {this.renderCourses()}
      <Webix ui={ui} data={data} />
    </div>
    );
  }
}
export default InstructorToolbar;

