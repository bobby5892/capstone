import LWAT from './index.js';


//* Example Unit Tests */

// // Testing Logout
let lwat = new LWAT();
//  lwat.get("http://localhost:8080/Account/Logout").then((response) => { 
//  	lwat.assert(response.success,true,"Check That logout works");
//  });

// // Testing Login
// lwat.post("http://localhost:8080/Account/Login",{ 
// 			"Email" : "admin@example.com",
// 			"Password" : "password",
// 			"returnUrl" : ""
// 	}).then((response) => { 
// 		lwat.assert(response.success,true,"Check That Login works");
// });

// // Testing GetCurrentUserRole. It must log in first before it attempts getting the role.
// lwat.post("http://localhost:8080/Account/Login",{ 
// 			"Email" : "admin@example.com",
// 			"Password" : "password",
// 			"returnUrl" : ""
// 	}).then((response) => { 
// 		lwat.get("http://localhost:8080/Account/GetCurrentUserRole").then((res) => { 
// 			//console.log(JSON.stringify(res.data[0].role));
// 			lwat.assert(res.data[0].role,"Administrator","Check That current role is admin");
// 	}).catch(console.log("Promise Rejection at GetCurrentUserRole portion of User Role Getter."));
// }).catch(console.log("Promise Rejection at login portion of Current User Role Getter."));


///\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/Comment Controller Tests\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\
//Testing GetCommentsByUserId with a user that has no comments
lwat.post("http://localhost:8080/Account/Login",{ 
			"Email" : "admin@example.com",
			"Password" : "password",
			"returnUrl" : ""
	}).then((response) => {
		//console.log(JSON.stringify(response));
        lwat.get("http://localhost:8080/Comment/GetCommentsByUserId").then((res) => {
		   lwat.assert(res.error[0].name,"No Comments","Check that user with no comments returns 'no comments'");
        });
	});
	
//Testing GetCommentsByAssignmentId with ivalid assignment id
lwat.post("http://localhost:8080/Account/Login",{
	"Email":"admin@example.com",
	"Password":"password",
	"returnUrl":""
}).then((response) => {
	lwat.get("http://localhost:8080/Comment/GetCommentsByAssignmentId?assignmentId:4").then((res)=>{	
	lwat.assert(res.error[0].name,"Null Assignment at Id","Null Assignment at Id");
	});
});