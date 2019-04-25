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
        /// <summary>
        /// Extinsion
        /// </summary>
        public string Ext { get; set; }
        /// <summary>
        /// Current User
        /// </summary>
        public AppUser AppUser { get; set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="extension"></param>
        /// <param name="user"></param>
        public PFile(string id, string extension, AppUser user)
        {
            ID = id;
            Ext = extension;
            AppUser = user;
        }

    }
}
