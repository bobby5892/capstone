
import React, { Component } from 'react';
import Webix from '../webix';
import Courses from '../containers/courses.js';
class StudentToolbar extends Component {


	constructor(props) {
	      super(props);
	      this.state = {
          currentUser: props.currentUser,
          role: props.role,
          viewingAssignment:props.viewingAssignment,
	        data : null,
          seed : props.seed
	      };
    this.logout = props.logout;
    this.handleMenuClick = props.handleMenuClick;
    this.renderAccountWindow = props.renderAccountWindow;
    this.uploadReview = props.uploadReview;
    this.accountClick = props.accountClick;
    this.redrawAll = props.redrawAll;
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
       viewingAssignment={this.state.viewingAssignment}
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
}
export default StudentToolbar;

