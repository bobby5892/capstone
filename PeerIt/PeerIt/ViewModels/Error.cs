using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeerIt.ViewModels
{
    public class Error
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }

        public Error()
        {

        }
        public Error (string Name, string Description)
        {
            this.Name = Name;
            this.Description = Description;
            this.Timestamp = DateTime.Now;
        }

    }
}
