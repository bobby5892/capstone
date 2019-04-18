using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace UnitTests.WebTests
{
   public class WebTest
    {
        public string Request(string url,string requestType, string postData)
        {
            // Create a request using a URL that can receive a post.   
            WebRequest request = WebRequest.Create(url);
            // Set the Method property of the request to POST.  
            request.Method = requestType;

            // Create POST data and convert it to a byte array.  
            
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            // Set the ContentType property of the WebRequest.  
            request.ContentType = "application/x-www-form-urlencoded";
            // Set the ContentLength property of the WebRequest.  
            request.ContentLength = byteArray.Length;

            // Get the request stream.  
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.  
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.  
            dataStream.Close();

            // Get the response.  
            WebResponse response = request.GetResponse();
            // Display the status.  
            // Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            string responseFromServer;
            // Get the stream containing content returned by the server.  
            // The using block ensures the stream is automatically closed.
            using (dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.  
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.  
                 responseFromServer = reader.ReadToEnd();
                // Display the content.  
               // Console.WriteLine(responseFromServer);
            }

            // Close the response.  
            response.Close();
            return responseFromServer;
        }
    }    
}
