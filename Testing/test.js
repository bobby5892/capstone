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

lwat.post("http://localhost:8080/Account/Login",{ 
			"Email" : "admin@example.com",
			"Password" : "password",
			"returnUrl" : ""
	}).then((response) => { 
		lwat.get("http://localhost:8080/Account/GetCurrentUserRole").then((res) => { 
			lwat.assert(res.data[0].role,"Administrator","Check That current role is admin");
	});
});


//\/\/\/\/\/\/\/\/\/\/\/\/\/\--Comment Controller Testing--/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/
/*lwat.get("http://localhost:8080/Comment/GetCommentByUserId").then((response) => { 
		lwat.assert(response.data[0].role,"Administrator","Check That current role is admin");
});
*/

