
import React, { Component } from 'react';
import Webix from '../webix';
import Courses from '../containers/courses.js';
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
	}
	renderCourses() {
       return <Courses currentUser={this.state.currentUser} role={this.state.role} />
	}	
	render() {
		let scope = this;
		let data = null;
		let ui =
		{
			type: "space",
			padding: 0,
			gravity: 1,
			rows: [
					{
						view: "list",
						gravity: 1,
						height: 175,
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
					}
			]
		}

		return (
			<div id="AdminToolbar" >
				<Webix ui={ui} data={data} />
				{this.renderCourses()}
			</div>
		);
	}
}
export default AdminToolbar;

