#define UseNewsApiSample  // Remove or undefine to use your own code to read live data

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json; //Requires nuget package System.Net.Http.Json
using System.Threading.Tasks;

using Assignment_A2_02.Models;
using Assignment_A2_02.ModelsSampleData;
namespace Assignment_A2_02.Services
{
    public class NewsService
    {
        public EventHandler<string> NewsAvailable;
        HttpClient httpClient = new HttpClient();
        readonly string apiKey = "cab0b08764234dd59524e98e44df478c";

        public async Task<News> GetNewsAsync(NewsCategory category)
        {     
            //NewsApiData nd = await NewsApiSampleData.GetNewsApiSampleAsync(category);

            OnNewsAvailable($"News in category available: {category}");

            //https://newsapi.org/docs/endpoints/top-headlines
            var uri = $"https://newsapi.org/v2/top-headlines?country=se&category={category}&apiKey={apiKey}";

           // Your code here to get live data
            HttpResponseMessage response = await httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            NewsApiData nd = await response.Content.ReadFromJsonAsync<NewsApiData>();

            News news = new News();
            news.Articles = new List<NewsItem>();

            nd.Articles.ForEach(item => news.Articles.Add(GetNewsItem(item)));

            return news;
        }

        private NewsItem GetNewsItem(Article item)
        { 
            NewsItem newsItem = new NewsItem();
            newsItem.Title = item.Title;
            newsItem.DateTime = item.PublishedAt;
            return newsItem;
        
        }
        protected virtual void OnNewsAvailable(string e) 
        {
            NewsAvailable?.Invoke(this, e);       
        }
    }
}
