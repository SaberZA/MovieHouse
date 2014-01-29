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
using SalesServiceHost.Common;

namespace MovieHouse.Logic
{
    public class MovieRequest
    {
        private string ApiKey;
        private string RawMovieRequestUrlString;
        public string QueryString { get; set; }
        public string JsonMovieResponse { get; set; }
        public RootObject JsonRootObject { get; set; }
        public List<Movie> Movies { get; set; } 

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
            var result = GetJsonResponse(UrlMovieRequestString);
            JsonRootObject = null;
            bool emptyJson = false;
            try
            {
                JsonRootObject = JsonConvert.DeserializeObject<RootObject>(result);
            }
            catch (Exception ex)
            {
                emptyJson = JsonRootObject == null;
            }

            if (!emptyJson)
            {
                this.JsonMovieResponse = result;
                this.Movies = JsonRootObject.movies;
                foreach (Movie movie in Movies)
                {
                    foreach (object jsonObject in movie.abridged_cast)
                    {
                        CastMember castMember = JsonConvert.DeserializeObject<CastMember>(jsonObject.ToString());
                        movie.Cast.Add(castMember);
                    }
                }
            }
            else
            {
                this.JsonMovieResponse = result;
                this.Movies = new List<Movie>();
            }
            
        }

        private static string GetJsonResponse(string url)
        {
            using (RtWebClient client = new RtWebClient())
            {
                return client.DownloadString(url);
            }
        }

    }
}
