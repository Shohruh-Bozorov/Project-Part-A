#define UseNewsApiSample  // Remove or undefine to use your own code to read live data

using System.Net.Http;
using System.Net.Http.Json; //Requires nuget package System.Net.Http.Json
using System.Threading.Tasks;
using System.Collections.Generic;
using Assignment_A2_01.Models;
using Assignment_A2_01.ModelsSampleData;
using System;

namespace Assignment_A2_01.Services
{
    public class NewsService
    {
        HttpClient httpClient = new HttpClient();
        readonly string apiKey = "cab0b08764234dd59524e98e44df478c";
        public async Task<NewsApiData> GetNewsAsync()
        {


            //NewsApiData nd = await NewsApiSampleData.GetNewsApiSampleAsync("sports");

            //Your code here to read live data
            //https://newsapi.org/docs/endpoints/top-headlines
            var uri = $"https://newsapi.org/v2/top-headlines?country=se&category=sports&apiKey={apiKey}";

            // part of your read web api code here
            HttpResponseMessage response = await httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            NewsApiData nd = await response.Content.ReadFromJsonAsync<NewsApiData>();

            // part of your data transformation to Forecast here
            NewsApiData newsApiData = new NewsApiData();

            newsApiData.Articles = new List<Article>();

            try
            {
                nd.Articles.ForEach(x => newsApiData.Articles.Add(x));

            }

            catch (Exception)
            {

                throw;
            }

            return newsApiData;
        }

       
           


    }
}
