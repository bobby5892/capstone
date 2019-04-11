import React, { Component } from 'react';
import ReactDOM from 'react-dom';

import * as webix from "../webix/codebase/webix.js";


//const webix = require("../webix/codebase/webix.js");
//import ''
import '../webix/codebase/webix.css';

class Portal extends Component {
  render() {
    return (
      <div ref="root"></div>
    );
  }

  setWebixData(data){
    const ui = this.ui;
    if (ui.setValues)
      ui.setValues(data);
    else if (ui.parse)
      ui.parse(data)
    else if (ui.setValue)
      ui.setValue(data); 
  }

  componentWillUnmount(){
    this.ui.destructor();
    this.ui = null;
  }

  componentWillUpdate(props){
    if (props.data)
      this.setWebixData(props.data);
    if (props.select)
      this.select(props.select);
  }

  componentDidMount(){
  	this.ui = window.webix.ui(
  	  this.props.ui, 
  	  ReactDOM.findDOMNode(this.refs.root)
	  );

    this.componentWillUpdate(this.props);
  }
  
}
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
export default Portal;