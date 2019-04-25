
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
          {
            view: "accordion",
            scroll: "y",
            multi: true,

            rows: [ //or cols 

              {
                view: "accordionitem",
                header: "Pane 1",
                headerHeight: 50,
                //headerAlt:"Pane 2 Closed", 
                body: "form", //ui component
                height: 100,
                collapsed: true
              },
              {
                view: "accordionitem",
                header: "Pane 2",
                padding: 0,
                //headerAlt:"Pane 2 Closed",
                body: "This is Pane 2 body", //just text
                collapsed: true
              },
              { header: "CS195", body: "arg 1", collapsed: true},
              { header: "CS260", body: "blah", collapsed: true },
              { header: "CS195-2", body: "webcrap 3", collapsed: true }
            ]
          },
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

                  // Lets only allow once

                }
              },
              // { view:"button", label:"Form", type:"form" },
              // { view:"button", label:"Danger", type:"danger" },
              // { view:"button", label:"Prev", type:"prev" },
              // { view:"button", label:"Next", type:"next" }
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

