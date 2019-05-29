import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import Webix from '../webix';

class ShowAssignment extends Component {
 constructor(props) {

    super(props);
    this.state = {
      currentUser: props.currentUser,
      role: props.role,
      viewingCourse: props.viewingCourse,
      viewingAssignment: props.viewingAssignment,
      downloadLink : this.buildAssignmentLink(props),
      courseName : props.viewingAssignment.fK_COURSE.name,
      assignmentName : props.viewingAssignment.name
    };
  
  }
  componentWillReceiveProps(props) {
    this.setState(props);
  }
  buildAssignmentLink(props){
  		try{
  			return  <div><a href={'/PFile/Download?pFileId='+props.viewingAssignment.pFile.id }   target='new'>Download Instructions</a><b></b></div>
  		}
  		catch(e){
  			console.log("Failed: " + e);
			return  `No Assignment Instructions Available`;
  		}
  		return null;
  }

  renderAdminInstructorList(){
  		return (
  			<div>
  			<h2>{this.state.courseName}</h2>
  			<h3>{this.state.assignmentName}</h3>
  			
  			<div id="downloadLink">{this.state.downloadLink}</div>
  					
  					

  			</div>
  			);
  		
  }
  render() {
  	let data = null;
	let ui = null;

    return (
      <div id="ShowAssignment">
      	
     {this.renderAdminInstructorList()}

      </div>
    );
  }
}
// <Webix ui={ui} data={data} />
export default ShowAssignment;