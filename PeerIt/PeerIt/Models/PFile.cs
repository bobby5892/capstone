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
    ///
    public class PFile
    {
        ///
        public string ID { get; set; }
        ///
        public string Ext { get; set; }
        ///
        public AppUser AppUser { get; set; }
        ///
        public PFile() { }
        ///
        public PFile(string id, string extension, AppUser user)
        {
            ID = id;
            Ext = extension;
            AppUser = user;
        }

    }
}
