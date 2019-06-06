
import React, { Component } from 'react';
import Webix from '../webix';

class CreateCourse extends Component {

	constructor(props) {
   super(props);
    this.state = {
      currentUser: props.currentUser,
      role: props.role,
      data: null,
      seed: props.seed

      
    };
    this.redrawAll = props.redrawAll;

  }
   createCourse() {
    let formData = window.webix.$$("newCourseForm").getValues();

    fetch("Course/CreateCourse?courseName=" + formData.CourseName, {
      method: 'POST', // or 'PUT'

      headers: {
        'Content-Type': 'application/json'
      },
      credentials: "include",
      mode: "no-cors"
    }).then(res => res.json())
      .then(response => {
        if (response.success) {
          this.setState({"data" : 0});

          this.redrawAll();
// eslint-disable-next-line          
          location.reload();
        } else {
       //   let errors = "";
          //response.error.forEach(error => {
       //  //   errors += error.description
      //    });

        }
        console.log(response);

      })
      .catch(error => console.error('Error:', error));
  }
  renderCreateCourseWindow() {
    
    if (window.webix.$$("newCourseWindow") == null) {
       window.webix.ui({
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
                //temp fix - if more time add a cleaner reload
                
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
                        this.createCourse();
                        window.webix.$$("newCourseWindow").close();
                      }.bind(this)
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

