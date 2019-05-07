
import React, { Component } from 'react';
import Webix from '../webix';

class InstructorToolbar extends Component {

	constructor(props) {
	      super(props);
	      this.state = {
	        data : null
        };
  
  this.logout = props.logout;
  
  this.renderUploadAssignmentWindow = props.renderUploadAssignmentWindow;

  }
  renderInPortal(){
    this.renderUploadAssignmentWindow();
  }
	render(){
		let data = null;
	    let ui = 
		{ 
            view:"list", 
            data:["Instructor", "Reports", "Settings","Upload Assignment","Logout"],
            ready:function(){ 
              this.select(this.getFirstId()); 
            },
            click:function(a){
            	if(a === "Logout"){
            		//Attempt to call the logout chain
            		this.logout();
            }
            else if (a === "Upload Assignment"){
              console.log("state: " + JSON.stringify(this.state));
              this.renderInPortal();
            }
            }.bind(this),
            select:true,
            scroll:false,
            width:200 
         };
		console.log("attempted render");
		 return(<div  id="InstructorToolbar">
        <Webix ui={ui} data={data}/>
      </div>
      );
	}
}
export default InstructorToolbar;

