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
      Courses:null
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
}
loadCourses() {

    console.log('made it to course accord' + JSON.stringify("Scope of loadCourse" + JSON.stringify(this)));
    let accord = [];
    fetch("Course/GetCourses", {
      method: 'GET', // or 'PUT'
      headers: {
        'Content-Type': 'application/json'
      },
      credentials: "include",
      mode: "cors"
    }).then(res => res.json())
      .then(response => {
        if (response.success) {
          //data = response.data;
          console.log(response.data)
          response.data.forEach(element => {
            console.log("Adding Element: " + element.name);

            accord.push({
              view: "accordionitem",
              header: element.name,
              id: element.id,
              padding: 0,
              //headerAlt:"Pane 2 Closed",
              body: "This is " + element.name, //just text
              collapsed: true
            });
          })
          console.log(accord);  
           this.setState({'Courses': accord});
           console.log("State after Fetch: " + JSON.stringify(this.state));
          // window.webix.$$("newCourseForm").close();
        } else {
          let errors = "";
          response.error.forEach(error => {
            errors += error.description
          });
        }
      })
      .catch(error => console.error('Error:', error));
    //return ({
      // view: "accordionitem",
      // header: data,
      // padding: 0,
      // //headerAlt:"Pane 2 Closed",
      // body: "This is Pane 2 body", //just text
      // collapsed: true
    //});
  }
  renderCourses(){
    //let scope = this;
  /*  if(this.state.Courses === null){
      console.log("RenderCourses: state was empty");
      console.log("State Courses"+ JSON.stringify(this.state.Courses));
      return([{
      view: "accordionitem",
      header: "No Courses",
      padding: 0,
      //headerAlt:"Pane 2 Closed",
      body: "This is Pane 2 body", //just text
      collapsed: true}])
    }
    else {
      /*var accorditem =[];
      forEach(item in this.state.Courses){
        accorditem.push(item);
      };
      console.log("state was good");
      console.log("State after Set: " + JSON.stringify(this.state));
      //return JSON.stringify(this.state.Courses );
      return this.state.Courses;
      
  }*/


    if(this.state.Courses != null){
    
      return this.state.Courses
      ;
    }
    else{
      return([{
        view: "accordionitem",
        header: "No Courses",
        padding: 0,
        //headerAlt:"Pane 2 Closed",
        body: "This is Pane 2 body", //just text
        collapsed: true}]);
    }
  }  
  render() {
    return(<div>
      
      
    </div>);
  /*  let ui = null;
    let data = null;
     

     return(
      <div>
        <Webix ui={ui} data={data}/>
      </div>
      );*/
  }
}
export default Courses;

