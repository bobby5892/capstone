import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import Webix from '../webix';

class CourseContent extends Component {
  constructor(props) {
    super(props);
    this.state = {
      currentUser: props.currentUser,
      role: props.role,
      viewingCourse: props.viewingCourse,
      addingStudent: false,
      seed : props.seed

    }
    this.redrawAll = props.redrawAll;
    this.handleCourseViewer = props.handleCourseViewer;
    this.handleMenuClick = props.handleMenuClick;
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
    }, window.webix.ui.view)
  }
  addStudentWindow() {
    window.webix.ui({
      view: "window",
      id: "addStudentWindow",
      width: 500,
      height: 500,
      move: true,
      position: "center",
      head: {
        type: "space",
        cols: [
          { view: "label", label: "Add Student to Course" },
          {
            view: "button", id: "addStudentButton", label: "Close", width: 70, left: 250,
            click: function () {
              this.setState({ addingStudent: false });
              window.webix.$$("addStudentWindow").close();
            }.bind(this)
          }
        ]
      },
      body: {
        type: "space",
        rows: [
          {
            view: "form",
            id: "addStudentForm",
            width: 400,
            elements: [
              { view: "text", label: "Email", name: "Email", labelWidth: 100, invalidMessage: "Must be valid email address" },
              {
                margin: 5,
                cols: [
                  {
                    view: "button",
                    value: "Add Student",
                    type: "form",
                    id: "AddStudentFormButton"
                  }
                ]
              }
            ],
            rules: {
              "Email": window.webix.rules.isEmail
            }
          }
        ]
      }

    }).show();

    console.log("Does button have click: " + window.webix.$$("AddStudentFormButton").hasEvent("onItemClick"));
    // If its not already bound lets do it
    if (window.webix.$$("AddStudentFormButton").hasEvent("onItemClick")=== false) {
      console.log("binding function to button");
      window.webix.$$("AddStudentFormButton").attachEvent("onItemClick", function (i) {

        let validResponse = window.webix.$$("addStudentForm").validate();
        if (validResponse) {
            let email = window.webix.$$("addStudentForm").elements["Email"].config.value;
            this.addStudent(email,this.state.viewingCourse);
        }
      }.bind(this));
    }

  }
  addStudent(email,courseID) {
        fetch("/Course/AddStudentToCourse?courseID=" + courseID + "&studentEmail=" + email, {
          method: 'POST', // or 'PUT'
          headers:{
            'Content-Type': 'application/json'
          },
          credentials: "include",
          mode:"no-cors"
        }).then(res => res.json())
        .then(response => {
          if(response.success){
            // Should reload student list
            console.log("Added Student");
            window.webix.$$("addStudentWindow").close();
            //this.handleMenuClick("LiveFeed");
             this.redrawAll();

          }
        })
        .catch(error => console.error('Error:', error));
  }
  componentWillReceiveProps(props) {
    this.setState(props);
  }
  removeStudent(studentID){
     fetch("/Course/RemoveStudentToCourse?courseID=" + this.state.viewingCourse + "&studentID=" + studentID, {
          method: 'DELETE', // or 'PUT'
          headers:{
            'Content-Type': 'application/json'
          },
          credentials: "include"
        }).then(res => res.json())
        .then(response => {
          if(response.success){
            // Should reload student list
            // this.handleMenuClick("LiveFeed");
             this.redrawAll();
          }
        })
        .catch(error => console.error('Error:', error));
  }
  getStudentGroup(studentID, courseID) {
    fetch("/Course/GetStudentGroup?studentID=" + studentID + "&courseID=" + courseID, 
    {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json'
      },
      credentials: "include"
    })
    .then(response => {
      if (response.success) {
        return response;
      }
    })
    .catch(error => console.error('Error:', error));
  }
  changeStudentGroup(courseGroupID, groupValue) {
    fetch("/Course/ChangeStudentGroup?&courseGroupID=" + courseGroupID + "&reviewGroupID=" + groupValue,
    {
      method: 'PATCH',
      headers: {
        'Content-Type': 'application/json'
      },
      credentials: "include"
    })
    .then(response => {
      if (response.success) {
        this.redrawAll();
      }
    })
    .catch(error => console.error('Error:', error));
  }
  renderUploadAssignmentWindow() {
    let scope = this;

    var newWindow = window.webix.ui({
      view: "window",
      id: "uploadAssignmentWindow",
      width:600,
      //height: 600,
      move: true,
      position: "center",
      head: {
        type: "space",
        cols: [
          { view: "label", label: "Upload an Assignment" },
          {
            view: "button", label: "Close", 
            width: 70, 
            left: 250,
            click: function () {
              //scope.setState({"editUser" : null });
              window.webix.$$("uploadAssignmentWindow").close();
            }
          }
        ]
      },
      body: {
        type: "space",
        rows: [
          {
            view: "form",
            id: "uploadAssignmentForm",
            //width: "auto",
            elements: [
              { view: "label", label: "Upload your Assignment form here: ", name: "", labelWidth: "auto", value: "" },
              {
                
                view: "uploader", inputName: "files", upload: "/CourseAssignment/CreateAssignment", 
                //urlData:{assignmentname:"new assignment",courseID:13,dueDate:"07/08/2019"} ,
                id: "AssignmentFile", link: "mylist", value: "Upload File", autosend: false
              },
              {
                view: "list", id: "mylist", type: "uploader",
                autoheight: true, borderless: true
              },
              
              { view: "text", label: "Assignment Name", name: "Assignment_Name",labelWidth: 200,invalidMessage:"Please enter Assignment Name" },
              /*{ view: "text", label: "Course", name: "Course", labelWidth: 200,invalidMessage:"What Course is this? ",value:this.state.viewingCourse },*/
              {view:"calendar",
              id:"my_calendar",
              name:"Due_Date",
              date:new Date(
                ),
              weekHeader:true,
              events:window.webix.Date.isHoliday,
              width:300,
              height:250},
              
              //{ view: "text", label: "Due Date", name: "Due_Date", labelWidth: 200,invalidMessage:"Please enter Valid Date" }, 
              {
                view: "button",value:"Upload", type:"form", 
                click: function () {
                  let validResponse = window.webix.$$("uploadAssignmentForm").validate();
                  let FormVal = window.webix.$$("uploadAssignmentForm").getValues();
                  window.webix.$$("AssignmentFile").define({
                    urlData:{assignmentname:FormVal.Assignment_Name,
                    courseID:FormVal.Course,
                    dueDate:(window.webix.$$("my_calendar").config.date).toUTCString()
                    }
                  });
                  window.webix.$$("AssignmentFile").send(function(response) {
                    console.log("upload send: " + JSON.stringify(response));
                    if (response != null){
                      console.log(Date("07/08/2019"));
                      window.webix.message("Succsess");
                      //window.webix.$$("uploadAssignmentWindow").attachEvent("onUploadComplete", function(response){
                        window.webix.$$("uploadAssignmentWindow").close();
                        //window.webix.message("done");
                    //}); 
                    }
                    else {
                    //console.log(Date("07/08/2019"));
                    alert("Nothing to Submit");
                  }})
                }
              }
            ],
            rules: {
              "Course": window.webix.rules.isNotEmpty,
              "Due_Date": window.webix.rules.isNotEmpty,
              "Assignment_Name":window.webix.rules.isNotEmpty
            }
          }
        ]
      }
    }).show();
    window.webix.$$("uploadAssignmentForm").setValues(
         { Course:this.state.viewingCourse}
     );
  }
  renderChangeStudentGroupWindow(courseGroupID) {
    let scope = this;

    var newWindow = window.webix.ui({
      view: "window",
      id: "changeStudentGroupWindow",
      width:600,
      move: true,
      position: "center",
      head: {
        type: "space",
        cols: [
          { view: "label", label: "Change Student Group" },
          {
            view: "button", label: "Close", 
            width: 70, 
            left: 250,
            click: function () {
              window.webix.$$("changeStudentGroupWindow").close();
            }
          }
        ]
      },
      body: {
        type: "space",
        rows: [
          {
            view: "form",
            id: "changeStudentGroupForm",
            elements: [
              { id: courseGroupID, view: "text", label: "CourseGroup Record ID: " + courseGroupID, name: "studentGroup",labelWidth: 200,
               invalidMessage:"Please Enter a Valid Student Group ID" },
              {
                view: "button",value:"Change", type:"form", 
                click: function () {
                  let validResponse = window.webix.$$("changeStudentGroupForm").validate();
                  let FormVal = window.webix.$$("changeStudentGroupForm").getValues();
                  this.changeStudentGroup(courseGroupID, FormVal.studentGroup);
                  window.webix.$$("changeStudentGroupWindow").close();
                  this.redrawAll();
                }.bind(this)
              }
            ],
            rules: {
              "studentGroup": window.webix.rules.isNotEmpty
            }
          }
        ]
      }
    }).show();
 /*   window.webix.$$("uploadAssignmentForm").setValues(
         { Course:this.state.viewingCourse}
     );
     */
  }
  deleteAssignment(assignmentID){
    fetch("/CourseAssignment/DeleteAssignment?assignmentID=" + assignmentID, {
          method: 'DELETE', // or 'PUT'
          headers:{
            'Content-Type': 'application/json'
          },
          credentials: "include"
          
        }).then(res => res.json())
        .then(response => {
          if(response.success){
            this.redrawAll();
            window.webix.$$("AssignmentsContent").load("/CourseAssignment/Assignments?courseID=" +this.state.viewingCourse);
          }
        })
        .catch(error => console.error('Error:', error));
  }
  render() {
    console.log("render course content");
    let reviewGroupOptions = function() {
      let optionString = ""
      for (let i = 1; i <= 20; i ++) {
        optionString += "<option id='" + i + "'>Group " + i + "</option>";
      }
      return optionString;
    }
    let putGroupID = function() {
      fetch()
    }
    let ui = {
      rows: [
        {
          header: "Course Settings (ID: " + this.state.viewingCourse + ")", body: " "
        },
       
        {
          view: "tabview",
          autoheight: true,
          header: "Course",
          cells: [
            {
              autoheight: true,
              header: "Assignments",
              body: {
                autoheight: true,
                rows: [
                  {
                    view: "button",
                    value: "Add Assignment",
                    type: "form",
                    id: "AddAssignmnetButton",
                    on: {
                          'onItemClick' : function(i){
                              this.renderUploadAssignmentWindow();
                            }.bind(this)
                    }
                  },
                  {
                    css: "subCourseMenu",
                    header: "Assignments",
                    autoheight:true,
                    body:  {
                        autoheight: true,
                        view: "datatable",
                      id: "AssignmentsContent",
                        columns: [
                          { id: "name", header: "Name",  width:150, sort:"string"},
                          { id: "dueDate", header: "DueDate", width:150, sort:"string"},
                          { header: "Manage",  gravity:2,template:function(obj){ 
                             return "<div class='webix_el_button'><button class='webixtype_base'>Remove</button></div>";
                           }}
                        ],
                        url: "/CourseAssignment/Assignments?courseID=" + this.state.viewingCourse,
                        onClick : {
                            webixtype_base:function(ev, id, html){
                            //eslint-disable-next-line
                            if(confirm("Are you sure you want to delete this assignment?")){
                               // window.webix.alert("Clicked row "+id);
                              let assignment =  window.webix.$$("AssignmentsContent").getItem(id);
                              this.deleteAssignment(assignment.id);
                            }
                            
                          }.bind(this) 
                        }   
                      }
                    }
                ]
              }
            },
            {
              header: "Groups",
              body: {
                autoheight: true,
                rows: [
                  {
                    autoheight: true,
                    view: "datatable",
                    columns: [
                      { id: "id", map: "#fK_AppUser.id#", header: "", width: 50 },
                      { id: "firstName", map: "#fK_AppUser.firstName#", header: "First Name", width: 200 },
                      { id: "lastName", map: "#fK_AppUser.lastName#", header: "Last Name", width: 200 },
                      { id: "reviewGroup", header: "Review Group", width: 400 }
                    ],
                    on:{
                      onItemClick:function(id, ev, html){
                        console.log(id["row"]);
                        if (window.webix.$$("changeStudentGroupWindow") == null) {
                          this.renderChangeStudentGroupWindow(id["row"]);
                        }
                      }.bind(this)
                    },
                    url: "/Course/GetStudentGroups?courseID=" + this.state.viewingCourse
                  }
                ]
              }
            },
            {
              header: "Students",
              body: {
                autoheight: true,
                rows: [
                  {
                    view: "button",
                    value: "Add Student",
                    css: "webix_primary",
                    inputWidth: 100,
                    on: {
                      'onItemClick': function (i) {
                        if (!this.state.addingStudent) {
                          this.setState({ addingStudent: true });
                          this.addStudentWindow();
                        }
                      }.bind(this)
                    }
                  },
                  {
                    autoheight: true,
                    view: "datatable",
                    columns: [
                      { id: "rank", header: "", width: 50 },
                      { id: "firstName", header: "First Name", width: 200 },
                      { id: "lastName", header: "Last Name", width: 80 },
                      { header: "Enrollment", width: 100 ,   template:"<input type='button' value='Remove' class='remove_button'>"}
                        
                      
                    ],
                    onClick : {
                      remove_button:function (i,ev){
                             // Don't remove the comment below - its actually functional.
                             //eslint-disable-next-line
                             let confirmCheck = confirm("Are you sure you want to remove this student?");
                             if(confirmCheck){
                                console.log("I want to remove " + ev.row);
                                this.removeStudent(ev.row);
                             }
                          }.bind(this)
                    },
                    url: "/Course/GetStudents?courseID=" + this.state.viewingCourse
                  }
                ]
              }
            }
          ]
        }
      ]
    };
    let data = null;

    return (
      <div id="CourseContent">
        <Webix ui={ui} data={data} />

      </div>
    );
  }
}
export default CourseContent;