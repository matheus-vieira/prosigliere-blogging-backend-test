using Blogging.Api.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.AddBloggingDomain();

var app = builder.Build();
await app.UseBloggingAsync().ConfigureAwait(false);

app.MapGet("/", () => "Hello World!");

app.Run();

public partial class Program
{
}
