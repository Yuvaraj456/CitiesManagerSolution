using CitiesManager.Core.Identity;
using CitiesManager.Core.ServiceContracts;
using CitiesManager.Core.Services;
using CitiesManager.Infrastructure.DatabaseContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddTransient<IJwtService, JwtService>();

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
    options.GroupNameFormat = "'v'VVV"; //v1 , v1.0
    options.SubstituteApiVersionInUrl= true;
});


//Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireDigit = true;

})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders()
.AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>() //identityUser Store added here!
.AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>();    //identityRole Store added here!


//Cors: LocalHost:4200
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policybuilder =>
    {
        policybuilder.WithOrigins(builder.Configuration.GetSection("AllowOrigins").Get<String[]>()) // add "*" allow all domain to invoke api https request and response, its is not secure so dont use it.
        //we could add one or more domain in the WithOrigins method

        .WithHeaders("Authorization", "origin", "accept", "content-type") //if request header contain any of these headers we accept to access api

        .WithMethods("GET","POST","PUT","DELETE"); //Allow request as we want, if we dont want Put then remove from this.
    });

    //Custom policy
    options.AddPolicy("CustomCorsPolicy",policybuilder =>
    {
        policybuilder.WithOrigins(builder.Configuration.GetSection("CustomAllowOrigins").Get<String[]>()) // add "*" allow all domain to invoke api https request and response, its is not secure so dont use it.
        //we could add one or more domain in the WithOrigins method

        .WithHeaders("Authorization", "origin", "accept") //if request header contain any of these headers we accept to access api 

        .WithMethods("GET"); //Allow only get request.
    });
});


var app = builder.Build();
    
// Configure the HTTP request pipeline.
app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSwagger(); //creates endpoints for swagger json
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "1.0");
    options.SwaggerEndpoint("/swagger/v2/swagger.json", "2.0");

}); // creates Swagger ul for testing all web Api endpoints/action methods
app.UseRouting();
app.UseCors();//enable cors
app.UseAuthentication(); //username and password to enter into website
app.UseAuthorization();//access for accessing particular page 

app.MapControllers();

app.Run();
