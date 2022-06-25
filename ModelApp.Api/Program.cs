using Microsoft.OpenApi.Models;
using ModelApp.Domain.Shareds;
using ModelApp.Infra.Contexts;
using ModelApp.Infra.Repositories;
using ModelApp.Infra.Repositories.Interfaces;
using ModelApp.Service.Helpers;
using ModelApp.Service.Helpers.Interfaces;
using ModelApp.Service.Services;
using ModelApp.Service.Services.Interfaces;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddEndpointsApiExplorer();

#region Dependence Injection setup

builder.Services.AddScoped<ModelAppContext, ModelAppContext>();

builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();
builder.Services.AddScoped<IMenuService, MenuService>();

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();

builder.Services.AddScoped<ICypherHelper, CypherHelper>();
builder.Services.AddScoped<IMailHelper, MailHelper>();
builder.Services.AddScoped<IPasswordGeneratorHelper, PasswordGeneratorHelper>();

#endregion

#region Swagger setup

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(
        "v1"
        , new OpenApiInfo
        {
            Title = "ModelApp.Api",
            Version = "v1",
            Description = "ModelApp api.",
            Contact = new OpenApiContact
            {
                Name = "Misael C. Homem",
                Url = new Uri("https://www.github.com/mchomem")
            },

        });

    string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

#endregion

#region Cors setup

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(o =>
    {
        o.WithOrigins(AppSettings.UrlsClientApp)
          .AllowAnyHeader()
          .AllowAnyMethod();
    });
});

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();
app.UseAuthorization();
app.MapControllers();
app.Run();
