using Microsoft.AspNetCore.Http;
using PeerIt.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PeerIt.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class PFile
    {
        /// <summary>
        /// ID
        public string ID { get; set; }
        public string Name { get; set; }

        public string Ext { get; set; }
        /// <summary>
        /// Current User
        /// </summary>
        public AppUser AppUser { get; set; }



        ///
        public PFile() { }
        ///
        public PFile(string id, string name, string extension, AppUser user)

        {
            ID = id;
            Name = name;
            Ext = extension;
            AppUser = user;
        }

    }
}
