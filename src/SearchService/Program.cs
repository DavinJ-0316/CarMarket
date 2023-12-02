using System.Net;
using Polly;
using Polly.Extensions.Http;
using SearchService.Data;
using SearchService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// .AddPolicyHandler(GetPolicy()); 这个函数会在此循环直到接通auctionservice后端为止； 或者直到系统的100秒；
// System.Threading.Tasks.TaskCanceledException: The request was canceled due to the configured HttpClient.Timeout of 100 seconds elapsing.
// 在此循环之时， 此进程会卡在.AddPolicyHandler(GetPolicy()); 不执行下面的代码； 所以
// DBInitializer.InitDb(app); 执行不了  无法使用现有的mongodb的数据 api could not send request
builder.Services.AddHttpClient<AuctionSvcHttpClient>().AddPolicyHandler(GetPolicy());

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseAuthorization();

app.MapControllers();

// Create a mongodb client
// var client = new MongoClient(DefaultConnectionString);

// public static string DefaultConnectionString = 
//   Program.Configuration.GetConnectionString("MongoDBConnection");

app.Lifetime.ApplicationStarted.Register(async () => {
    try
    {
        await DBInitializer.InitDb(app);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
    }
});

app.Run();

static IAsyncPolicy<HttpResponseMessage> GetPolicy()
    => HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
        .WaitAndRetryForeverAsync(_ => TimeSpan.FromSeconds(3));
