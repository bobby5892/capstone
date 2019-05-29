import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import Webix from '../webix';

class ShowStudentAssignment extends Component {
 constructor(props) {
    super(props);
    this.state = {
      data: null,
      currentUser: props.currentUser,
      role: props.role,
      viewingCourse: props.viewingCourse,
      viewingAssignment: props.viewingAssignment,
      courseName : props.viewingAssignment.fK_COURSE.name,
      assignmentName : props.viewingAssignment.name
    };
    this.fetchStudentAssignmentInfoForCourseAndUser.bind(this);
  }
  componentWillReceiveProps(props) {
    this.setState(props);
  }
  fetchStudentAssignmentInfoForCourseAndUser(props){
    fetch("/StudentAssignment/GetAssignmentsByCourseId?courseID="+this.state.viewingCourse,{
      method: 'GET',
      headers:{'Content-Type': 'application/json'},
      credentials: "include",
      mode:"no-cors"
    }).then(response => response.json())
      .then(response => {
        if(response.success){
          let data = {
            "StudentAssignment" : response.Data[0],
          }
          
          if(response.Data.length > 0) { this.setState({"data":response.Data}); }
          else { alert("no data"); }
          console.log("STUDENT ASSIGNMENT = "+this.state.StudentAssignment);
          return true;
        }
        else
        {
          console.log("response was not a success");
        }})
      .then(this.setState()).catch(console.log("error fetching assignment"));
  }
  renderStudentAssignmentInfoWindow(){
    this.fetchStudentAssignmentInfoForCourseAndUser();
    window.webix.ui({
      view:"window",
      id:"studentAssInfoWindow",
      width: 900,
      height: 600,
      move:true,
      position:"center",
      head:{
          type:"space",
          cols:[
              {view:"label", label: "Student Assignment Information" },
              {view:"label", label: "Assignment: " },
              {
                view:"button", label:"Close", width:70,left:250,
                click:function(){
                  this.setState({"assignmentInfo" : null });
                  window.webix.$$("studentAssInfoWindow").close();
                }.bind(this)
              }
           ]   
      },
      body:{
          type:"space",
          rows:
          [
            { 
              view:"form", 
              id:"studentAssInfoForm",
              width:900,
              elements:
              [
                { view:"label", label:"Student Assignment Information", name:"headerLabel", labelWidth:100, },
              ],
            }
          ]
      }
}).show();
  }
  render() {
  // 	let data = null;
	// let ui = null;
// {this.renderStudentAssignmentInfoWindow()}
    return (
      <div id="ShowStudentAssignment">
      	show student assignment
    

      </div>
    );
  }
}
// <Webix ui={ui} data={data} />
export default ShowStudentAssignment;