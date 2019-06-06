
import React, { Component } from 'react';
import Webix from '../webix';
import Courses from '../containers/courses.js';
import CreateCourse from '../widget/CreateCourse.js';
class AdminToolbar extends Component {

	constructor(props) {
		super(props);
		this.state = {
			currentUser: props.currentUser,
			role: props.role,
			viewingCourse: props.viewingCourse,
			viewingAssignment:props.viewingAssignment,
			data: null,
			seed : props.seed
			
		};
		this.logout = props.logout;
		// Receive the function handle for handleManageUsersMenuClick
		this.handleMenuClick = props.handleMenuClick;
		this.showCreateCourse = props.handleCreateCourse;
	    this.accountClick = props.accountClick;
	    this.handleCourseViewer = props.handleCourseViewer;
	    this.redrawAll = props.redrawAll;
	}
    componentWillReceiveProps(props) {
    	this.setState(props);
    }
	renderCourses() {
       return <Courses 
       currentUser={this.state.currentUser} 
       role={this.state.role}  
       handleCourseViewer={this.handleCourseViewer.bind(this)} 
       viewingCourse={this.state.viewingCourse}
       handleMenuClick={this.handleMenuClick}
       accountClick={this.accountClick.bind(this)}
       redrawAll={this.redrawAll} seed={this.state.seed}
       viewingAssignment={this.state.viewingAssignment}
       />
	}
	renderCreateCourseButton() {
       return <CreateCourse currentUser={this.state.currentUser} role={this.state.role} showCreateCourse={this.showCreateCourse}
       redrawAll={this.redrawAll} seed={this.state.seed} />
	}			
	render() {
		//let scope = this;
		let data = null;
		let ui =
		{
			type: "space",
            scroll: "false",
            height: window.innerHeight,
            autowidth:true,
            padding: 0,
            responsive: "a1",
			rows: [
					{
						view: "list",
						gravity: 1,
						padding:0,
						scroll: false,
						margin:0,
						height:225,
						data: ["Admin", "My Account", "Manage Users", "Settings", "Logout"],
						ready: function () {
							// Highlight the first one
							this.select(this.getFirstId());
						},
						click: function (a) {
							if (a === "Logout") {
								//Attempt to call the logout chain
								this.logout();
							}
							else if (a === "Manage Users") {
								this.handleMenuClick("ManageUsers");
							}
							else if (a === "Manage Courses") {
								this.handleMenuClick("ManageCourses");
							}
							else if (a === "Settings") {
								if (window.webix.$$("AdminSettings") == null) {
									this.handleMenuClick("AdminSettings");	
								}
							}
							else if (a === "Admin") {
								this.handleMenuClick("LiveFeed");
							}
				            else if( a === "My Account"){
				            	if (window.webix.$$("accountWindow") == null) {
									this.accountClick();
				            	}
				            }    

						}.bind(this),
						select: true,
					},
                	{
						gravity: 1,
					 	view: "template",
                 		scroll: false,
                 		template: "right",
                 		content: "CreateCourse",
                 		autoheight:true,
                 		align:"right"
                	},
				    {
					 	view: "template",
	             		template: "right",
	             		content: "Courses",
	             		height: 300,
	             		scroll: true,
	             		align:"right"
                	}
			]
		}

		return (
			<div id="AdminToolbar" >
				{this.renderCreateCourseButton()}
				{this.renderCourses()}
				<Webix ui={ui} data={data} />
			</div>
		);
	}
}
export default AdminToolbar;

