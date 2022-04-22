using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json; //Requires nuget package System.Net.Http.Json
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Text.Json;

using Assignment_A1_03.Models;

namespace Assignment_A1_03.Services
{
    public class OpenWeatherService
    {
        HttpClient httpClient = new HttpClient();
        readonly string apiKey = "0b950222dafb518e7aba1268983b2346"; // Your API Key

        // part of your event and cache code here
        public event EventHandler<string> WeatherForecastAvailable;
        ConcurrentDictionary<string, Forecast> _Cache = new ConcurrentDictionary<string, Forecast>();
        ConcurrentDictionary<(double, double), Forecast> _Cache2 = new ConcurrentDictionary<(double, double), Forecast>();
        DateTime cachedTime { get; set; } = DateTime.Now; 
        private void OnWeatherAvailable(string e)
        {
            WeatherForecastAvailable?.Invoke(this, e);
        }

        public async Task<Forecast> GetForecastAsync(string City)
        {
            //part of cache code here

            if (DateTime.Now > cachedTime.AddSeconds(10))
            {
                _Cache.Clear();

            }

            string key = City;
            bool existInCache = _Cache.TryGetValue(key, out Forecast forecastFromCache);

            if (existInCache)
            {
                OnWeatherAvailable($"Cached wheater forecast for {forecastFromCache.City} available"); //generate an event with different message if cached data
            }
            
            //part of event and cache code here

            if (!existInCache)
            {
                //https://openweathermap.org/current
                var language = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                var uri = $"https://api.openweathermap.org/data/2.5/forecast?q={City}&units=metric&lang={language}&appid={apiKey}";

                Forecast forecast = await ReadWebApiAsync(uri);

                _Cache.TryAdd(key, forecast);
                cachedTime= DateTime.Now;

                OnWeatherAvailable($"New weather forecast for {forecast.City} available");

                return forecast;
            }
                        
    

            return forecastFromCache;

        }
        public async Task<Forecast> GetForecastAsync(double latitude, double longitude)
        {
            //part of cache code here

            if (DateTime.Now > cachedTime.AddSeconds(60))
            {
                _Cache2.Clear();

            }

            var key = (latitude, longitude);
            bool existInCache = _Cache2.TryGetValue(key, out Forecast forecastFromCache);

            if (existInCache)
            {
                OnWeatherAvailable($"Cached wheater forecast available for ({latitude}, {longitude})");  //generate an event with different message if cached data
            }

            
            //part of event and cache code here
            if (!existInCache)
            {
                //https://openweathermap.org/current
                var language = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                var uri = $"https://api.openweathermap.org/data/2.5/forecast?lat={latitude}&lon={longitude}&units=metric&lang={language}&appid={apiKey}";

                Forecast forecast = await ReadWebApiAsync(uri);

                _Cache2.TryAdd(key, forecast);
                cachedTime = DateTime.Now;

                OnWeatherAvailable($"New weather forecast for ({latitude}, {longitude})");
                return forecast;

            }

            return forecastFromCache;


        }
        private async Task<Forecast> ReadWebApiAsync(string uri)
        {        
            // part of your read web api code here
            HttpResponseMessage response = await httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            WeatherApiData wd = await response.Content.ReadFromJsonAsync<WeatherApiData>();

            // part of your data transformation to Forecast here
            Forecast forecast = new Forecast();
            try
            {
                forecast.City = wd.city.name;
                forecast.Items = wd.list.Select(x => new ForecastItem
                {
                    WindSpeed = x.wind.speed,
                    Temperature = x.main.temp,
                    Icon = x.weather[0].icon,
                    DateTime = UnixTimeStampToDateTime(x.dt),
                    Description = x.weather[0].description
                }).ToList();
            }

            catch (Exception)
            {

                throw;
            }

            return forecast;
        }

            private DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
    }
}
