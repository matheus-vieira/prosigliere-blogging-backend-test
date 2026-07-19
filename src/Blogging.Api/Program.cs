using Blogging.Api.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.AddBloggingDomain();
builder.AddBloggingApi();

var app = builder.Build();
await app.UseBloggingAsync().ConfigureAwait(false);
app.UseBloggingApi();

app.MapGet("/", () => "Hello World!");

app.Run();

public partial class Program
{
}
