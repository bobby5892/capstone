import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import Webix from '../webix';

class CourseAssignmentSettings extends Component {
    constructor(props) {
        super(props);
        
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
        let ui = {
            type:"tabview",
            id:"cAssignmentSettings",
            height: 750,
            width: 750,
            rows:[
                {
                    type:"space", 
                    padding:0, 
                    responsive:"a1", 
                    margin:0,
                    cols:[
                        {},
                        { 
                            view:"form", 
                            id:"loginForm",
                            elements:[
                                { 
                                    type:"header",
                                    template:"Please Login"
                                },
                                { 
                                     
                                    view:"text",
                                    labelAlign:"top",
                                    labelPosition:"top", 
                                    name:"emailAddress",
                                    label:"Email Address",
                                    validate:"isNotEmpty",
                                    validateEvent:"key",
                                    value:"" ,
                                  
                                },
                                {
                                    view:"text", 
                                    labelPosition:"top",
                                    type:"password", 
                                    name:"password", 
                                    label:"Password",
                                    validate:"isNotEmpty", 
                                    validateEvent:"key",
                                    value:""
                                },
                                { 
                                    view:"label", 
                                    name:"passwordErrorLabel", 
                                    label:"" 
                                },
                                {}, 
                                { view:"button", label: 'login', click:this.doLogin.bind(this), hotkey:"enter"}
                            ],
                            width:500,
                            height:500,
                            gravity:0.3
                        },
                        {}
                    ]
                }
            ]
        };
    
                    // Now lets check for returns
                   //console.log(window.webix.$$("loginForm").elements);
                   
         
    
         return(
            <div>
                <Webix ui={ui} data={data}/>
            </div>
        );
      }
}