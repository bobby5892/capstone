import React, { Component } from 'react';
class CommentForm extends Component {
    constructor(props) {
        super(props);
        this.state ={
            currentUser: props.currentUser,
            role: props.role,
            assignmentId : props.assignmentId
        };
    }

    renderCommentForm() {

       window.webix.ui({
            view: "window",
            id: "comment_win",
            head: {
                type: "space",
                padding: 0,
                cols: [
                    { view: "label", label: "Add Comment" },
                    {
                        view: "button", label: "Close", width: 70, left: 250,
                        click: function () {
                            window.webix.$$("comment_win").close();
                        }
                    }]
            },
            move: true,
            width: 300,
            height: 200,
            body: {
                type: "space",
                rows: [
                    {
                        view: "form",
                        id: "newCommentForm",
                        padding: 0,
                        elements: [
                            {
                                view: "textarea",
                                name: "textBox"
                            },
                            {
                                view: "button", value: "Comment", type: "form", click: function () {
                                    this.createComment()  //studentAssignmentID,comment);
                                    window.webix.$$("comment_win").close();
                                }.bind(this)
                            }
                        ]
                    }]
            }
        }).show();
        
    }
    createComment() {
        let formdata = window.webix.$$("newCommentForm").getValues();
        //console.log(formdata.textBox);

        fetch("Comment/CreateComment?studentAssignmentId=" + this.state.assignmentId + "&commentContent=" + formdata.textBox, {
            method: 'POST', // or 'PUT'

            headers: {
                'Content-Type': 'application/json'
            },
            credentials: "include",
            mode: "no-cors"
        }).then(res => res.json())
            .then(response => {
                if (response.success) {
                    this.setState({ "data": 0 });
                  //  this.redrawAll();
                  
                } else {
                    //   let errors = "";
                    //response.error.forEach(error => {
                    //  //   errors += error.description
                    //    });

                }
                console.log("attempt reload");
                 //eslint-disable-next-line
                 location.reload();

            })
            .catch(error => console.error('Error:', error));

    }


    render() {

        return (<div id="CommentForm">
            
    {this.renderCommentForm()}
        </div>)
    }
}
export default CommentForm;




