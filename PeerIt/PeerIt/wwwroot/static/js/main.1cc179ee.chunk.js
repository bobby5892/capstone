(window.webpackJsonp=window.webpackJsonp||[]).push([[0],{14:function(e,t,n){},15:function(e,t,n){},21:function(e,t,n){"use strict";n.r(t);var i=n(0),a=n.n(i),r=n(1),o=n.n(r),s=(n(14),n(2)),l=n(3),c=n(5),u=n(4),d=n(6),h=(n(15),n(16),n(20),function(e){function t(){return Object(s.a)(this,t),Object(c.a)(this,Object(u.a)(t).apply(this,arguments))}return Object(d.a)(t,e),Object(l.a)(t,[{key:"render",value:function(){return a.a.createElement("div",{ref:"root"})}},{key:"setWebixData",value:function(e){var t=this.ui;t.setValues?t.setValues(e):t.parse?t.parse(e):t.setValue&&t.setValue(e)}},{key:"componentWillUnmount",value:function(){this.ui.destructor(),this.ui=null}},{key:"componentWillUpdate",value:function(e){e.data&&this.setWebixData(e.data),e.select&&this.select(e.select)}},{key:"componentDidMount",value:function(){this.ui=window.webix.ui(this.props.ui,o.a.findDOMNode(this.refs.root)),this.componentWillUpdate(this.props)}}]),t}(i.Component)),p=(i.Component,function(e){function t(e){var n;return Object(s.a)(this,t),(n=Object(c.a)(this,Object(u.a)(t).call(this,e))).state={data:null},window.webix.protoUI({name:"react",defaults:{borderless:!0},$init:function(e){this.$ready.push(function(){o.a.render(this.config.app,this.$view)})}},window.webix.ui.view),n}return Object(d.a)(t,e),Object(l.a)(t,[{key:"render",value:function(){return a.a.createElement("div",null,a.a.createElement(h,{ui:{type:"space",id:"a1",rows:[{type:"space",padding:0,responsive:"a1",cols:[{view:"list",data:["Users","Reports","Settings"],ready:function(){this.select(this.getFirstId())},select:!0,scroll:!1,width:200},{template:"column 2",width:200},{view:"datatable",select:!0,columns:[{id:"title",fillspace:1},{id:"votes"}],data:"data",minWidth:300}]}]},data:null}))}}]),t}(i.Component)),w=function(e){function t(e){var n;return Object(s.a)(this,t),(n=Object(c.a)(this,Object(u.a)(t).call(this,e))).state={data:null},window.webix.protoUI({name:"react",defaults:{borderless:!0},$init:function(e){this.$ready.push(function(){o.a.render(this.config.app,this.$view)})}},window.webix.ui.view),n}return Object(d.a)(t,e),Object(l.a)(t,[{key:"doLogin",value:function(){alert("do login")}},{key:"render",value:function(){var e={type:"space",id:"a1",height:window.innerHeight,width:window.innerWidth,minWidth:500,minHeight:500,rows:[{type:"space",padding:50,responsive:"a1",margin:0,cols:[{view:"form",elements:[{type:"header",template:"Please Login"},{view:"text",labelAlign:"top",labelPosition:"top",name:"emailAddress",label:"Email Address",validate:"isNotEmpty",validateEvent:"key",value:""},{view:"text",labelPosition:"top",name:"password",label:"Password",validate:"isNotEmpty",validateEvent:"key",value:""},{},{view:"button",label:"login",click:this.doLogin}]}],maxWidth:600,minWidth:300}]};return a.a.createElement("div",null,a.a.createElement(h,{ui:e,data:null}))}}]),t}(i.Component),m=function(e){function t(e){var n;return Object(s.a)(this,t),(n=Object(c.a)(this,Object(u.a)(t).call(this,e))).state={currentUser:null},n}return Object(d.a)(t,e),Object(l.a)(t,[{key:"renderPortal",value:function(){if(null!=this.state.currentUser)return a.a.createElement(p,{currentUser:this.state.currentUser})}},{key:"renderLogin",value:function(){if(null==this.state.currentUser)return a.a.createElement(w,{currentUser:this.state.currentUser})}},{key:"render",value:function(){return a.a.createElement("div",{className:"appContainer"},this.renderLogin(),this.renderPortal())}}]),t}(i.Component);Boolean("localhost"===window.location.hostname||"[::1]"===window.location.hostname||window.location.hostname.match(/^127(?:\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)){3}$/));o.a.render(a.a.createElement(m,null),document.getElementById("root")),"serviceWorker"in navigator&&navigator.serviceWorker.ready.then(function(e){e.unregister()})},9:function(e,t,n){e.exports=n(21)}},[[9,1,2]]]);
//# sourceMappingURL=main.1cc179ee.chunk.js.map