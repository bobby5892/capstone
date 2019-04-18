class UnitTestLib{
	constructor(){
		this.console = document.getElementById("console");
			this.consoleLog("Loaded Unit Test Libs");
				this.testLogin();
	}	
	testLogin(){
		this.consoleLog("Running Test Login");
		let response = this.requestBase('localhost:8080/Account/Login','GET',null);
		this.consoleLog(response);
		if(response.success == true){
				this.consoleLog("Passed");
		}
		else{
				this.consoleLog("Failed");
		}		
	}
	consoleLog(data){
		this.console.value = data + this.console.value;
	}
	requestBase(url,type,fields){

// Default options are marked with *
 		 if(fields != null){
	  			url = url + "?" + fields;
 			}
	 
		    return fetch(url, {
		        method: type, // *GET, POST, PUT, DELETE, etc.
		        mode: "cors", // no-cors, cors, *same-origin
		        cache: "no-cache", // *default, no-cache, reload, force-cache, only-if-cached
		        credentials: "same-origin", // include, *same-origin, omit
		        headers: {
		            //
		             "Content-Type": "application/x-www-form-urlencoded",
		        },
		        redirect: "follow", // manual, *follow, error
		        referrer: "no-referrer" // no-referrer, *client
		        //body: JSON.stringify(data), // body data type must match "Content-Type" header
		    })
		    .then(response => response.json()); // parses JSON response into native Javascript objects 
		
	}

}
let unitTestLib;
window.addEventListener("load", () =>  {unitTestLib = new UnitTestLib();});