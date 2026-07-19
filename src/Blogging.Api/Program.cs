using Blogging.Api.DependencyInjection;
using Blogging.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);
builder.AddBloggingDomain();
builder.AddBloggingApi();

var app = builder.Build();
await app.UseBloggingApiAsync().ConfigureAwait(false);

app.Run();

public partial class Program
{
}
