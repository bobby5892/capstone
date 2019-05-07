
import React, { Component } from 'react';
import Webix from '../../webix';
import ReactDOM from 'react-dom';
class AdminInstructorStudentsList extends Component {

	constructor(props) {
	      super(props);
	      this.state = {
	        viewingCourse : props.viewingCourse
	      };


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
	     
  }
  componentWillReceiveProps(props){
  		this.setState({viewingCourse : props.viewingCourse});


  		// Trigger Webix to Redraw the component
  		//window.webix.$$().setHTML("<h1>YEP</h1>");
  } 
	render(){
		console.log("AdminInstructorStudentList Loading " + this.state.viewingCourse);
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
    { id:3, title:"Item 3"}
  ]
};
	
let courseID = (this.state.viewingCourse !== null) ? this.state.viewingCourse : "";


 	 return(<div id="AdminInstructorStudentsList">
    thing
        <Webix ui={ui} data={data}/>
      </div>
      );
	}
	componentDidMount() {
		console.log("after load");
	
  	}
}
export default AdminInstructorStudentsList;

