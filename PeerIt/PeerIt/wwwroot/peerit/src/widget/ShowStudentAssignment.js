import React, { Component } from 'react';
import renderHTML from 'react-render-html';
import ReactDOM from 'react-dom';
import Webix from '../webix';
import { format } from 'url';
import CommentForm from './CommentForm.js';
import CommentView from './CommentView.js';
import { INSPECT_MAX_BYTES } from 'buffer';


class ShowStudentAssignment extends Component {
  constructor(props) {
    super(props);
    this.state = {
      errorMsg: null,
      assignment: null,
      studentAssignmentPfileId: null,
      studentAssignmentId: null,
      listOfReviews: null,
      reviewErrorMsg: null,
      reviewPFileId:null,
      currentUser: props.currentUser,
      role: props.role,
      viewingCourse: props.viewingCourse,
      viewingAssignment: props.viewingAssignment,
      downloadLink: props.buildAssignmentLink,
      viewOtherStudent: props.viewOtherStudent,
      showCommentForm: false
     
    };
    this.uploadReview.bind(this);
    this.renderStudentAssignmentReviewsDataTable.bind(this);
    this.fetchStudentAssignmentByCourseAssignmentAndUser.bind(this);
    this.fetchAllReviewsForTheStudenAssignmentSubmission.bind(this);
    this.renderUploadStudentAssignmentWindow.bind(this);
    this.renderAssignmentReviewButton.bind(this);
    this.getStudentAssignmentSubmissionDetails(props);
  }
  componentWillReceiveProps(props) {
    this.setState(props);
  }

  //Functions that fetch the data
  fetchStudentAssignmentByCourseAssignmentAndUser(props) {
    fetch("/StudentAssignment/GetStudentAssignmentsByCourseAssignmentAndUser?courseAssignmentId=" + this.state.viewingAssignment.id, {
      method: 'GET',
      headers: { 'Content-Type': 'application/json' },
      credentials: "include",
      mode: "no-cors"
    }).then(response => response.json())
      .then(response => {
        console.log("THE RESPONSE = " + JSON.stringify(response));
        if (response.success) {
          if (response.data.length > 0) {
            this.setState({
              assignment: response.data[0].fK_PFile.name,
              assignmentID: response.data[0].fK_PFile.id,
              studentAssignmentId: response.data[0].id,
              studentAssignmentPfileId: response.data[0].fK_PFile.id,
              
            });
            this.fetchAllReviewsForTheStudenAssignmentSubmission();
          }
        } else {
          this.setState({ errorMsg: response.error[0].description });
        }
      })
  }

