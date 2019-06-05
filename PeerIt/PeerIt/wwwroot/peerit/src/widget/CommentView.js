import React, { Component } from 'react';
class CommentView extends Component {
    constructor(props) {
        super(props);
        this.state ={
            currentUser: props.currentUser,
            role: props.role,
            assignmentId : props.assignmentId
        };
       if(this.state.assignmentId != null){
           this.GetComments();
       } 
    }
    componentWillReceiveProps(props){
        this.setState(props);
        this.GetComments();
    }
    GetComments(){

        fetch("Comment/GetCommentsByAssignmentId?studentAssignmentId=" + this.state.assignmentId, {
            method: 'Get', // or 'PUT'

            headers: {
                'Content-Type': 'application/json'
            },
            credentials: "include",
            mode: "no-cors"
        }).then(res => res.json())
            .then(response => {
                if (response.success) {
                    this.setState({ "data": 0 });
                   // this.redrawAll();
                } else {
                    //   let errors = "";
                    //response.error.forEach(error => {
                    //  //   errors += error.description
                    //    });

                }
                console.log('this is the comments responce: '+JSON.stringify(response));

            })
            .catch(error => console.error('Error:', error));

    }

    renderComments(){
        //this.GetComments();
    }
    render() {

        return (<div id="CommentView">
            Comment views go here
            {this.renderComments()}
        </div>)
    }

}
    export default CommentView;