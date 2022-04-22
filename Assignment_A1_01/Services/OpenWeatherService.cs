using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json; //Requires nuget package System.Net.Http.Json
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;

using Assignment_A1_01.Models;

namespace Assignment_A1_01.Services
{
    public class OpenWeatherService
    {
        HttpClient httpClient = new HttpClient();
        readonly string apiKey = "0b950222dafb518e7aba1268983b2346"; // Your API Key
        public async Task<Forecast> GetForecastAsync(double latitude, double longitude)
        {
            //https://openweathermap.org/current
            var language = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            var uri = $"https://api.openweathermap.org/data/2.5/forecast?lat={latitude}&lon={longitude}&units=metric&lang={language}&appid={apiKey}";

            //Read the response from the WebApi
            HttpResponseMessage response = await httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            WeatherApiData wd = await response.Content.ReadFromJsonAsync<WeatherApiData>();

            //Your Code to convert WeatherApiData to Forecast using Linq.
            
            Forecast forecast = new Forecast()
            {
                City = wd.city.name
            };


            forecast.Items = wd.list.Select(x => new ForecastItem
            {
                WindSpeed = x.wind.speed,
                Temperature = x.main.temp,
                Icon = x.weather[0].icon,
                DateTime = UnixTimeStampToDateTime(x.dt),
                Description = x.weather[0].description
            }).ToList();



            return forecast;
        }

        public async Task<Forecast> GetForecastAsync(string cityName)
        {
            //https://openweathermap.org/current
            var language = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            var uri = $"https://api.openweathermap.org/data/2.5/forecast/daily?q={cityName}&units=metric&lang={language}&appid={apiKey}";

            //Read the response from the WebApi
            HttpResponseMessage response = await httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            WeatherApiData wd = await response.Content.ReadFromJsonAsync<WeatherApiData>();

            //Your Code to convert WeatherApiData to Forecast using Linq.

            Forecast forecast = new Forecast()
            {
                City = wd.city.name
            };


            forecast.Items = wd.list.Select(x => new ForecastItem
            {
                WindSpeed = x.wind.speed,
                Temperature = x.main.temp,
                Icon = x.weather[0].icon,
                DateTime = UnixTimeStampToDateTime(x.dt),
                Description = x.weather[0].description
            }).ToList();



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