  fetchAllReviewsForTheStudenAssignmentSubmission() {
    fetch("Review/GetReviewsByStudentAssignmentId?studentAssignmentId=" + this.state.studentAssignmentId, {
      method: 'GET',
      headers: { 'Content-Type': 'application/json' },
      credentials: "include",
      mode: "no-cors"
    }).then(response => response.json())
      .then(response => {
        let listOfReviews = [];
        if (response.success) {
          for (let i = 0; i < response.data.length; i++) {
            listOfReviews[i] = response.data[i];
          }
        }
        else {
          this.setState({ reviewErrorMsg: response.error[0] });
        }
      })
  }
  getStudentAssignmentSubmissionDetails(props) {
    this.fetchStudentAssignmentByCourseAssignmentAndUser(props);

  }
  //render the buttons
  renderAssignmentReviewButton() {
    let ui = {
      view: "button",
      id: "uploadReviewButton",
      value: "Upload Review",
      css: "webix_primary",
      inputWidth: 175,
      click: function () {
        this.uploadReview();
      }.bind(this)
    };
    return <Webix ui={ui} data={null} />
  }
  renderUploadStudentAssignmentButton() {
    let ui = {
      view: "button",
      id: "uploadStudentAssignmentButton",
      value: "Upload Assignment",
      css: "webix_primary",
      inputWidth: 175,
      click: function () {
        this.renderUploadStudentAssignmentWindow();
      }.bind(this)
    };
    return <Webix ui={ui} data={null} />
  }
  renderStudentAssignmentReviewsDataTable() {
    if (this.state.studentAssignmentId != null) {
      let reviewDataTable = {
        view: "datatable",
        id:"studentReviewsDataTable",
        autoheight:true,
        columns: [
          { id: "reviewName", map: "#fK_PFile.name#", header: "Review File Name", width: 125 },
          { id: "firstName", map: "#fK_PFile.appUser.firstName#", header: "First Name", width: 125 },
          { id: "lastName", map: "#fK_PFile.appUser.lastName#", header: "Last Name", width: 125 },
          { id:"download",css:"myaction", header: { css:"myaction",text:"download",width:150},template:"Download"}
        ],
        url: "Review/GetReviewsByStudentAssignmentId?studentAssignmentId="+this.state.studentAssignmentId,
        onClick:{ 
          "myaction" : function(arg,t,e) {
            console.log("Clicked On: " + JSON.stringify(window.webix.$$("studentReviewsDataTable").getItem(t)));
            let theReview = window.webix.$$("studentReviewsDataTable").getItem(t);
            fetch("PFile/Download?pFileId="+theReview.fK_PFile.id);
        }
      },
      };

      return <Webix ui={reviewDataTable} />
    }
  }
  //Render the forms
  renderUploadStudentAssignmentWindow(props) {
    let scope = this;
    var newWindow = window.webix.ui({
      view: "window",
      id: "uploadStudentAssignmentWindow",
      width: 600,
      //height: 600,
      move: true,
      position: "center",
      head: {
        type: "space",
        cols: [
          { view: "label", label: "Upload a student Assignment" },
          {
            view: "button", label: "Close",
            width: 70,
            left: 250,
            click: function () {
              window.webix.$$("uploadStudentAssignmentWindow").close();
            }
          }
        ]
      },
      body: {
        type: "space",
        rows: [
          {
            view: "form",
            id: "uploadStudentAssignmentForm",
            elements: [
              { view: "label", label: "Upload your student assignment here: ", name: "", labelWidth: "auto", value: "" },
              {
                view: "uploader", inputName: "files", upload: "/StudentAssignment/UploadStudentAssignment",
                id: "studentAssignmentFile", link: "mylist", value: "Upload File", autosend: false
              },
              {
                view: "list", id: "mylist", type: "uploader",
                autoheight: true, borderless: true
              },
              //{ view: "text", label: "Assignment Name", name: "Assignment_Name",labelWidth: 200,invalidMessage:"Please enter Assignment Name" },
              {
                view: "button", value: "Upload", type: "form",
                click: function (props) {
                  console.log("VIEWING ASSIGNMENT ID = " + this.state.viewingAssignment.id);
                  let validResponse = window.webix.$$("uploadStudentAssignmentForm").validate();
                  let FormVal = window.webix.$$("uploadStudentAssignmentForm").getValues();
                  window.webix.$$("studentAssignmentFile").define({
                    urlData: { courseAssignmentId: this.state.viewingAssignment.id }
                  });
                  window.webix.$$("studentAssignmentFile").send(function (response) {
                    if (response != null) {
                      window.webix.message("Success");
                    }
                    else {
                      alert("Nothing to Submit");
                    }
                  })
                }.bind(this)
              }
            ],
            rules: {
              //No rules defined yet!!!
            }
          }
        ]
      }
    }).show();
    window.webix.$$("uploadStudentAssignmentForm").setValues(
      { Course: this.state.viewingCourse }
    );
  }
  uploadReview() {
    let scope = this;
    var newWindow = window.webix.ui({
      view: "window",
      id: "uploadStudentAssignmentReviewWindow",
      width: 600,
      //height: 600,
      move: true,
      position: "center",
      head: {
        type: "space",
        cols: [
          { view: "label", label: "Upload a review for this student Assignment" },
          {
            view: "button", label: "Close",
            width: 70,
            left: 250,
            click: function () {
              window.webix.$$("uploadStudentAssignmentReviewWindow").close();
            }
          }
        ]
      },
      body: {
        type: "space",
        rows: [
          {
            view: "form",
            id: "uploadStudentAssignmentReviewForm",
            elements: [
              { view: "label", label: "Upload your review for this student assignment here: ", name: "", labelWidth: "auto", value: "" },
              {
                view: "uploader", inputName: "files", upload: "/Review/UploadReview",
                id: "reviewFile", link: "mylist", value: "Upload File", autosend: false
              },
              {
                view: "list", id: "mylist", type: "uploader",
                autoheight: true, borderless: true
              },
              {
                view: "button", value: "Upload Review", type: "form",
                click: function (props) {
                  let validResponse = window.webix.$$("uploadStudentAssignmentReviewForm").validate();
                  let FormVal = window.webix.$$("uploadStudentAssignmentReviewForm").getValues();
                  window.webix.$$("reviewFile").define({
                    urlData: { studentAssignmentId: this.state.studentAssignmentId }
                  });
                  window.webix.$$("reviewFile").send(function (response) {
                    if (response != null) {
                      window.webix.message("Succsess");
                      window.webix.$$("uploadStudentAssignmentReviewWindow").close();
                    }
                  })
                }.bind(this)
              }
            ],
            rules: {
              //No rules defined yet!!!
            }
          }
        ]
      }
    }).show();
    window.webix.$$("uploadStudentAssignmentReviewForm").setValues(
      { Course: this.state.viewingCourse }
    );
  }
  renderLink() {
    if (this.state.studentAssignmentPfileId != null) {
      console.log("render the link");
      return (<div><a href={'/PFile/Download?pFileId=' + this.state.studentAssignmentPfileId}>Download Your Assignment Submission </a></div>);
    }
    console.log("no render");
  }
  renderCommentForm() {
    if (this.state.showCommentForm) {
      return <CommentForm currentUser={this.state.currentUser} role={this.state.role} assignmentId={this.state.studentAssignmentId} />
    }
  }
  renderAddCommentButton() {
    let ui = {
      view: "button",
      id: "addComment",
      value: "Add Comment",
      css: "webix_primary",
      inputWidth: 175,
      click: function () {
        this.setState({ showCommentForm: true });
      }.bind(this)
    };
    return <Webix ui={ui} data={null} />
  }
  renderComments() {
    console.log('rendercomments called this.state.studentassignmentid = '+ this.state.studentAssignmentId)
    if (this.state.studentAssignmentID != null) {
      return <CommentView currentUser={this.state.currentUser} role={this.state.role} assignmentId={this.state.studentAssignmentID} />
    }
  }

  render() {
    console.log('this is student id :' + this.state.studentAssignmentId);
    return (
      <div id="ShowStudentAssignment" className="showStudentAss">
        <h1>Your Submission Information for {this.state.viewingAssignment.name}</h1>
        <h3>{this.state.errorMsg}</h3>
        {this.renderUploadStudentAssignmentButton()}
        {this.renderLink()}
        <h1>Reviews for your Assignment Submission</h1>
        {this.renderStudentAssignmentReviewsDataTable()}
        {this.state.listOfReviews}
        {this.state.reviewErrorMsg}
        {this.renderAssignmentReviewButton()}
        <h1>Comments for your assignment</h1>
        {this.renderAddCommentButton()}
        {this.renderCommentForm()}
        {this.renderComments()}
      </div>
    );
  }
}
export default ShowStudentAssignment;