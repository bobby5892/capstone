// example of custom component with Webix UI inside
// this one is a static view, not linked to the React data store

import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import Webix from '../webix';

 
class Courses extends Component {

  constructor(props) {

    super(props);
    this.state = {
      currentUser: props.currentUser,
      role: props.role,
      data: null,
      Courses: [{
              view: "accordionitem",
              header: "No Courses",
              id : "noCourse",
              padding: 0,
              body:"No Courses", 
              collapsed: false
            }]
    };

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
          let accord = {};
          response.data.forEach(element => {
            accord = {
              view: "accordionitem",
              header: element.name,
              id: element.id,
              padding: 0,
              body: element.name, 
              collapsed: true,
              height:200
            };
            window.webix.$$("courses").addView(accord);
          })
           this.setState({"Courses":accord});
           window.webix.$$("courses").removeView("noCourse");


           // force react to redraw
           
        } else {
/*          let errors = "";
          response.error.forEach(error => {
            errors += error.description
          });*/
        }
      })
      .catch(error => console.error('Error:', error));

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
      </div>
      );
  }
}
export default Courses;

