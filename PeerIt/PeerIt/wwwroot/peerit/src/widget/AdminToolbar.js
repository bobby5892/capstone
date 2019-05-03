
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
			data: null
			
		};
		this.logout = props.logout;
		// Receive the function handle for handleManageUsersMenuClick
		this.handleMenuClick = props.handleMenuClick;
		this.showCreateCourse = props.handleCreateCourse;
	}
	renderCourses() {
       return <Courses currentUser={this.state.currentUser} role={this.state.role} />
	}
	renderCreateCourseButton() {
       return <CreateCourse currentUser={this.state.currentUser} role={this.state.role} showCreateCourse={this.showCreateCourse} />
	}			
	render() {
		let scope = this;
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
						view: "list",
						gravity: 1,
						padding:0,
						scroll: false,
						margin:0,
						//scroll:"y",
						data: ["Admin",  "Manage Users", "Settings", "Logout"],
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
								this.handleMenuClick("AdminSettings");
							}
							else if (a === "Admin") {
								this.handleMenuClick("LiveFeed");
							}

						}.bind(this),
						select: true,
					},
				
				
                	{
						
					 	view: "template",
                 		scroll: true,
                 		template: "right",
                 		content: "Courses",
                 		align:"right"
                	},
                	{
						gravity: 1,
					 	view: "template",
                 		scroll: true,
                 		template: "right",
                 		content: "CreateCourse",
                 		align:"right"
                	},
				
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

