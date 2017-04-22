using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTeste
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var userName = "qwe@qwe.qwe";
                var password = "qweqweQ1!";

                WebRequest requestToken = WebRequest.Create("http://localhost:52909/Token");
                requestToken.ContentType = "application/x-www-form-urlencoded";
                requestToken.Method = "POST";
                string postData = $"UserName={userName}&Password={password}&Grant_Type=password";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                requestToken.ContentLength = byteArray.Length;
                Stream dataStream2 = requestToken.GetRequestStream();
                dataStream2.Write(byteArray, 0, byteArray.Length);
                dataStream2.Close();
                WebResponse response2 = requestToken.GetResponse(); 
                dataStream2 = response2.GetResponseStream();
                StreamReader reader2 = new StreamReader(dataStream2);
                string responseFromServer2 = reader2.ReadToEnd();

                JToken jtoken = JObject.Parse(responseFromServer2);
                var token = jtoken.SelectToken("access_token");

                //Console.Write(token);

                reader2.Close();
                dataStream2.Close();
                response2.Close();

                WebRequest request = WebRequest.Create("http://localhost:52909/api/values");
                request.Headers["Authorization"] = $"Bearer {token}";
                request.Credentials = CredentialCache.DefaultCredentials;
                WebResponse response = request.GetResponse();
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                Console.WriteLine(responseFromServer);
                reader.Close();
                response.Close();
            }
            catch (Exception e)
            {
                Console.Write(e);
            }
            Console.ReadKey();

        }
    }
}
