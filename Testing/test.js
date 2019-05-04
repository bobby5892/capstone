import LWAT from './index.js';


//* Example Unit Tests */

// Testing if login works
let lwat = new LWAT();
lwat.get("http://localhost:8080/Account/Logout").then((response) => { 
	lwat.assert(response.success,true,"Check That logout works");
});

lwat.post("http://localhost:8080/Account/Login",{ 
			"Email" : "admin@example.com",
			"Password" : "password",
			"returnUrl" : ""
	}).then((response) => { 
		lwat.assert(response.success,true,"Check That Login works");
});



lwat.get("http://localhost:8080/Account/GetCurrentUserRole").then((response) => { 
		lwat.assert(response.data[0].role,"Administrator","Check That current role is admin");
});

//lwat.get("http://localhost:8080/" + )
