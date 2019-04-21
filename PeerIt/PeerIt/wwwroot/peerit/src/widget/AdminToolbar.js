
import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import Webix from '../webix';

class AdminToolbar extends Component {

	constructor(props) {
	      super(props);
	      this.state = {
	      	currentUser : props.currentUser,
	      	role : props.role,
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
                        data:["Admin", "Reports", "Settings","Logout"],
                        ready:function(){ 
                          let select = this.select(this.getFirstId()); 
                          console.log(select);
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
		
		return(
		 	<div>
        		<Webix ui={ui} data={data}/>
      		</div>
      	);
	}
}
export default AdminToolbar;

