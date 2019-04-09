import React, { Component } from 'react';
import ReactDOM from 'react-dom';

import * as webix from "../webix/codebase/webix.js";
//const webix = require("../webix/codebase/webix.js");
//import ''
import '../webix/codebase/webix.css';

/* 
class SliderView extends Component {
  render() {
    return (
      <div ref="root"></div>
    );
  }

  componentDidMount(){
    this.ui = window.webix.ui({
      view:"slider",
      container:ReactDOM.findDOMNode(this.refs.root)
    });
  }

  componentWillUnmount(){
    this.ui.destructor();
    this.ui = null;
  }

  shouldComponentUpdate(){
    return false;
  }
}
*/ 
//class Portal extends Component {
/* Does not work const ui = {
    view:"slider"
};
const value = 123;
 
const SliderView = () => (
  <Webix ui={ui} data={value} />
)
*/
 /* Doesn't Work
 constructor(){
    super();
    this.state = {

    }
    this.webix = this.loadWebix();
  }
  render() {
    return(
      <div id="chart">blarg</div>
        
      
    );
  }
  loadWebix(){
    const container = window.getElementById("chart");
    let tableConfig = {
       data: ['test','test']
    }
    tableConfig.view = "datatable";
    this._table = webix.ui(tableConfig, container);
  }
  */

//}
export default SliderView;