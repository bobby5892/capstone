import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import Webix from '../webix';

class CourseContent extends Component {
    constructor(props) {
        super(props);
        this.state = {
          scurrentUser: props.currentUser,
          role: props.role
        }
     
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
        }, window.webix.ui.view)
    }

    render(){
       
    // Now lets check for returns
    //console.log(window.webix.$$("loginForm").elements);
                   
         
    // <Webix ui={ui} data={data}/>
         return(
            <div>
               Course Content
            </div>
        );
      }
}
export default CourseContent;