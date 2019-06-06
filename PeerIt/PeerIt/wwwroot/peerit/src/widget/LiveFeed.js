
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
			 <div className="arrowWithIstructions">
			 <img className="arrow" src="../../../images/arrow2.png"></img>
			 	 <div className="instructions">
					<h1>Click here to peer at some assignments.  Completely up to you though.</h1>
				 </div>
			 </div>
			 <div className="logoImage">
			 <img className="theLogo" src="../../../images/logo.png"></img>
			 </div>
      </div>
      );
	}
}
export default LiveFeed;

