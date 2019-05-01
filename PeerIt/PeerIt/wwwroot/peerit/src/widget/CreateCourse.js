
import React, { Component } from 'react';
import Webix from '../webix';

class CreateCourse extends Component {

	constructor(props) {
   super(props);
    this.state = {
      currentUser: props.currentUser,
      role: props.role,
      data: null
      
    };
    console.log("ShowCreatecourse: " + props.showCreateCourse);
    //this.showCreateCourse = props.showCreateCourse;

  }
   createCourse() {
    let formData = window.webix.$$("newCourseForm").getValues();

    fetch("Course/CreateCourse?courseName=" + formData.CourseName, {
      method: 'POST', // or 'PUT'
      // body: JSON.stringify({"Email":userName,"Password":password,"returnUrl":null}), // data can be `string` or {object}!
      headers: {
        'Content-Type': 'application/json'
      },
      credentials: "include",
      mode: "cors"
    }).then(res => res.json())
      .then(response => {
        if (response.success) {
          this.setState({});
          window.webix.$$("newCourseWindow").close();
        } else {
          let errors = "";
          response.error.forEach(error => {
            errors += error.description
          });

        }
        console.log(response);

      })
      .catch(error => console.error('Error:', error));
  }
  renderCreateCourseWindow() {
    let scope = this;
    if (window.webix.$$("newCourseWindow") == null) {
      var newWindow = window.webix.ui({
        view: "window",
        id: "newCourseWindow",
        width: 500,
        height: 500,
        move: true,
        position: "center",
        head: {
          type: "space",
          cols: [
            { view: "label", label: "Create New Course" },
            {
              view: "button", label: "Close", width: 70, left: 250,
              click: function () {
                window.webix.$$("newCourseWindow").close();
              }
            }
          ]
        },
        body: {
          type: "space",
          rows: [
            {
              view: "form",
              id: "newCourseForm",
              width: 400,
              elements: [

                { view: "text", label: "Course Name", name: "CourseName", labelWidth: 100, invalidMessage: "Course Name can not be empty" },
               
                {
                  margin: 5, cols: [
                    {
                      view: "button", value: "Create Course", type: "form", click: function () {
                        scope.createCourse();
                      }
                    }
                  ]
                }
              ],
              rules: {
                "CourseName": window.webix.rules.isNotEmpty
              }
            }
          ]
        }

      }).show();
    }
  }
	render(){
    let scope = this;
    let ui = {
        view: "button",
        gravity: 1,
        label: "Create Course",
        id: "createCourse_button",
        value: "Create Course",

        inputWidth: 100,
        click: function () {  
          scope.renderCreateCourseWindow();
        }
      };
    let data = null;
	
		 return(<div  id="CreateCourse">
        <Webix ui={ui} data={data}/>
      </div>
      );
	}
}
export default CreateCourse;

