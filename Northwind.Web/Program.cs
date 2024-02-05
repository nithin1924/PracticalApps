var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

#region Configure the HTTP pipeline and routes
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}
app.UseHttpsRedirection();

app.UseDefaultFiles(); // index.html, default.html, and so on.
app.UseStaticFiles();

app.MapGet("/hello", () =>$"Environment is {app.Environment.EnvironmentName}");
app.MapGet("/", () => "Hello World!");
#endregion

app.Run();