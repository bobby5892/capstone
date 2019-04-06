using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeerIt.ViewModels
{
    /// <summary>
    /// JsonResponse - This view model formats all responses in a consistent form.
    /// </summary>
    /// <typeparam name="T">Type of data</typeparam>
    public class JsonResponse<T>
    {
        /// <summary>
        /// Was the request a success?
        /// </summary>
        public bool Success {
            get {
                if (this.Error.Count == 0) {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// The total number of data results included
        /// </summary>
        public int TotalResults {
            get {
                return this.Data.Count;
            }
        }

        /// <summary>
        /// The list of Errors
        /// </summary>
        public List<Error> Error { get; set; }

        /// <summary>
        /// The type of data and the list of data stored in this response
        /// </summary>
        public List<T> Data { get; set; }

        /// <summary>
        /// Default Constructor that creates new blank lists ready to add data to.
        /// </summary>
        public JsonResponse(){
            this.Error = new List<Error>();
            this.Data = new List<T>();
        }
    }
}
