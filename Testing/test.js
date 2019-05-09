import LWAT from './index.js';


//* Example Unit Tests */

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

// Testing GetCurrentUserRole. It must log in first before it attempts getting the role.
lwat.post("http://localhost:8080/Account/Login",{ 
			"Email" : "admin@example.com",
			"Password" : "password",
			"returnUrl" : ""
	}).then((response) => { 
		lwat.get("http://localhost:8080/Account/GetCurrentUserRole").then((res) => { 
			//console.log(JSON.stringify(res.data[0].role));
			lwat.assert(res.data[0].role,"Administrator","Check That current role is admin");
	}).catch(console.log("Promise Rejection at GetCurrentUserRole portion of User Role Getter."));
}).catch(console.log("Promise Rejection at login portion of Current User Role Getter."));