
import React, { Component } from 'react';
import Webix from '../webix';

class CreateCourse extends Component {

	constructor(props) {
   super(props);
    this.state = {
      currentUser: props.currentUser,
      role: props.role,
      data: null
      
    };
    console.log("ShowCreatecourse: " + props.showCreateCourse);
    this.showCreateCourse = props.showCreateCourse;

  }
	render(){
    let scope = this;
    let ui = {
        view: "button",
        gravity: 1,
        label: "Create Course",
        id: "createCourse_button",
        value: "Create Course",

        inputWidth: 100,
        click: function () {  
         scope.ShowCreateCourse();
        }
      };
    let data = null;
	
		 return(<div  id="CreateCourse">
        <Webix ui={ui} data={data}/>
      </div>
      );
	}
}
export default CreateCourse;

