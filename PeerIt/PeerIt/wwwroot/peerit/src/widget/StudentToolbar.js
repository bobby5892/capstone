
import React, { Component } from 'react';
import Webix from '../webix';
import Courses from '../containers/courses.js';
class StudentToolbar extends Component {

	constructor(props) {
	      super(props);
	      this.state = {
          currentUser: props.currentUser,
          role: props.role,
	        data : null
	      };
  this.logout = props.logout;
  this.uploadReview = props.uploadReview;
  }
  handleCourseViewer(statechange){
      this.setState(statechange);
  }  
  renderCourses() {
       return <Courses currentUser={this.state.currentUser} role={this.state.role} handleCourseViewer={this.handleCourseViewer.bind(this)} viewingCourse={this.state.viewingCourse}/>
  }
	render(){
		let data = null;
	    let ui = 
        { 
          type: "space",
            scroll: "auto",
            height: window.innerHeight,
            width:275,
            padding: 0,
            responsive: "a1",
            rows: [
                    {
                        view:"list", 
                        data:["Student", "Reports", "Settings","upload Review","Logout"],
                        ready:function(){ 
                          this.select(this.getFirstId()); 
                        },
                        click:function(a){
                        	if(a === "Logout"){
                        		//Attempt to call the logout chain
                        		this.logout();
                        	}
                     else if(a === "Upload Review"){
            		//Attempt to call the logout chain
            		this.uploadReview();
            	}
                        }.bind(this),
                        select:true,
                        scroll:false,
                        height:200
                    },
                    {
                      view: "template",
                        scroll: true,
                        template: "right",
                        content: "Courses",
                        align:"right"
                    }
                ]
      }
		console.log("attempted render");
		 return(<div  id="StudentToolbar">
        <Webix ui={ui} data={data}/>
        {this.renderCourses()}
      </div>
      );
	}
}
export default StudentToolbar;

