
import React, { Component } from 'react';
import Webix from '../webix';

class CreateCourse extends Component {

	constructor(props) {
	    

  }
/*
{
          view: "button",
          gravity: 1,
          label: "Create Course",
          id: "createCourse_button",
          value: "Create Course",

          inputWidth: 100,
          click: function () {  
            scope.handleCreateCourseWindow();
          }
        }
*/

	render(){
	
		 return(<div  id="StudentToolbar">
        <Webix ui={ui} data={data}/>
      </div>
      );
	}
}
export default StudentToolbar;

