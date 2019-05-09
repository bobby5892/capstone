import LWAT from './index.js';

let lwat = new LWAT();


//*Comment Unit Tests */

// Testing Logout
let lwat = new LWAT();
lwat.get("http://localhost:8080/Account/Logout").then((response) => { 
	lwat.assert(response.success,true,"Check That logout works");
});

// Testing Login
lwat.post("http://localhost:8080/Account/Login",{ 
			"Email" : "admin@example.com",
			"Password" : "password",
			"returnUrl" : ""
	}).then((response) => { 
		lwat.assert(response.success,true,"Check That Login works");
});

//Testing GetCommentsByUserId
lwat.post("http://localhost:8080/Account/Login",{ 
			"Email" : "admin@example.com",
			"Password" : "password",
			"returnUrl" : ""
	}).then((response) => {
        lwat.get("http://localhost:8080/Comment/GetCommentsByUserId").then((res) => {
            lwat.assert(res.Error[0].Name,"No Comments","Check that user with no comments returns 'no comments");
        }).catch(console.log("Promise rejection at GetCommentsByUserId"));
    }).catch(console.log("Promise rejected at login"));