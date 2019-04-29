
import React, { Component } from 'react';
import Webix from '../webix';

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
		this.handleCreateCourseWindow = props.handleCreateCourse;
		console.log("Constructor of Admin Toolbar" + JSON.stringify(this));
	}
	// Click up in Portal 
	builtCourseAccord() {
		console.log('made it to course accord');

		return ({
			view: "accordionitem",
			header: "Pane 2",
			padding: 0,
			//headerAlt:"Pane 2 Closed",
			body: "This is Pane 2 body", //just text
			collapsed: true
		});

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
					data: ["Admin", "Manage Courses", "Manage Users", "Settings", "Logout"],
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
					//scroll: false,
					//width: 200
				}, {
					view: "accordion",
					scroll: "y",
					multi: true,

					rows: [
						scope.builtCourseAccord()]
						// {
						// 	view: "accordionitem",
						// 	header: "Pane 2",
						// 	padding: 0,
						// 	//headerAlt:"Pane 2 Closed",
						// 	body: "This is Pane 2 body", //just text
						// 	collapsed: true
						// }]
				},
				
				{
					view: "button",
					gravity: 1,
					label: "Create Course",
					id: "createCourse_button",
					value: "Create Course",

					inputWidth: 100,
					click: function () {
						//console.log("From the EventHandler in AdminToolbar: " + this);
						scope.handleCreateCourseWindow();

						// Lets only allow once

					}
				}



			]
		}
		// let ui = 





		return (
			<div id="AdminToolbar">
				<Webix ui={ui} data={data} />
			</div>
		);
	}
}
export default AdminToolbar;

