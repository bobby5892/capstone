
import React, { Component } from 'react';
import Webix from '../webix';
import Courses from '../containers/courses.js';
class StudentToolbar extends Component {


	constructor(props) {
	      super(props);
	      this.state = {
          currentUser: props.currentUser,
          role: props.role,
          viewingCourse: props.viewingCourse,
          viewingAssignment:props.viewingAssignment,
	        data : null,
          seed : props.seed,
          courseGroup : null
	      };
    this.logout = props.logout;
    this.handleMenuClick = props.handleMenuClick;
    this.renderAccountWindow = props.renderAccountWindow;
    this.uploadReview = props.uploadReview;
    this.accountClick = props.accountClick;
    this.redrawAll = props.redrawAll;
    this.handleCourseViewer = props.handleCourseViewer;
    // Load course group
    console.log("COnstruct Student: " + props.viewingCourse);
    
  }
  componentWillReceiveProps(props) {

      this.setState(props);

      this.getCourseGroup(props);
   
  }
  getCourseGroup(props){
   // console.log("trying to get course group:" +JSON.stringify( props));
     fetch("/StudentAssignment/GetCourseGroup?courseId=" + props.viewingCourse, {
        method: 'GET', // or 'PUT'
        headers: {
          'Content-Type': 'application/json'
        },
        credentials: "include",
        mode: "no-cors"
      }).then(res => res.json()).then(response => {
          if (response.success) {
            console.log("GROUP:" + JSON.stringify(response.data[0].reviewGroup));
            this.setState({courseGroup:response.data[0].reviewGroup});
          }
          
        })
        .catch(error => console.error('Error:', error));
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
          data: ["Student", "My Account", "Logout"],
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

