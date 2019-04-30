
import React, { Component } from 'react';
import Webix from '../webix';
import { compileFunction } from 'vm';

class InstructorToolbar extends Component {

  constructor(props) {
    super(props);
    this.state = {
      data: null
    };
    this.logout = props.logout;
    this.handleCreateCourseWindow = props.handleCreateCourse;

  }
  

  render() {
    let scope = this;
    let data = null;
    let ui =
    {
      type: "space",
      padding: 0,
      //responsive: "a1",
      height: window.innerHeight,
      //width: window.innerWidth,
      rows:
        [
          {
            view: "list",
            data: ["Instructor", "Reports", "Settings", "Logout"],
            ready: function () {
              this.select(this.getFirstId());
            },
            click: function (a) {
              if (a === "Logout") {
                //Attempt to call the logout chain
                this.logout();
              }
            }.bind(this),
            select: true,
            scroll: false,
            //width: 350,
            height: 150
          },
          // This is where we would render courses        
          {
            view: "form", scroll: false,
            elements: [
              {
                view: "button",
                label: "Create Course",
                id: "createCourse_button",
                value: "Create Course",

                inputWidth: 100,
                click: function () {
                  scope.handleCreateCourseWindow();
                }
              },
            ]
          }
        ]
    };


    console.log("attempted render");
    return (<div id="InstructorToolbar" >
      <Webix ui={ui} data={data} />
    </div>
    );
  }
}
export default InstructorToolbar;

