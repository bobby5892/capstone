
import React, { Component } from 'react';
import Webix from '../webix';

class InstructorToolbar extends Component {

	constructor(props) {
	      super(props);
	      this.state = {
	        data : null
	      };
  this.logout = props.logout;
  this.renderAccountWindow = props.renderAccountWindow;

  }

	render(){
		let data = null;
	    let ui = 
		{ 
            view:"list", 
            data:["Instructor", "Reports", "Settings","Logout"],
            ready:function(){ 
              this.select(this.getFirstId()); 
            },
            click:function(a){
            	if(a === "Logout"){
            		//Attempt to call the logout chain
            		this.logout();
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

