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
        	console.log("response.data: " + JSON.stringify(response.data));
        	
        	this.drawCourses();
        } 
      })
      .catch(error => console.error('Error:', error));

  }

  drawCourses(){

  	let scope=this;
  	   let accordArray =[];
  	   if(this.state.courses != null && this.state.courses.length>0){
   			// Clear
   			/*  window.webix.ui([{

		        view:"scrollview",
		        id:"verses",
		        scroll:"y", // vertical scrolling
		        height: 350, 
		        width: 250,
		        body:{
		        rows:[
		            { 
		            	"view":"accordion",
			            "gravity":3,
			            "scroll" : "y",
			            "multi":false,
			            "css":"webix_dark",
			            "id" : "courses",
			            "rows" :  []
		            }
		        ]   
		      }        
		    }],window.webix.$$("courses")); */
		    // lets remove 
		    console.log("Courses" + JSON.stringify(this.state.courses));
	        this.state.courses.forEach(element => {
	        	console.log("considered" + window.webix.$$("courses").index(element.id));
	       		
    			let accord = {
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
		        // Lets try if it doesnt exisit
		        try{
		       		 if(window.webix.$$("courses").index(element.id) == -1){
		        		window.webix.$$("courses").addView(accord);
		        	}
		        }
		        catch (e){
		        		console.log("error" + e);
		        		
		        }
				
        	});
		}     
  }

  renderSubMenu(courseID){

    if(this.state.role === "Administrator" || this.state.role === "Instructor"){
      let renderObjects = {
        'Students' : null
      };
      console.log("check : " + this.state.viewingCourse+ " " +  courseID);
        if(this.state.viewingCourse === courseID){
        	console.log("NOW IS A GOOD TIME T TRY AND CHANGE IDS");
        	/*window.webix.ui([{
             	  css:"subCourseMenu",
                  header: "Students",
                  id:"AdminInstructorSubListItem",
                  body: {
                   // view: "context",
                    content: "AdminInstructorStudentsList"
                  },
                  autoheight:true,
                  collapsed:true,
                  gravity:1
                }],window.webix.$$("AdminInstructorSubListItem"));*/
                //console.log(window.webix.$$("coursesTabView"));
               // window.webix.$$("AdminInstructorSubListItem").setContent(document.getElementById("AdminInstructorStudentsList"));

               window.webix.ui([{
                id:"coursesTabView"+courseID,
                view: "tabview",
                css:"subCourseTabMenu",
                multiview:{
                   animate:true
                },
              cells: [
              {
             	  css:"subCourseMenu",
                  header: "Students",
                  id:"AdminInstructorSubListItem",
                  body: {
                   		content: "AdminInstructorStudentsList"
                  },
                  autoheight:true,
                  collapsed:true,
                  gravity:1
                },
                {
                  css:"subCourseMenu",
                  header: "Assignments",
                  body: {
                    view: "template",
                    template: "selected"
                  },
                  collapsed:true
                },              
                {
                  css:"subCourseMenu",
                  header: "Bulk",
                  body: {
                    view: "template",
                    template: "bulk"
                  }
                },
                {
                  css:"subCourseMenu",
                  header: "Settings",
                  body: {
                     view: "template",
                    template: "settings"
                  }
                }              
              ]
            }],window.webix.$$("coursesTabView"+courseID));

        }  
        else{
           return ( 
            [
              {
                id:"coursesTabView"+courseID,
                view: "tabview",
                css:"subCourseTabMenu",
                multiview:{
                   animate:true
                },
              cells: [
              {
             	  css:"subCourseMenu",
                  header: "Students",
                  id:"AdminInstructorSubListItem",
                  body: {
                    view: "template",
                    template: "Default de"

                  },
                  autoheight:true,
                  collapsed:true,
                  gravity:1
                },
                {
                  css:"subCourseMenu",
                  header: "Assignments",
                  body: {
                    view: "template",
                    template: "Assignments"
                  },
                  collapsed:true
                },              
                {
                  css:"subCourseMenu",
                  header: "Bulk",
                  body: {
                    view: "template",
                    template: "bulk"
                  }
                },
                {
                  css:"subCourseMenu",
                  header: "Settings",
                  body: {
                     view: "template",
                    template: "settings"
                  }
                }              
              ]
            }
       	 ]
      	);
      }
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
            "rows" : []

            }
        ]   
      }

        
    };
    let data = null;
     return(
      <div id="Courses">
        
        <Webix ui={ui} data={data}/>
        {this.renderAdminInstructorStudentsList()}
       
        
      </div>
      );
  }
}
export default Courses;

