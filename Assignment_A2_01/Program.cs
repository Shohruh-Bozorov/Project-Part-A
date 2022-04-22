using System;
using System.Threading.Tasks;
using Assignment_A2_01.Models;
using Assignment_A2_01.Services;

namespace Assignment_A2_01
{
    class Program
    {
        static void Main(string[] args)
        {
            NewsService service = new NewsService();
            Task<NewsApiData> task1 = service.GetNewsAsync();

            Task.WaitAll(task1);

            NewsApiData newsApiData = task1.Result;

            if (task1?.Status == TaskStatus.RanToCompletion)
            {
                Console.WriteLine("Top Headlines");

                foreach (var item in newsApiData.Articles)
                {
                    Console.WriteLine(item.Title + newsApiData.Status);
                }

            }

        }
    }
}
