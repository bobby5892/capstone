
import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import Webix from '../webix';

class InstructorToolbar extends Component {

	constructor(props) {
	      super(props);
	      this.state = {
	        data : null
	      };
	this.logout = props.logout;

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
		let data = null;
	    let ui = 
        {
          type:"space", id:"a1", rows:
            [
              {
                 type:"space", 
                 padding:0, 
                 responsive:"a1", 
                 height: window.innerHeight,
                 width: window.innerWidth,
                 cols:
                 [
                     { 
                        view:"list", 
                        data:["Instructor", "Reports", "Settings","Logout"],
                        ready:function(){ 
                          this.select(this.getFirstId()); 
                        },
                        click:function(a){
                        	if(a === "Logout"){
                        		//Attempt to call the logout chain
                        		this.logout();
                        }
                        }.bind(this),
                        select:true,
                        scroll:false,
                        width:200 
                     }
                ]
              }
            ]
        };
		console.log("attempted render");
		 return(<div>
        <Webix ui={ui} data={data}/>
      </div>
      );
	}
}
export default InstructorToolbar;

