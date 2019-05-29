import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import Webix from '../webix';

class ShowStudentAssignment extends Component {
 constructor(props) {

    super(props);
    this.state = {
      data: null,
      currentUser: props.currentUser,
      role: props.role,
      viewingCourse: props.viewingCourse,
      viewingAssignment: props.viewingAssignment,
    };
  }
  componentWillReceiveProps(props) {
    this.setState(props);
  }
  renderStudentAssignmentInfo(){
  		return (
  			<div>
  			<h2>Student Assignment</h2>
  			<div>{this.state.viewingCourse}</div>
  					<div>{JSON.stringify(this.state.viewingAssignment.pFile.name)}</div>
  			</div>
  			);	
  }
  renderStudentAssignmentInfoWindow(){
    window.webix.ui({
      view:"window",
      id:"uploadReviewWindow",
      width: 900,
      height: 600,
      move:true,
      position:"center",
      head:{
          type:"space",
          cols:[
              { view:"label", label: "Upload a Review" },
              {
                view:"button", label:"Close", width:70,left:250,
                click:function(){
                  //scope.setState({"editUser" : null });
                  window.webix.$$("uploadReviewWindow").close();
                } 
              }
           ]   
      },
      body:{
          type:"space",
          rows:[
              { 
                view:"form", 
                id:"uploadReviewForm",
                width:900,
                elements:[
                  { view:"label", label:"Upload your review form here: ", name:"", labelWidth:100,value:"" },
                  { view:"uploader",inputName:"files",upload:"/Review/UploadReview" ,urlData:{studentAssignmentId:35} ,name:"ReviewFile",value:"Click here to upload your review file"},
                  { view:"text", label:"Course", name:"Course", labelWidth:100, value:""}, 
                  { view:"text", label:"Assignment", name:"Assignment", labelWidth:100, value:""},
                    
                    // { margin:5, cols:[
                    //     { view:"button", value:"Upload" , type:"form", click:function(){
                    //       scope.uploadTheReviewDoc();
                    //     }}
                    // ]}
                ],
                rules:{
                    "Email": window.webix.rules.isEmail,
                    "LastName": window.webix.rules.isNotEmpty,
                    "FirstName": window.webix.rules.isNotEmpty,
                    "Password" :  window.webix.rules.isNotEmpty
                }
              }
          ]
      }
}).show();
  }
  render() {
  	let data = null;
	let ui = null;

    return (
      <div id="ShowStudentAssignment">
      	
     {this.renderStudentAssignmentInfoWindow()}

      </div>
    );
  }
}
// <Webix ui={ui} data={data} />
export default ShowStudentAssignment;