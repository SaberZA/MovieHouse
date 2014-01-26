using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MovieHouse.Logic.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MovieHouse.Logic
{
    public class MovieRequest
    {
        private string ApiKey;
        private string RawMovieRequestUrlString;
        public string QueryString { get; set; }
        public string JsonMovieResponse { get; set; }

        public MovieRequest(string queryString)
        {
            this.ApiKey = Resources.ResourceManager.GetString("RtApiKey");
            this.RawMovieRequestUrlString = Resources.ResourceManager.GetString("RtMovieQuery");
            this.QueryString = queryString;
        }

        public string UrlEncodedQueryString
        {
            get { return QueryString.Replace(" ", "+"); }
        }

        public string UrlMovieRequestString
        {
            get
            {
                var urlMovieRequestString = RawMovieRequestUrlString.Replace("[apiKey]", this.ApiKey);
                urlMovieRequestString = urlMovieRequestString.Replace("[searchQuery]", this.UrlEncodedQueryString);
                return urlMovieRequestString;
            }
        }

        public void DownloadJsonMovieResponse()
        {
            //WebRequest request = WebRequest.Create(UrlMovieRequestString);
            //WebResponse response = request.GetResponse();
            //Stream dataStream = response.GetResponseStream();
            //StreamReader streamReader = new StreamReader(dataStream);
            //var json = streamReader.ReadToEnd();

            //var result = new WebClient().DownloadData(UrlMovieRequestString);
            //var result = WebGetRequest.GetWebRequest(UrlMovieRequestString);
            var result = GetJsonResponse("http://stackoverflow.com/questions/17565940/converting-a-string-encoded-in-utf8-to-unicode-in-c-sharp");
            //var result = DownloadString(new WebClient(), UrlMovieRequestString, Encoding.UTF32);
            JObject json = JObject.Parse(result);
            this.JsonMovieResponse = json.ToString();
        }

        private static string GetJsonResponse(string url)
        {
            using (var client = new WebClient())
            {
                //client.Encoding = Encoding.UTF8;
                //client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-GB; rv:1.9.2.12) Gecko/20101026 Firefox/3.6.12");
                //client.Headers.Add("Accept", "*/*");
                //client.Headers.Add("Accept-Language", "en-gb,en;q=0.5");
                //client.Headers.Add("Accept-Charset", "ISO-8859-1,utf-8;q=0.7,*;q=0.7");
                return client.DownloadString(url);
            }
        }

        public String DownloadString(WebClient webClient, String address, Encoding encoding)
        {
            byte[] buffer = webClient.DownloadData(address);

            byte[] bom = encoding.GetPreamble();

            if ((0 == bom.Length) || (buffer.Length < bom.Length))
            {
                return encoding.GetString(buffer);
            }

            for (int i = 0; i < bom.Length; i++)
            {
                if (buffer[i] != bom[i])
                {
                    return encoding.GetString(buffer);
                }
            }

            return encoding.GetString(buffer, bom.Length, buffer.Length - bom.Length);
        }
    }
}
