import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import Webix from '../webix';

class CourseContent extends Component {
    constructor(props) {
        super(props);
        this.state = {
          currentUser: props.currentUser,
          role: props.role,
          viewingCourse: props.viewingCourse,
          addingStudent : false
        }
     
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
   addStudentWindow(){ 
        window.webix.ui({
                view:"window",
                id:"addStudentWindow",
                width: 500,
                height: 500,
                move:true,
                position:"center",
                head:{
                    type:"space",
                    cols:[
                        { view:"label", label: "Add Student to Course" },
                        {
                          view:"button", label:"Close", width:70,left:250,
                          click:function(){
                              this.setState({addingStudent : false});
                            window.webix.$$("addStudentWindow").close();
                          }.bind(this)
                        }
                     ]   
                },
                body:{
                    type:"space",
                    rows:[
                            { 
                              view:"form", 
                              id:"addStudentForm",
                              width:400,
                              elements:[
                                  
                                 
                                  { view:"text", label:"Email", name:"Email", labelWidth:100,invalidMessage: "Must be valid email address"},
                                 
                                  { 
                                    margin:5, 
                                    cols:[
                                          { 
                                            view:"button", 
                                            value:"Add Student" , 
                                            type:"form", 
                                            click:function(){
                                                this.addStudent();
                                            }.bind(this)
                                          }
                                        ]
                                  }
                              ]
                            }
                       ]
                }
                
            }).show();
          
   }    
   componentWillReceiveProps(props){
        this.setState(props);
   }
    render(){
        console.log("render course content");
        let ui ={
          rows: [
                    {
                        header:"Course Settings (ID: " + this.state.viewingCourse + ")", body:" " 
                    },
                    {
                      view: "tabview",
                      autoheight:true,
                      header: "Course",
                      cells: [
                        {
                          autoheight:true,
                          header: "Assignments",
                          body: {
                                autoheight:true,
                                view:"datatable", 
                                columns:[
                                    { id:"rank",    header:"",              width:50},
                                    { id:"title",   header:"Film title",    width:200},
                                    { id:"year",    header:"Released",      width:80},
                                    { id:"votes",   header:"Votes",         width:100}
                                ],
                                data: [
                                    { id:1, title:"The Shawshank Redemption", year:1994, votes:678790, rank:1},
                                    { id:2, title:"The Godfather", year:1972, votes:511495, rank:2}
                                ]
                            }
                        },
                        {
                          header: "Groups",
                          body: {
                                autoheight:true,
                                view:"datatable", 
                                columns:[
                                    { id:"rank",    header:"",              width:50},
                                    { id:"title",   header:"Film title",    width:200},
                                    { id:"year",    header:"Released",      width:80},
                                    { id:"votes",   header:"Votes",         width:100}
                                ],
                                data: [
                                    { id:1, title:"The Shawshank Redemption", year:1994, votes:678790, rank:1},
                                    { id:2, title:"The Godfather", year:1972, votes:511495, rank:2}
                                ]
                            }
                        },
                        { 
                          header: "Students",
                          body: {
                                autoheight:true,
                                rows : [
                                           {
                                                view:"button", 
                                                value:"Add Student", 
                                                css:"webix_primary", 
                                                inputWidth:100,
                                                on: {
                                                     'onItemClick' : function (i){
                                                        if(!this.state.addingStudent){
                                                            this.setState({addingStudent : true});
                                                            this.addStudentWindow();
                                                        }

                                                     }.bind(this)
                                                }
                                            },
                                            {
                                                autoheight:true,
                                                view:"datatable", 
                                                columns:[
                                                    { id:"rank",    header:"",              width:50},
                                                    { id:"firstName",   header:"First Name",    width:200},
                                                    { id:"lastName",    header:"Last Name",      width:80},
                                                  
                                                ],
                                                url:"/Course/GetStudents?courseID=" + this.state.viewingCourse

                                               /* data: [
                                                    { id:1, title:"The Shawshank Redemption", year:1994, votes:678790, rank:1},
                                                    { id:2, title:"The Godfather", year:1972, votes:511495, rank:2}
                                                ]*/
                                            }
                                        ]
                                }
                        }
                      ] 
                    }
                ]
        };
        let data = null;
    
         return(
            <div id="CourseContent">
              <Webix ui={ui} data={data}/>
             
            </div>
        );
      }
}
export default CourseContent;