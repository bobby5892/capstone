
import React, { Component } from 'react';
//import Webix from '../webix';

class LiveFeed extends Component {

	constructor(props) {
	      super(props);
	      this.state = {
	        data : null
	      };
  }

	render(){
		 return(<div  id="LiveFeed">
			 <div className="images">
			 <img className="arrow" src="../../../images/arrow2.png"></img>
			 <img src="../../../images/logo.png"></img>
			 </div>
			 <h1>Click here if thats what you want to do.</h1>
			 <h1>Completely up to you though.</h1>
      </div>
      );
	}
}
export default LiveFeed;

