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
      viewingAssignment: props.viewingAssignment
    };
  }
  componentWillReceiveProps(props) {
    this.setState(props);
  }
  renderAdminInstructorList(){
  		return (
  			<div>
  			<h2>Test2</h2>
  			<div>{this.state.viewingCourse}</div>
  					<div>{JSON.stringify(this.state.viewingAssignment)}</div>
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