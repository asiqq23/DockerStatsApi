namespace StatsApi
{
    using System.Text;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Primitives;
    using Microsoft.Net.Http.Headers;

    [Route("stats")]
    public class StatsController : Controller
    {
        [HttpGet("stream")]
        public async Task Stream()
        {
            
            Response.StatusCode = 200;
            Response.Headers.Add(HeaderNames.ContentType, "application/octet-stream");

            while (true)
            {
                Console.WriteLine("writing");

                var dd = Encoding.UTF8.GetBytes(DateTime.Now.ToString() + Environment.NewLine);
                await Response.Body.WriteAsync(dd, 0, dd.Length);
                await Task.Delay(1000);
            }
        }

        [HttpGet("hi")]
        public IActionResult Hi() => Ok("hi");
    }
}
