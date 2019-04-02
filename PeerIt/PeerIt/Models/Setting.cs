using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PeerIt.Models
{
    public class Setting
    {

    	[Required(ErrorMessage ="Must be less than 30 characters"),StringLength(30, ErrorMessage = "Must be less than 30 characters")]    	
    	public string ID {get; set;}

    	[StringLength(1000, ErrorMessage = "Must be less than 1000 characters")]                    
    	public string StringValue {get;set;}

    	[StringLength(20, ErrorMessage = "Must be less than 20 characters")]                    
    	public int NumericValue{get;set;}
        public Setting()
        {

        }
    }
}
