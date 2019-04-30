
import React, { Component } from 'react';
import Webix from '../webix';

class AdminToolbar extends Component {

	constructor(props) {
		super(props);
		this.state = {
			currentUser: props.currentUser,
			role: props.role,
			data: null,
			Courses:null
		};
		this.logout = props.logout;
		// Receive the function handle for handleManageUsersMenuClick
		this.handleMenuClick = props.handleMenuClick;
		this.handleCreateCourseWindow = props.handleCreateCourse;
		console.log("Constructor of Admin Toolbar" + JSON.stringify(this));
		
		this.loadCourses();
	}
	// Click up in Portal 
	loadCourses() {

		console.log('made it to course accord' + JSON.stringify("Scope of loadCourse" + JSON.stringify(this)));
		let accord = [];
		fetch("Course/GetCourses", {
			method: 'GET', // or 'PUT'
			headers: {
				'Content-Type': 'application/json'
			},
			credentials: "include",
			mode: "cors"
		}).then(res => res.json())
			.then(response => {
				if (response.success) {
					//data = response.data;
					console.log(response.data)
					response.data.forEach(element => {
						console.log("Adding Element: " + element.name);

						accord.push({
							view: "accordionitem",
							header: element.name,
							id: element.id,
							padding: 0,
							//headerAlt:"Pane 2 Closed",
							body: "This is " + element.name, //just text
							collapsed: true
						});
					})
					console.log(accord);	
					 this.setState({'Courses': accord});
					 console.log("State after Fetch: " + JSON.stringify(this.state));
					// window.webix.$$("newCourseForm").close();
				} else {
					let errors = "";
					response.error.forEach(error => {
						errors += error.description
					});
				}
			})
			.catch(error => console.error('Error:', error));
		//return ({
			// view: "accordionitem",
			// header: data,
			// padding: 0,
			// //headerAlt:"Pane 2 Closed",
			// body: "This is Pane 2 body", //just text
			// collapsed: true
		//});
	}
	renderCourses(){
		//let scope = this;
	/*	if(this.state.Courses === null){
			console.log("RenderCourses: state was empty");
			console.log("State Courses"+ JSON.stringify(this.state.Courses));
			return([{
			view: "accordionitem",
			header: "No Courses",
			padding: 0,
			//headerAlt:"Pane 2 Closed",
			body: "This is Pane 2 body", //just text
			collapsed: true}])
		}
		else {
			/*var accorditem =[];
			forEach(item in this.state.Courses){
				accorditem.push(item);
			};
			console.log("state was good");
			console.log("State after Set: " + JSON.stringify(this.state));
			//return JSON.stringify(this.state.Courses );
			return this.state.Courses;
			
	}*/


		if(this.state.Courses != null){
			console.log("attempt to render");
			console.log(this.state.Courses);
			return this.state.Courses
			;
		}
		else{
			return([{
				view: "accordionitem",
				header: "No Courses",
				padding: 0,
				//headerAlt:"Pane 2 Closed",
				body: "This is Pane 2 body", //just text
				collapsed: true}]);
		}
	}
	render() {
		console.log("RENDER");
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
					//height:200,
					//width:150,
					rows: 
					this.renderCourses()

					//{\"view\":\"accordionitem\",\"header\":\"140u\",\"id\":2,\"padding\":0,\"body\":\"This is 140u\",\"collapsed\":true},{\"view\":\"accordionitem\",\"header\":\"140u\",\"id\":3,\"padding\":0,\"body\":\"This is 140u\",\"collapsed\":true},{\"view\":\"accordionitem\",\"header\":\"140u\",\"id\":4,\"padding\":0,\"body\":\"This is 140u\",\"collapsed\":true},{\"view\":\"accordionitem\",\"header\":\"jkgu\",\"id\":5,\"padding\":0,\"body\":\"This is jkgu\",\"collapsed\":true},{\"view\":\"accordionitem\",\"header\":\"html\",\"id\":6,\"padding\":0,\"body\":\"This is html\",\"collapsed\":true},{\"view\":\"accordionitem\",\"header\":\"Html\",\"id\":7,\"padding\":0,\"body\":\"This is Html\",\"collapsed\":true}]"},{"view":"button","gravity":1,"label":"Create Course","id":"createCourse_button","value":"Create Course","inputWidth":100}]
						//[{"view":"accordionitem","header":"140u","id":2,"padding":0,"body":"This is 140u","collapsed":true},{"view":"accordionitem","header":"140u","id":3,"padding":0,"body":"This is 140u","collapsed":true},{"view":"accordionitem","header":"140u","id":4,"padding":0,"body":"This is 140u","collapsed":true},{"view":"accordionitem","header":"jkgu","id":5,"padding":0,"body":"This is jkgu","collapsed":true},{"view":"accordionitem","header":"html","id":6,"padding":0,"body":"This is html","collapsed":true},{"view":"accordionitem","header":"Html","id":7,"padding":0,"body":"This is Html","collapsed":true}]
						//[{"view":"accordionitem","header":"140u","id":2,"padding":0,"body":"This is 140u","collapsed":true},{"view":"accordionitem","header":"140u","id":3,"padding":0,"body":"This is 140u","collapsed":true},{"view":"accordionitem","header":"140u","id":4,"padding":0,"body":"This is 140u","collapsed":true},{"view":"accordionitem","header":"jkgu","id":5,"padding":0,"body":"This is jkgu","collapsed":true},{"view":"accordionitem","header":"html","id":6,"padding":0,"body":"This is html","collapsed":true},{"view":"accordionitem","header":"Html","id":7,"padding":0,"body":"This is Html","collapsed":true}]	
					/*[{
						view: "accordionitem",
					 	header: "Pane 2",
					 	padding: 0,
					// 	//headerAlt:"Pane 2 Closed",
						body: "This is Pane 2 body", //just text
					 	collapsed: true
					 }]*/
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

		console.log("UI CHECK:" + JSON.stringify(ui));



		return (
			<div id="AdminToolbar" >
				<Webix ui={ui} data={data} />
			</div>
		);
	}
}
export default AdminToolbar;

