import React, { Component } from 'react';
import Webix from '../webix';
class CommentView extends Component {
    constructor(props) {
        super(props);
        this.state = {
            currentUser: props.currentUser,
            role: props.role,
            assignmentId: props.assignmentId,
            comments:null
        };
        if (this.state.assignmentId != null) {
            //this.GetComments();
        }
    }
    componentWillReceiveProps(props) {
        this.setState(props);
        //this.GetComments();
    }
    GetComments() {

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
                    this.setState({ "comments": response.data });
                    console.log(this.state)
                    // this.redrawAll();
                } else {
                    //   let errors = "";
                    //response.error.forEach(error => {
                    //  //   errors += error.description
                    //    });

                }
                console.log('this is the comments responce: ' + JSON.stringify(response));

            })
            .catch(error => console.error('Error:', error));

    }

    renderComments() {

        let ui = ({
				view:"datatable",
				columns:[
                    { id:"firstName",map:"#fK_APP_USER.firstName#",	header:"Name", css:"",width:100},
                    { id:"content",	header:"", css:"",width:550},
                    
                    
					
				],
				autoheight:true,
				autowidth:true,
                //data: JSON.stringify(this.state.comments)
                url:"Comment/GetCommentsByAssignmentId?studentAssignmentId=" + this.state.assignmentId
			});	

        return <Webix ui={ui} />

    }
    render() {

        return (<div id="CommentView">
      
            {this.renderComments()}
        </div>)
    }

}
export default CommentView;