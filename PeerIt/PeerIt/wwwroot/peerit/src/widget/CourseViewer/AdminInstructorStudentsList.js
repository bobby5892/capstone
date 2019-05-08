
import React, { Component } from 'react';
import Webix from '../../webix';
import ReactDOM from 'react-dom';
class AdminInstructorStudentsList extends Component {

	constructor(props) {
	      super(props);
	      this.state = {
	        viewingCourse : props.viewingCourse,
          students : []
	      };

    this.loadStudents();
    window.webix.protoUI({
      name:"react",
      defaults:{
        borderless:true
      },
      $init:function(config){
        this.$ready.push(function(){    
          ReactDOM.render(
            this.config.app,
            this.$view
          );
        });
      }
    }, window.webix.ui.view)
	 this.loadStudents();  
  }
  loadStudents(){
      fetch("Course/GetCourses", {
        method: 'GET', // or 'PUT'
        headers: {
          'Content-Type': 'application/json'
        },
        credentials: "include",
        mode: "no-cors"
      }).then(res => res.json())
        .then(response => {
          if (response.success) {
            this.setState({courses: response.data});
          //  console.log("response.data: " + JSON.stringify(response.data));
            
            
          } 
        })
        .catch(error => console.error('Error:', error));
  }
  componentWillReceiveProps(props){
  		this.setState({viewingCourse : props.viewingCourse});


  		// Trigger Webix to Redraw the component
  		//window.webix.$$().setHTML("<h1>YEP</h1>");
  } 
	render(){
		//console.log("AdminInstructorStudentList Loading " + this.state.viewingCourse);
		let data = null;
		let ui = {
  view:"list",
  width:250,
  autoheight:true,
  template:"#title#",
  select:true,
  data:[
    { id:1, title:"Item 1"},
    { id:2, title:"Item 2"},
    { id:3, title:"Item 3"},
    { id:4, title:"Item 4"},
    { id:5, title:"Item 5"},
    { id:6, title:"Item 6"}
  ]
};
	
let courseID = (this.state.viewingCourse !== null) ? this.state.viewingCourse : "";


 	 return(<div id="AdminInstructorStudentsList">
        <Webix ui={ui} data={data}/>
      </div>
      );
	}
	componentDidMount() {
		//console.log("after load");
	
  	}
}
export default AdminInstructorStudentsList;

