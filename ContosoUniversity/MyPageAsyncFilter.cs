using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;

namespace ContosoUniversity
{
    public class MyPageAsyncFilter : IAsyncPageFilter
    {
        Stopwatch sw;
        async Task IAsyncPageFilter.OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            if(sw != null && sw.IsRunning)
            {
                sw.Stop();
                try
                {
                    context.HttpContext.Request.EnableBuffering();
                    var query = context.HttpContext.Request.QueryString.Value;
                    context.HttpContext.Request.Body.Position = 0;
                    await context.HttpContext.Request.Body.CopyToAsync(Console.OpenStandardOutput());
                    Console.WriteLine();
                    var url = context.HttpContext.Request.Path;
                    var httpType = context.HttpContext.Request.Method;

                    Console.WriteLine($"{url + query},{sw.Elapsed.TotalSeconds}");
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"{ex.Message}");
                }
            }
            await next.Invoke();
        }

        Task IAsyncPageFilter.OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            sw = new Stopwatch();
            sw.Start();
            
            return Task.CompletedTask;
        }
    }
}
