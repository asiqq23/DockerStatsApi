using Newtonsoft.Json;

//var http = new HttpClient();
//http.BaseAddress = new Uri("http://localhost:2376");

//var response = await http.GetAsync("/containers/json");
//var jsonString = await response.Content.ReadAsStringAsync();
//var containers = JsonConvert.DeserializeObject<IEnumerable<Container>>(jsonString);

//var tries = 0;

//foreach (var container in containers)
//{

//    var httpResponseMessage = await http.SendAsync(new HttpRequestMessage(HttpMethod.Get, $"/containers/{container.Id}/stats"), HttpCompletionOption.ResponseHeadersRead);

//    //foreach (var header in httpResponseMessage.Headers.TransferEncodingChunked)
//    //{

//    //    Console.WriteLine(header.Key + " "  + header.Value);
//    //}

//    var stream = await httpResponseMessage.Content.ReadAsStreamAsync();

//    using var reader = new StreamReader(stream);

//    while (!reader.EndOfStream)
//    {
//        tries++;

//        if (tries > 3)
//        {
//            return;
//        }
//        var str = await reader.ReadLineAsync();
//        var stats = JsonConvert.DeserializeObject<Statistics>(str);


//        Console.WriteLine($"--------------- {container.ServiceName} ---------------");
//        Console.WriteLine(stats);
//    }

//    //var stream = await http.GetStreamAsync($"/containers/{container.Id}/stats");

//    //using var reader = new StreamReader(stream);

//    //while (!reader.EndOfStream)
//    //{
//    //    var str = await reader.ReadLineAsync();
//    //    var stats = JsonConvert.DeserializeObject<Statistics>(str);


//    //    Console.WriteLine($"--------------- {container.ServiceName} ---------------");
//    //    Console.WriteLine(stats);
//    //}
//}



var http = new HttpClient();
http.BaseAddress = new Uri("http://localhost:5069");

var dd = await http.SendAsync(new HttpRequestMessage(HttpMethod.Get, $"/monitoring/StreamStatistics"), HttpCompletionOption.ResponseHeadersRead);

Console.WriteLine("Code: " + dd.StatusCode);

var stream = await dd.Content.ReadAsStreamAsync();
using var reader = new StreamReader(stream);

Console.WriteLine("End of stream: " + reader.EndOfStream);

var buffer = new char[1024];
while (!reader.EndOfStream)
{
    var res = await reader.ReadLineAsync();
    Console.WriteLine(res);
}

Console.ReadLine();

public class Container
{
    public string Id { get; set; }
    public string Image { get; set; }
    public string ServiceName => Image.Substring(Image.LastIndexOf("/") + 1);
    public string Version => ServiceName.Substring(ServiceName.LastIndexOf(":") + 1);
}

public class Statistics
{
    [JsonProperty("memory_stats")]
    public MemoryUsage MemoryUsageStats { get; set; }

    public class MemoryUsage
    {
        [JsonProperty("privateworkingset")]
        public ulong Bytes { get; set; }


        public string MegaBytes => ConvertBytesToMegabytes(Bytes).ToString("N2");
    }

    public override string ToString()
    {
        return $"MemoryUsage usage: {MemoryUsageStats.MegaBytes} MB";
    }

    static double ConvertBytesToMegabytes(ulong bytes)
    {
        return (bytes / 1024f) / 1024f;
    }
}