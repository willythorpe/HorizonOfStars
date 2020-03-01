using Newtonsoft.Json;
using SharpTrooper.Entities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;

namespace API_HorizonOfStars
{
    public class Swapi
    {

        private enum HttpMethod
        {
            GET,
            POST
        }

        private string apiUrl = "http://swapi.co/api";
        private string _proxyName = null;
     
        #region Private

        private string Request(string url, HttpMethod httpMethod)
        {
            return Request(url, httpMethod, null, false);
        }

        private string Request(string url, HttpMethod httpMethod, string data, bool isProxyEnabled)
        {
            string result = string.Empty;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = httpMethod.ToString();

            if (!String.IsNullOrEmpty(_proxyName))
            {
                httpWebRequest.Proxy = new WebProxy(_proxyName, 80);
                httpWebRequest.Proxy.Credentials = CredentialCache.DefaultCredentials;
            }

            if (data != null)
            {
                byte[] bytes = UTF8Encoding.UTF8.GetBytes(data.ToString());
                httpWebRequest.ContentLength = bytes.Length;
                Stream stream = httpWebRequest.GetRequestStream();
                stream.Write(bytes, 0, bytes.Length);
                stream.Dispose();
            }

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            StreamReader reader = new StreamReader(httpWebResponse.GetResponseStream());
            result = reader.ReadToEnd();
            reader.Dispose();

            return result;
        }

        private string SerializeDictionary(Dictionary<string, string> dictionary)
        {
            StringBuilder parameters = new StringBuilder();
            foreach (KeyValuePair<string, string> keyValuePair in dictionary)
            {
                parameters.Append(keyValuePair.Key + "=" + keyValuePair.Value + "&");
            }
            return parameters.Remove(parameters.Length - 1, 1).ToString();
        }

        private T GetSingle<T>(string endpoint, Dictionary<string, string> parameters = null) where T : SharpEntity
        {
            string serializedParameters = "";
            if (parameters != null)
            {
                serializedParameters = "?" + SerializeDictionary(parameters);
            }

            return GetSingleByUrl<T>(url: string.Format("{0}{1}{2}", apiUrl, endpoint, serializedParameters));
        }

        private SharpEntityResults<T> GetMultiple<T>(string endpoint) where T : SharpEntity
        {
            return GetMultiple<T>(endpoint, null);
        }

        private SharpEntityResults<T> GetMultiple<T>(string endpoint, Dictionary<string, string> parameters) where T : SharpEntity
        {
            string serializedParameters = "";
            if (parameters != null)
            {
                serializedParameters = "?" + SerializeDictionary(parameters);
            }

            string json = Request(string.Format("{0}{1}{2}", apiUrl, endpoint, serializedParameters), HttpMethod.GET);
            SharpEntityResults<T> swapiResponse = JsonConvert.DeserializeObject<SharpEntityResults<T>>(json);
            return swapiResponse;
        }

        private NameValueCollection GetQueryParameters(string dataWithQuery)
        {
            NameValueCollection result = new NameValueCollection();
            string[] parts = dataWithQuery.Split('?');
            if (parts.Length > 0)
            {
                string QueryParameter = parts.Length > 1 ? parts[1] : parts[0];
                if (!string.IsNullOrEmpty(QueryParameter))
                {
                    string[] p = QueryParameter.Split('&');
                    foreach (string s in p)
                    {
                        if (s.IndexOf('=') > -1)
                        {
                            string[] temp = s.Split('=');
                            result.Add(temp[0], temp[1]);
                        }
                        else
                        {
                            result.Add(s, string.Empty);
                        }
                    }
                }
            }
            return result;
        }

        private SharpEntityResults<T> GetAllPaginated<T>(string entityName, string pageNumber = "1") where T : SharpEntity
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("page", pageNumber);

            SharpEntityResults<T> result = GetMultiple<T>(entityName, parameters);

            result.nextPageNo = String.IsNullOrEmpty(result.next) ? null : GetQueryParameters(result.next)["page"];
            result.previousPageNo = String.IsNullOrEmpty(result.previous) ? null : GetQueryParameters(result.previous)["page"];

            return result;
        }
        private SharpEntityResults<T> GetAllPaginated<T>(string entityName, string pageNumber = "1", string nmSpaceship = "") where T : SharpEntity
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("page", pageNumber);
            parameters.Add("search", nmSpaceship);

            SharpEntityResults<T> result = GetMultiple<T>(entityName, parameters);

            result.nextPageNo = String.IsNullOrEmpty(result.next) ? null : GetQueryParameters(result.next)["page"];
            result.previousPageNo = String.IsNullOrEmpty(result.previous) ? null : GetQueryParameters(result.previous)["page"];

