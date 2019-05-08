
import React, { Component } from 'react';
import Webix from '../../webix';

class AdminInstructorSettings extends Component {

	constructor(props) {
	      super(props);
	      this.state = {
	      	viewingCourse : props.viewingCourse,
	        data : null
	      };
	      this.handleMenuClick = props.handleMenuClick;

	    
  }
    componentWillReceiveProps(props){
  		this.setState(props);
   }  

	render(){
		let data = null;
		let ui = window.webix.ui({
		    view:"button", 
		    value:"More", 
		    css:"webix_primary", 
		    inputWidth:100,
		    on:{
		    	'onItemClick' : function(i){
		    		console.log("button was clicked");
		    		this.handleMenuClick("CourseContent");
		    	}.bind(this)
		    }
		});
		//let courseID = (this.state.viewingCourse !== null) ? this.state.viewingCourse : "";
		return(<div  id="AdminInstructorSettings">
			  <Webix ui={ui} data={data}/>
        
      	</div>
      	);
	}
}
export default AdminInstructorSettings;

