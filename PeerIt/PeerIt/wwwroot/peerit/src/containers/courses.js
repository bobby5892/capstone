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
    this.handleMenuClick = props.handleMenuClick;
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
  return <AdminInstructorSettings currentUser={this.state.currentUser} role={this.state.role}  handleMenuClick={this.handleMenuClick} viewingCourse={this.state.viewingCourse}/>
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
	//console.log("reload courses");
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
        	//console.log("response.data: " + JSON.stringify(response.data));
        	
        	this.drawCourses();
        } 
      })
      .catch(error => console.error('Error:', error));

  }
 webixGetNextChild(id){
 	// There is a known bug in this -  process to recreate
 	// click on a course - then click on the same course 
 	// this needs to shift the webix content to the new accordion item if it exists - the condition its still missing is when there is no childView

 	let indexCount = -1;

 	let numberOfViews;
		try {
			console.log("CoursesCheck: " + typeof(window.webix.$$("courses")) );
 			if(typeof(window.webix.$$("courses")) != undefined){
 				numberOfViews =  window.webix.$$("courses").getChildViews().length;	
 			}
 		}
 		catch(e){
 			numberOfViews= 0;
 		}
 	
 	//console.log("webixGetNextChild Number of Views: " + numberOfViews);
 	for(let i=0; i<numberOfViews;i++){
 		//console.log("webixGetNextChild: considering " + this.state.viewingCourse + " vs " +  window.webix.$$("courses").getChildViews()[i].config.id);
 		if(this.state.viewingCourse	== window.webix.$$("courses").getChildViews()[i].config.id){
 			//console.log("webixGetNextChildfound current child");
 			indexCount = i;
 		}
 	}
 	//console.log("webixGetNextChild: indexCount : " + indexCount);
 	//console.log("webixGetNextChild NextChildType : " + typeof(window.webix.$$("courses").getChildViews()[(indexCount)+1]) !== "undefined");
 	if(
 		(indexCount != -1 ) && (typeof(window.webix.$$("courses").getChildViews()[(indexCount)+1]) !== "undefined")
 	 ){
 		//console.log("webixGetNextChild: bout to send it back" + indexCount);
 		let nextChildID = window.webix.$$("courses").getChildViews()[(indexCount)+1].config.id;
 	//	console.log("bout to send it back" + nextChildID);
		return nextChildID;
	}
	return false;
 	//console.log("ChildView:" + typeof(window.webix.$$("courses").getChildViews()[++i]));

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
		    //console.log("Courses" + JSON.stringify(this.state.courses));
	        this.state.courses.forEach(element => {
	        	//console.log("considered" + window.webix.$$("courses").index(element.id));
	       		
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
		                	let redraw = false;
		                	// Check if it should redraw
		                	if(parseInt(i) != parseInt(this.state.viewingCourse)){
		                		redraw = true;
		                	}
		                	else{
		                		// They clicked on self
		                		// Check and see if there is another view passed to change the active view to
		                		try {
		                			let index = window.webix.$$("courses").index(i);
		                			//Check if the next index is a thing
		                			console.log("Detected a click on self " + i);
		                			
		                			let nextChild = scope.webixGetNextChild(i);
		                			console.log("next child is: " + nextChild + " compared to " + i);
		                			if(nextChild != false){
		                				console.log("setting view to next child");
		                				let stateChange = {"viewingCourse" : parseInt(nextChild)};
		                				console.log("CHANGING STATE: " + JSON.stringify(stateChange));
		                				this.handleCourseViewer(stateChange);

		                			}
		                				// Lets set the one we're viewing up to what the accordion is showing
		                				
		                			
		                		}
		                		catch(e){
		                			console.log("error" + e);
		                		}
		                	}
		                	// Update the viewing course
		                	this.handleCourseViewer({"viewingCourse" : parseInt(i)});
		                //	console.log("i: " + i + " viewingCourse: " + scope.state.viewingCourse);
		                	// Check if clicking on self and its already viewable - if so do nothing
		                	if(redraw){
		                    		//console.log(" I clicked on a " + i + " and the currentViewed is " + scope.state.viewingCourse);
		                    		//scope.handleCourseViewer({"viewingCourse" : parseInt(i)});
		                    		this.drawCourses();
		                    }
		                 }.bind(this)
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

      // Remove all event handlers on accordians
      // add new click event handlers on accordians

    //  console.log("check : " + this.state.viewingCourse+ " " +  courseID);
        if(courseID != null && this.state.viewingCourse === courseID ){
        	//console.loog("CheckCollapsed: "  + window.webix.$$("courses").index)
        		// Lets check if its already collapsed or not
        		try{
        		/*	let accordIndex = window.webix.$$("courses").index(courseID);
        			//console.log("Debug: " + JSON.stringify(accordID));
        			console.log("Drawing: " + courseID + " in index position" + accordIndex);
        			console.log("CollapseCheck "+ accordIndex+ " : " + window.webix.$$("courses").getChildViews()[accordIndex].config.collapsed);
        			// "collapsed":true,;
        		// Lets try and clear the content in  all the children that we're not drawing
        			let countAccords = window.webix.$$("courses").getChildViews().length;
        			for(let i=0; i<countAccords;i++){

        				 Close - but this is actually in tabular - so gotta figure that out
        				window.webix.$$("courses").getChildViews()[i].define("body", {
        					"view" : "template",
        					"template" : "not shown"
        				});*/
        				//window.webix.$$("courses").getChildViews()[i].define("content", null);
        			//}
        			
        		}
        		catch(e) {
        			 console.log("Error" + e); 
        		}

        		// Need to focus on the next line - its whats causing the fault..
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
                   		content: "AdminInstructorStudentsList",
                   		scroll: "y",
                  		width:250
                  },
                  autoheight:true,
                  collapsed:true,
                  gravity:1,
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
                  	content: "AdminInstructorSettings",
					width:250,
					/*on : {
						'onFocus' : function (i) {
							console.log("I WAS click in body");
							 this.handleMenuClick("CourseContent");
							
						}.bind(this)
					}
                  },
                  on : {
						'onFocus' : function (i) {
							console.log("I WAS click Outside Body ");
							 this.handleMenuClick("CourseContent");
							
						}.bind(this)
					}
                     */
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
        {this.renderAdminInstructorSettings()}
       
        
      </div>
      );
  }
}
export default Courses;

