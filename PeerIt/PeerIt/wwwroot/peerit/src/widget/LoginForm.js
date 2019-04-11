// example of custom component with Webix UI inside
// this one is a static view, not linked to the React data store
import { Component } from 'react';
import ReactDOM from 'react-dom';
//import Webix from '../webix';

 class LoginForm extends Component {
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
      return {
      view:"form", width:400, elements:[
      
        { view:"text", name:"email", label:"Email"},
        { view:"text",type:"password", name:"password", label:"Password"},
        { cols:[
          {}, { view:"button", value:"Save", click:function(){
             console.log("save");
          }}
        ]}
      ]
    };
  }
}

//const Portal = ({ data, save }) => (
  
//)
export default LoginForm;
