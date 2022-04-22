using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Assignment_A2_03.Models;
using Assignment_A2_03.Services;

namespace Assignment_A2_03
{
    class Program
    {
        static void Main(string[] args)
        {
            NewsService service = new NewsService();
            service.NewsAvailable += ReportNewsDataAvailable;

            Task<News> task1 = null, task2 = null;
            Exception exception = null;

            try
            {
                for (NewsCategory category = NewsCategory.business; category < NewsCategory.technology + 1 ; category++)
                {
                    task1 = service.GetNewsAsync(category);
                }
                Task.WaitAll(task1);

                for (NewsCategory category = NewsCategory.business; category < NewsCategory.technology + 1; category++)
                {
                    task2 = service.GetNewsAsync(category);
                }
                Task.WaitAll(task2);
            }
            catch (Exception ex)
            {
                exception = ex;
                Console.WriteLine(ex);
            }

            for (NewsCategory category = NewsCategory.business; category < NewsCategory.technology + 1; category++)
            {
                Console.WriteLine($"News in category: {category}");
                if (task1?.Status == TaskStatus.RanToCompletion)
                {
                    News news = task1.Result;
                    news.Articles.ForEach(article => Console.WriteLine($"- {article.DateTime.ToString("yyyy-MM-dd HH-mm-ss")} \t { article.Title}"));
                    Console.WriteLine();

                }
                else
                    Console.WriteLine("Something went wrong");
            }

            for (NewsCategory category = NewsCategory.business; category < NewsCategory.technology + 1; category++)
            {
                Console.WriteLine($"Cached News in category: {category}");
                if (task2?.Status == TaskStatus.RanToCompletion)
                {
                    News news = task2.Result;
                    news.Articles.ForEach(article => Console.WriteLine($"- {article.DateTime.ToString("yyyy-MM-dd HH-mm-ss")} \t { article.Title}"));
                    Console.WriteLine();

                }
                else
                    Console.WriteLine("Something went wrong");
            }






        }

        static void ReportNewsDataAvailable(object sender, string message)
        {
            Console.WriteLine($"Event message from news service: {message}");
        }
    }
}
