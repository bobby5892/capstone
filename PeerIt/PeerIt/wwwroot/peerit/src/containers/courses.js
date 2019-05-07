// example of custom component with Webix UI inside
// this one is a static view, not linked to the React data store

import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import Webix from '../webix';
import '../css/courses.css';
import AdminInstructorAssignments from '../widget/CourseViewer/AdminInstructorAssignments';
import AdminInstructorSettings from '../widget/CourseViewer/AdminInstructorSettings';
import AdminInstructorStudentsList from '../widget/CourseViewer/AdminInstructorStudentsList';
import StudentGroupAssignments from '../widget/CourseViewer/StudentGroupAssignments';
import StudentYourAssignments from '../widget/CourseViewer/StudentYourAssignments';
import AdminInstructorBulk from '../widget/CourseViewer/AdminInstructorBulk';
class Courses extends Component {

  constructor(props) {

    super(props);
    this.state = {
      currentUser: props.currentUser,
      role: props.role,
      data: null,
      viewingCourse: props.viewingCourse,
      Courses: [{
              view: "accordionitem",
              header: "No Courses",
              id : "noCourse",
              padding: 0,
              body:"No Courses", 
              collapsed: false
            }]
    };
    console.log("Receiving course view" + props.viewingCourse);
    this.handleCourseViewer = props.handleCourseViewer;
    this.component = {};
    window.webix.protoUI({
      name:"react",
      defaults:{
        borderless:true
      },
      $init:function(config){
        this.$ready.push(function(){    
          ReactDOM.render(
            this.config.app,
            this.$view
          );
        });
      }
    }, window.webix.ui.view);
    this.loadCourses();
}

renderAdminInstructorAssignments(){
  return <AdminInstructorAssignments currentUser={this.state.currentUser} role={this.state.role} viewingCourse={this.state.viewingCourse}/>
}
renderAdminInstructorBulk(){
  return <AdminInstructorBulk currentUser={this.state.currentUser} role={this.state.role} viewingCourse={this.state.viewingCourse}/>
}
renderAdminInstructorSettings(){
  return <AdminInstructorSettings currentUser={this.state.currentUser} role={this.state.role} viewingCourse={this.state.viewingCourse}/>
}
renderAdminInstructorStudentsList(){

  return  <AdminInstructorStudentsList currentUser={this.state.currentUser} role={this.state.role} viewingCourse={this.state.viewingCourse}/>
}
renderStudentGroupAssignments(){
  return <StudentGroupAssignments currentUser={this.state.currentUser} role={this.state.role} viewingCourse={this.state.viewingCourse}/>
}
renderStudentYourAssignments(){
  return <StudentYourAssignments currentUser={this.state.currentUser} role={this.state.role} viewingCourse={this.state.viewingCourse}/>
}
  componentWillReceiveProps(props){
	this.setState(props);
	this.loadCourses();
	console.log("reload courses");
  		// Trigger Webix to Redraw the component
  		//window.webix.$$().setHTML("<h1>YEP</h1>");
  } 
loadCourses() {
 let scope = this;
    //let accord = [];
    fetch("Course/GetCourses", {
      method: 'GET', // or 'PUT'
      headers: {
        'Content-Type': 'application/json'
      },
      credentials: "include",
      mode: "no-cors"
    }).then(res => res.json())
      .then(response => {
        if (response.success) {
        	this.setState({courses: response.data});
        	scope.drawCourses();
        } 
      })
      .catch(error => console.error('Error:', error));

  }
  drawCourses(){

  	let scope=this;
  	   let accord = {};
  	   if(this.state.courses != null && this.state.courses.length>0){
	        this.state.courses.forEach(element => {
	            accord = {
	              view: "accordionitem",
	              header: element.name,
	              id: element.id,
	              padding: 0,
	              css: "courseMenuItem",
	              body:{ 
	                cols : this.renderSubMenu(element.id)
	              },
	              on:{
	                'onItemClick' : function (i){
	                    scope.handleCourseViewer({"viewingCourse" : parseInt(i)});
	                    scope.drawCourses();
	                 }
	              },
	              collapsed: true,
	              height:200
	            };
	            window.webix.$$("courses").addView(accord);

	      	});

		 
		    window.webix.$$("courses").removeView("noCourse");
		      
	        window.webix.$$("coursesTabView").attachEvent("onViewShow", function(){
	              // your handler here
	          });
	          // We need to watch for a click on the header - then load content based on the header
		}     
  }

  renderSubMenu(courseID){

    if(this.state.role === "Administrator" || this.state.role === "Instructor"){
      let renderObjects = {
        'Students' : null
      };
      console.log("check : " + this.state.viewingCourse+ " " +  courseID);
      if(this.state.viewingCourse === courseID){
      	console.log("trying content");
        renderObjects.studentList =  {
                  css:"subCourseMenu",
                  header: "Students",
                  content: "AdminInstructorStudentsList",
                  autoheight:true,
                  collapsed:true,
                  gravity:1
                };
      }
      else{
		   renderObjects.studentList =  {
                  css:"subCourseMenu",
                  header: "Students",
                  template: "Not visible",
                  autoheight:true,
                  collapsed:true,
                  gravity:1
                };
      }


          return ( 
            [
              {
                id:"coursesTabView",
                view: "tabview",
                css:"subCourseTabMenu",
                multiview:{
                   animate:true
                },
              cells: [
               renderObjects.studentList,
                {
                  css:"subCourseMenu",
                  header: "Assignments",
                  body: {
                    view: "template",
                    template: "Default template with some text inside"
                  },
                  collapsed:true
                },              
                {
                  css:"subCourseMenu",
                  header: "Bulk",
                  body: {
                    id: "menuItemAdminInstructorBulk",
                    body: "temp"
                  }
                },
                {
                  css:"subCourseMenu",
                  header: "Settings",
                  body: {
                    id: "menuItemAdminInstructorSettings",
                    body: "temp"
                  }
                }              
              ]
            }
        ]
      );
    }
    else if (this.state.role === "Student"){
      return ( [{
        view: "tabview",
        cells: [
          {
            header: "Your Assignments",
            body: {
              id: "menuItemStudentYourAssignments",
              body: "temp"
            }
          },
          {
            header: "Group Assignments",
            body: {
              id: "menuItemStudentGroupAssignments",
              body: "temp"
            }
          }
        ]
      }]);
    }
  }
   componentWillReceiveProps(props){
  		this.setState(props);
  }
  render() {
    let ui = {

        view:"scrollview",
        id:"verses",
        scroll:"y", // vertical scrolling
        height: 350, 
        width: 250,
        body:{
        rows:[
            { "view":"accordion",
            "gravity":3,
            "scroll" : "y",
            "multi":false,
            "css":"webix_dark",
            "id" : "courses", 
            "rows":Array.from(this.state.Courses)}
        ]   
      }

        
    };
    let data = null;
     return(
      <div id="Courses">
       
        <Webix ui={ui} data={data}/>
        {this.renderAdminInstructorStudentsList()}
        {this.drawCourses()}
      </div>
      );
  }
}
export default Courses;

