
import React, { Component } from 'react';
//import Webix from '../webix';

class LiveFeed extends Component {

	constructor(props) {
	      super(props);
	      this.state = {
					currentUser : props.currentUser,
	      	role : props.role,
	        data : null
	      };
	}
	getEvents(){
		//console.log("this.state.currentUser: " + this.state.currentUser);
		fetch("/Event/GetEventsByUser", {
      method: 'GET', // or 'PUT'
     
      headers:{
        'Content-Type': 'application/json'
      },
      credentials: "include",
      mode:"no-cors"
    }).then(res => res.json())
    .then(response => {
      if(response.success){
				console.log("Responce: " + response)
        //this.updateLogin(response.data[0].emailAddress,response.data[0].role);
      }else{
        let errors = "";
        response.error.forEach( error => {
          errors += error.description
        }); 
        //window.webix.$$("loginForm").elements.passwordErrorLabel.setValue(errors);
      }

    })
    .catch(error => console.error('Error:', error));
		
		


	}
	

	render(){
		 return(<div  id="LiveFeed">
        LiveFeed
				${this.getEvents()}
      </div>
      );
	}
}
export default LiveFeed;

