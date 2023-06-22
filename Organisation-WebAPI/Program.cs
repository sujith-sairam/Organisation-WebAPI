 global using Organisation_WebAPI.Dtos;
global using Organisation_WebAPI.Models;
global using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using EmailService;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Organisation_WebAPI.Data;
using Organisation_WebAPI.Services.AuthRepo;
using Organisation_WebAPI.Services.Customers;
using Organisation_WebAPI.Services.Departments;
using Organisation_WebAPI.Services.Employees;
using Organisation_WebAPI.Services.Products;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<OrganizationContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAutoMapper(typeof(Program).Assembly);


builder.Services.AddScoped<IAuthRepository,AuthRepository>();
builder.Services.AddScoped<IProductService,ProductService>();
builder.Services.AddScoped<IDepartmentService,DepartmentService>(); 
builder.Services.AddScoped<IEmployeeService,EmployeeService>();
builder.Services.AddScoped<ICustomerService,CustomerService>();

builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IEmailSender, EmailSender>();

var emailConfig = builder.Configuration
        .GetSection("EmailConfiguration")
        .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

#region Configure Authentication

 

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8
            .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });
#endregion

#region Configure Swagger

builder.Services.AddSwaggerGen(c=>
{
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer Scheme, e.g. \"bearer {token}\"",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    c.OperationFilter<SecurityRequirementsOperationFilter>();
});

#endregion


builder.Services.AddMemoryCache();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
