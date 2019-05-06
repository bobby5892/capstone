
import React, { Component } from 'react';
import Webix from '../webix';

class AdminToolbar extends Component {

	constructor(props) {
	      super(props);
	      this.state = {
	      	currentUser : props.currentUser,
	      	role : props.role,
	        data : null
	      };
	      this.logout = props.logout;
	      // Receive the function handle for handleManageUsersMenuClick
	      this.handleMenuClick = props.handleMenuClick;
    }
    // Click up in Portal 
    
	render(){
		let data = null;
	    let ui = 
	    {
            view:"list", 
            data:["Admin", "Manage Courses","Manage Users", "Settings","Logout"],
            ready:function(){ 
              // Highlight the first one
              this.select(this.getFirstId()); 
            },
            click:function(a){
            	if(a === "Logout"){
            		//Attempt to call the logout chain
            		this.logout();
            	}
            	else if( a === "Manage Users"){
            		this.handleMenuClick("ManageUsers");
            	}
            	else if( a === "Manage Courses"){
            		this.handleMenuClick("ManageCourses");
            	}
            	else if( a === "Settings"){
            		this.handleMenuClick("AdminSettings");
            	}
            	else if( a === "Admin"){
								this.handleMenuClick("LiveFeed");
							}
							    	

            }.bind(this),

            select:true,
            scroll:false,
            width:200 
         };
		
		return(
		 	<div id="AdminToolbar">
        		<Webix ui={ui} data={data}/>
      		</div>
      	);
	}
}
export default AdminToolbar;

