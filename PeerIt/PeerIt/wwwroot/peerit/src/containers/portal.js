// example of custom component with Webix UI inside
// this one is a static view, not linked to the React data store

import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import Webix from '../webix';
import LoginForm from '../widget/LoginForm';
 
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

  render(){
    let data = null;
    let ui = {type:"space", id:"a1", rows:[{
                 type:"space", padding:0, responsive:"a1", cols:[
                     { view:"list", data:["Users", "Reports", "Settings"],
                       ready:function(){ this.select(this.getFirstId()); },
                       select:true, scroll:false, width:200 },
                     { template:"column 2", width:200 },
                     { view:"datatable", select:true, columns:[
                        { id:"title", fillspace:1 }, { id:"votes"}
                       ], data:"data",
                       minWidth:300 }
                 ]}]};
     return(
      <div>
        <Webix ui={ui} data={data}/>
      </div>
               
             
      );
  }
}

//const Portal = ({ data, save }) => (
  
//)
export default Portal;

