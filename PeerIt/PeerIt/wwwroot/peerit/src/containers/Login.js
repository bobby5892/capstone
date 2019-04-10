// example of custom component with Webix UI inside
// this one is a static view, not linked to the React data store

import React from 'react';
import Webix from '../webix';

 
function getForm(save){
  return {
    view:"form", width:400, elements:[
      
      { view:"text", name:"email", label:"Email"},
      { view:"password", name:"password", label:"password"},
      { cols:[
        {}, { view:"button", value:"Save", click:function(){
          if (save)
            save(this.getFormView().getValues());
        }}
      ]}
    ]
  };
}
const LoginForm = ({ data, save }) => (
  <div>
  
  </div>
)
export default LoginForm;
