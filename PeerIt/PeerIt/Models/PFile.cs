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
    public class PFile
    {
        public int ID { get; set; }
        public string Path { get; set; }
        ///
        public PFile() { }
    }
}
