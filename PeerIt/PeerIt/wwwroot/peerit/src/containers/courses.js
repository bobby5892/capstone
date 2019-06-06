// example of custom component with Webix UI inside
// this one is a static view, not linked to the React data store

import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import Webix from '../webix';
import '../css/courses.css';

class Courses extends Component {

  constructor(props) {

    super(props);
    this.state = {
      currentUser: props.currentUser,
      role: props.role,
      data: null,
      viewingCourse: props.viewingCourse,
      courseGroup : null,
      Courses: []
    };
   
    this.handleCourseViewer = props.handleCourseViewer;
    this.handleMenuClick = props.handleMenuClick;
    this.component = {};
    window.webix.protoUI({
      name: "react",
      defaults: {
        borderless: true
      },
      $init: function (config) {
        this.$ready.push(function () {
          ReactDOM.render(
            this.config.app,
            this.$view
          );
        });
      }
    }, window.webix.ui.view);
    this.loadCourses();
    if (this.state.role === "Student") {
      console.log("At Constructor CourseViewing: " + this.state.viewingCourse);
      this.getCourseGroup();
    }
  }
  getCourseGroup(props){
     //console.log("trying to get course group:"  + "State" + JSON.stringify(this.state));
     let CourseID = this.state.viewingCourse;
     if(CourseID == null && typeof(props) != "undefined"){ CourseID = props.viewingCourse; }
     fetch("/StudentAssignment/GetCourseGroup?courseId=" + CourseID, {
        method: 'GET', // or 'PUT'
        headers: {
          'Content-Type': 'application/json'
        },
        credentials: "include",
        mode: "no-cors"
      }).then(res => res.json()).then(response => {
          if (response.success) {
            console.log("GROUP:" + JSON.stringify(response.data[0].reviewGroup));

            if(this.state.courseGroup != response.data[0].reviewGroup){
              this.setState({courseGroup:response.data[0].reviewGroup});
              window.webix.$$("GroupAssignments"+CourseID).load("/StudentAssignment/GetAssignmentsByCourseAndReviewGroup?courseId=" + CourseID + "&reviewGroupId=" +response.data[0].reviewGroup);
            }
          }
          else{
         
          }
        })
        .catch(error => console.error('Error:', error));
      
  }
  componentWillReceiveProps(props) {
    this.setState(props);
    console.log("At Receive Props CourseViewing: " + this.state.viewingCourse + props.viewingCourse);
    this.getCourseGroup(props);
    this.loadCourses();
    
    //console.log("reload courses");
    // Trigger Webix to Redraw the component
    //window.webix.$$().setHTML("<h1>YEP</h1>");
  }
  loadCourses() {
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
          if(response.data.length > 0){
          	this.setState({ courses: response.data });
          }
          this.drawCourses();
        }
      })
      .catch(error => console.error('Error:', error));

  }

  drawCourses() {
  
    if (this.state.courses != null && this.state.courses.length > 0) {

      this.state.courses.forEach(element => {
        //console.log("considered" + window.webix.$$("courses").index(element.id));

        let accord = {
          view: "accordionitem",
          header: element.name,
          id: element.id,
          padding: 0,
          css: "courseMenuItem",
          autoheight:true,
          body: {
            cols: this.renderSubMenu(element.id)
          },
          on: {
            'onItemClick': function (i) {
              let redraw = false;
              // Check if it should redraw
              if (parseInt(i) !== parseInt(this.state.viewingCourse)) {
                redraw = true;
              }
              else {
                // They clicked on self
                // Check and see if there is another view passed to change the active view to
                try {
                  //let index = window.webix.$$("courses").index(i);
                  let nextChild = this.webixGetNextChild(i);
                  //console.log("next child is: " + nextChild + " compared to " + i);
                  if (nextChild !== false) {
                    //console.log("setting view to next child");
                    let stateChange = { "viewingCourse": parseInt(nextChild) };
                    this.handleCourseViewer(stateChange);
                  }
                  // Lets set the one we're viewing up to what the accordion is showing
                }
                catch (e) {
                  console.log("error" + e);
                }
              }

              if (redraw) {
                this.handleCourseViewer({ "viewingCourse": parseInt(i) });
                this.drawCourses();
              }
            }.bind(this)
          },
          collapsed: true,
          height: 200
        };
        // Lets try if it doesnt exisit
        try {
          if (window.webix.$$("courses").index(element.id) === -1) {
            window.webix.$$("courses").addView(accord);
          }
        }
        catch (e) {
          console.log("error" + e);
        }
      });
    }
    else{
    	// No Courses
    	if(window.webix.$$("courses").getChildViews().length == 0){
	    	let ui = {
	    	  view: "accordionitem",
	          header: "No Courses",
	          padding: 0,
	          css: "courseMenuItem",
	          autoheight:true,
	          body: {
	             view:"template",
	             template: "No Courses"
	          }
	        };
	    	 window.webix.$$("courses").addView(ui);
	    }
    	
    	
    }
  }

  renderSubMenu(courseID) {
   // console.log("Submenu - Check State:" + JSON.stringify(this.state));
    if (this.state.role === "Administrator" || this.state.role === "Instructor") {
      return (
        [
          {
            id: "coursesTabView" + courseID,
            view: "tabview",
            css: "subCourseTabMenu",
            multiview: {
              animate: true
            },
            cells: [
              {
                css: "subCourseMenu",
                header: "Students",
                id: "AdminInstructorSubListItem",
                autoheight:true,
                body:  {
                    autoheight: true,
                    view: "datatable",
                    columns: [
                      { id: "firstName", header: "First Name", width: 100 },
                      { id: "lastName", header: "Last Name", width: 80 },

                    ],
                    url: "/Course/GetStudents?courseID=" + courseID

               
                  },
                autoheight: true,
                collapsed: true,
                gravity: 1
              },
              {
                css: "subCourseMenu",
                header: "Assignments",
                autoheight:true,
                body:  {
                    autoheight: true,
                    view: "datatable",
               		id: "Assignments"+courseID,
                    columns: [
                      { id: "name", header: "Name", width:150 },
                      { id: "dueDate", header: "DueDate", width:150 },

                    ],
                    url: "/CourseAssignment/Assignments?courseID=" + courseID,
                    on : { 'onItemClick' : function(i){
	                    //	console.log("test" + i);	
	                    	//console.log("Data:" + JSON.stringify(window.webix.$$("Assignments" + courseID).getItem(i)));
                       	this.handleCourseViewer({viewingAssignment:window.webix.$$("Assignments" + courseID).getItem(i)});
                        this.handleMenuClick("ShowAssignment");
                        
	                    }.bind(this) 
	                }
                  },
                collapsed: true
              },
              {
                css: "subCourseMenu",
                header: "Bulk",
                body: {
                  view: "template",
                  template: "bulk"
                }
              },
              {
                css: "subCourseMenu",
                header: "Settings",
                body: {
                  view: "button",
                  label: "Settings",
                  type: "standard",
                  on: {
                    'onItemClick': function (i) {
                      this.handleMenuClick("CourseContent");
                    }.bind(this)
                  }
                }
              }
            ]
          }
        ]
      );
      //}
    }
    else if (this.state.role === "Student") {
     
      return ([{
        view: "tabview",
        cells: [
          {
            header: "Your Assignments",
            body:  {
                autoheight: true,
                view: "datatable",
              id: "Assignments"+courseID,
                columns: [
                  { id: "name", header: "Name", width:150 },
                  { id: "dueDate", header: "DueDate", width:150 },
                ],
                url: "/CourseAssignment/Assignments?courseID=" + courseID,
                on : { 'onItemClick' : function(i){
                    this.handleCourseViewer({viewingAssignment:window.webix.$$("Assignments" + courseID).getItem(i)});
                    // console.log("AsSIGNMENT: " + JSON.stringify(window.webix.$$("Assignments" + courseID).getItem(i)));
                      
                    this.handleMenuClick("ShowAssignment");
                  }.bind(this) 
              }
            }
          },
          {
            header: "Group Assignments",
            body:  {
                autoheight: true,
                view: "datatable",
                id: "GroupAssignments"+courseID,
                columns: [
                 
                  
                  { id: "firstName", map:"#appUser.firstName#", header: "First Name", width:100 },
                  { id: "lastName", map:"#appUser.lastName#", header: "Last Name", width:100 },

                  { id: "name", map: " #courseAssignment.name#", header: "Assignment", width:150 },
                ],
                url: "/StudentAssignment/GetAssignmentsByCourseAndReviewGroup?courseID=" + courseID + "&reviewGroupId=",
                on : { 'onItemClick' : function(i){
                    //console.log("Load This assignent: " + window.webix.$$("GroupAssignments" + courseID).getItem(i).courseAssignment);
                    this.handleCourseViewer({viewingAssignment:window.webix.$$("GroupAssignments" + courseID).getItem(i).courseAssignment});                      
                    this.handleMenuClick("ShowAssignment");
                  }.bind(this) 
              }
            }
          }
        ]
      }]);

    }
  }

  render() {
    let ui = {

      view: "scrollview",
      id: "verses",
      scroll: "y", // vertical scrolling
      height:500,
      width: 320,
      body: {
        rows: [
          {
          	autoheight:true,
            "view": "accordion",
            "gravity": 3,
            "scroll": "y",
            "multi": false,
            "css": "webix_dark",
            "id": "courses",
            "rows": []
          }
        ]
      }


    };
    let data = null;
    return (
      <div id="Courses">

        <Webix ui={ui} data={data} />




      </div>
    );
  }
}
export default Courses;

