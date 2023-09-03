using CitiesManager.WebApi.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ConsumesAttribute("application/json")); // added default request body content type

    options.Filters.Add(new ProducesAttribute("application/json")); // added default response body content type
})
 .AddXmlSerializerFormatters();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
//Swagger Services
builder.Services.AddEndpointsApiExplorer(); //generate description for All endpoints
builder.Services.AddSwaggerGen(options => //generate open api specification
{ 
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "api.xml"));
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo() {Title="Cities Web Api", Version= "1.0" });
    options.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo() { Title = "Cities Web Api", Version = "2.0" });
});

//Enable versioning in web API controllers
builder.Services.AddApiVersioning(config =>
{   //mostly used
    config.ApiVersionReader = new UrlSegmentApiVersionReader(); //1.reads version number from request url at "apiVersion" contraint

    //config.ApiVersionReader = new QueryStringApiVersionReader("version"); //2.reads version number from request query string called "api-version"

    //config.ApiVersionReader = new HeaderApiVersionReader("api-version");  //3.reads version number from request header  called "api-version" eg:api-version: 1.0



    config.DefaultApiVersion = new ApiVersion(1, 0); // default version option if not specified in url
    config.AssumeDefaultVersionWhenUnspecified = true;

});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VV"; //v1 , v1.0, v1.00
    options.SubstituteApiVersionInUrl= true;
});

var app = builder.Build();
    
// Configure the HTTP request pipeline.
app.UseHsts();
app.UseHttpsRedirection();


app.UseSwagger(); //creates endpoints for swagger json
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "1.0");
    options.SwaggerEndpoint("/swagger/v2/swagger.json", "2.0");

}); // creates Swagger ul for testing all web Api endpoints/action methods

app.UseAuthorization();

app.MapControllers();

app.Run();
