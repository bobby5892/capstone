const Curl = require('node-libcurl').Curl;

class LWAT {
	constructor(){
		console.log("Loaded Lightweight API Testing - alpha ");
		this.curl = (require( 'curl-request' ))();
		this.userAgent = 'Mozilla/5.0 (Macintosh; Intel Mac OS X 10_12_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36';
		this.headers =  ['user-agent: ' + this.userAgent];
		this.cookiesEnabled = true;
		this.colors = require('colors');
	}
	get(url,params=null){

		this.curl = new (require( 'curl-request' ))();
		this.curl.setHeaders(this.headers);
		this.curl._setUrl(url);
		this.curl.curl.setOpt(Curl.option.CUSTOMREQUEST,'GET');
		this.curl.setBody(params);
		return this.curlReq();
	}
	post(url,params=null){
		this.curl = new (require( 'curl-request' ))();
		this.curl.setHeaders(this.headers);
		this.curl._setUrl(url);
		this.curl.setBody(params);
		this.curl.curl.setOpt(Curl.option.CUSTOMREQUEST,'POST');
		return this.curlReq();	
	}
	delete(url,params=null){
		this.curl = new (require( 'curl-request' ))();
		this.curl.setHeaders(this.headers);
		this.curl._setUrl(url);
		this.curl.setBody(params);
		this.curl.curl.setOpt(Curl.option.CUSTOMREQUEST,'DELETE');

		return this.curlReq();
	}
	put(url,params=null){
		this.curl = new (require( 'curl-request' ))();
		this.curl.setHeaders(this.headers);
		this.curl._setUrl(url);
		this.curl.setBody(params);
		this.curl.curl.setOpt(Curl.option.CUSTOMREQUEST,'PUT');
		return this.curlReq();
	}
	setUserAgent(agent){
			this.userAgent = agent;
	}
	setCookies(value){
		this.cookiesEnabled = value;
	}
	curlReq(){
		let data = "";
			return (this.curlSub()).then( function(result){
				return result;
			});
	}
	curlSub(){
		if(this.cookiesEnabled){
			// Write
			this.curl.curl.setOpt(Curl.option.COOKIEJAR,'cookies.txt');
			//Read
			this.curl.curl.setOpt(Curl.option.COOKIEFILE,'cookies.txt');
			
		}
		try{
			return (this.curl._submit())
			.then( (res) => {
				return JSON.parse(res.body);
			});
			
		}
		catch(error){
			return error;
		}
	}
	assert(value1,value2,description){
		try{
			if(value1 === value2){
				console.log(this.colors.green("passed ") +  description);
			}
			else{
				console.log(this.colors.red("failed ") +  description);	
			}
		}
		catch(error){
			console.log(this.colors.red("failed ") + description + " e"  ) ;
		}
	}
		
}
module.exports = LWAT;