import LWAT from './index.js';

let lwat = new LWAT();

// Test GetSetting()
lwat.post("http://localhost:8080/Account/Login",
			{"Email" : "admin@example.com",
			 "Password" : "password",
			 "returnUrl" : ""
	}).then((response) => { 
		lwat.assert(response.success,true,"Login as Admin.");
	   }).then(() => {
	   		lwat.post("http://localhost:8080/Setting/GetSetting",{
	   			"id" : "SMTP_HOST"
	   		}).then((response) => {
	   			lwat.assert(response.success,true,"After Login: Got a Setting");
	   			lwat.assert(response.data[0]["id"],"SMTP_HOST","After Login: Setting Name: " + response.data[0]["id"]);
	   		});
	   });