import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import Webix from '../webix';
import FormView from './FormView';
 
class Portal extends Component {

	constructor(props) {
	    super(props);
	    this.state = {
	    	data : null
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
		}, window.webix.ui.view)
	}
	getForm(){
 		 var subApp = <FormView></FormView>;

	  return {
	    view:"form", width:500, elements:[
	      { view:"text", name:"Company", label:"Name", placeholder:"Type your full name here"},
	      { type:"header", template:"Owner" },
	      {
	        view:"react", height: 220, app:subApp
	      },
	      { view:"label", label:"the above form is a separate React App inside of Webix UI" }
	    ]
	  };
	}
	render(){
		return(
			<div>
		  		  <Webix ui={this.getForm()} data={this.state.data}/>
		 	 </div>
	 	 );
	}
}

//const Portal = ({ data, save }) => (
  
//)
export default Portal;