            return result;
        }

        private SharpEntityResults<T> GetAllPaginated<T>(string entityName) where T : SharpEntity
        {
            SharpEntityResults<T> result = GetMultiple<T>(entityName, null);
            return result;
        }

        #endregion

        #region Public

        /// <summary>
        /// get a specific resource by url
        /// </summary>
        public T GetSingleByUrl<T>(string url) where T : SharpEntity
        {
            string json = Request(url, HttpMethod.GET);
            T swapiResponse = JsonConvert.DeserializeObject<T>(json);
            return swapiResponse;
        }
       
        // Planet
        /// <summary>
        /// get a specific planet resource
        /// </summary>
        public Planet GetPlanet(string id)
        {
            return GetSingle<Planet>("/planets/" + id);
        }

        /// <summary>
        /// get all the planet resources
        /// </summary>
        public SharpEntityResults<Planet> GetAllPlanets(string pageNumber = "1")
        {
            SharpEntityResults<Planet> result = GetAllPaginated<Planet>("/planets/", pageNumber);

            return result;
        }
          
        // Starship
        /// <summary>
        /// get a specific starship resource
        /// </summary>
        public Starship GetStarship(string id)
        {
            return GetSingle<Starship>("/starships/" + id);
        }

        /// <summary>
        /// get all the starship resources
        /// </summary>
        public SharpEntityResults<Starship> GetAllStarships(string pageNumber = "1")
        {
            SharpEntityResults<Starship> result = GetAllPaginated<Starship>("/starships/", pageNumber);

            return result;
        }

        public SharpEntityResults<Starship> GetAllStarships(string pageNumber = "1", string nmSpaceship = "")
        {
            SharpEntityResults<Starship> result = GetAllPaginated<Starship>("/starships/", pageNumber, nmSpaceship);

            return result;
        }

        public SharpEntityResults<Starship> GetAllStarships()
        {
            SharpEntityResults<Starship> result = GetAllPaginated<Starship>("/starships/");

            return result;
        }  

        #endregion

        public string GetRequestQueryStarship(string nmNameSpaceship, int nuOPPlanet)
        {
            int count = Convert.ToInt32(GetAllStarships(null, nmNameSpaceship).count);
            string responseSpaceship = "Not is spaceship with this especification";
            List<string> techAircraft = new List<string>();

            if (count > 0)
            {
                techAircraft.Add(GetAllStarships(null, nmNameSpaceship).results[0].name.ToString());
                techAircraft.Add(GetAllStarships(null, nmNameSpaceship).results[0].MGLT.ToString());

                //Calc stop resupply from Spaceship

                string sMGLT = GetAllStarships(null, nmNameSpaceship).results[0].MGLT.ToString();
                int nuMGLT = Convert.ToInt32(sMGLT);

                CalcMath cm = new CalcMath();
                string cr = cm.calcStopResupply(nuMGLT, nuOPPlanet).ToString();
                techAircraft.Add(cr);

                // End calc resupply from Spaceship

                StringBuilder builder = new StringBuilder();

                foreach (string element in techAircraft)
                {
                    // Append each int to the StringBuilder overload.
                    builder.Append(element).Append(" | ");
                }
                
                responseSpaceship = builder.ToString();
            }

            return responseSpaceship;
        }

        public string GetAllSpaceships()
        {
            SharpEntityResults<Starship> oStarships = GetAllStarships();

            int count = Convert.ToInt32(oStarships.count);
            int limitStarship = 10;
            string responseSpaceship = "";
            List<string> techAircraft = new List<string>();

            if (count > 0)
            {
                int x = 0;

                while (limitStarship > x)
                {                    
                    techAircraft.Add(oStarships.results[x].name.ToString());
                    techAircraft.Add(oStarships.results[x].MGLT.ToString() + " & ");
                    x = x + 1;
                }

                foreach (string element in techAircraft)
                {
                    StringBuilder builder = new StringBuilder();

                    if (element.Contains(" & "))
                    {
                        builder.Append(element);
                    }
                    else
                    {
                        builder.Append(element).Append(" | ");
                    }

                    responseSpaceship += builder.ToString();
                }

            }
            else
            {
                responseSpaceship = "Not is spaceship with this especification";
            }

            return responseSpaceship;
        }


        public string GetAllPlanets()
        {
            SharpEntityResults<Planet> oPlanets = GetAllPlanets("1");

            int count = Convert.ToInt32(oPlanets.count);
            int limitPlanets = 10;
            string responsePlanets = "";

            List<string> detailPlanet = new List<string>();            

            if (count > 0)
            {
                int x = 0;

                while (limitPlanets > x)
                {

                    detailPlanet.Add(oPlanets.results[x].name.ToString());
                    detailPlanet.Add(oPlanets.results[x].orbital_period.ToString() + " & ");                    
                    x = x + 1;

                }

                foreach (string element in detailPlanet)
                {
                    StringBuilder builder = new StringBuilder();
                    // Append each int to the StringBuilder overload.
                    if(element.Contains(" & ")) { 
                         builder.Append(element);
                    }
                    else
                    {
                         builder.Append(element).Append(" | ");
                    }

                    responsePlanets += builder.ToString();

                }

            }
            else
            {
                responsePlanets = "Not is planets with this especification";
            }

            return responsePlanets;
        }
    }
}