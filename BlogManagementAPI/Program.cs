using BlogManagementAPI.CustomMiddleware;
using BlogManagementAPI.Filters;
using BlogManagementAPI.Helpers.BL;
using BlogManagementAPI.Mapper;
using BlogManagementAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>(); // Register the custom validation filter globally
});

// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["Secret"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        NameClaimType = ClaimTypes.Name
    };
});

// authtication on swagger UI

// Add Swagger services
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BlogPost API", Version = "v1" });

    // Define the security scheme
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Apply the security scheme globally to all operations
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,

            },
            new List<string>()
        }
    });

    // Optionally include XML comments if you have them
    // var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    // var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    // c.IncludeXmlComments(xmlPath);
});
//

// Register Authorization policies if needed
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("User", policy => policy.RequireRole("User"));
    // Add more policies as needed
});

//mapper
builder.Services.AddAutoMapper(typeof(MapperProfile));
// Register LoadService as a singleton since it holds in-memory data
builder.Services.AddSingleton<ILoadData, LoadData>();
builder.Services.AddSingleton<IUserService, UserServices>();
builder.Services.AddHostedService(provider => (LoadData)provider.GetRequiredService<ILoadData>());
builder.Services.AddTransient<IBlogPostBL, BlogPostBL>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Define CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder
                .WithOrigins("*") // Angular app URL
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});


var app = builder.Build();



//Registering the custom middleware exception handler
app.UseMiddleware<CustomExceptionHandlingMiddleware>();

// to make sure that the he Data directory and JSON file exist
var env = app.Services.GetRequiredService<IWebHostEnvironment>();
var dir = Path.Combine(env.ContentRootPath, "Data");
if (!Directory.Exists(dir))
{
    Directory.CreateDirectory(dir);
}
var jsonFilePath = Path.Combine(dir, "blogpostdata.json");
if (!File.Exists(jsonFilePath))
{
    File.WriteAllText(jsonFilePath, "[]"); //Initialize with empty array if file does not exists
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Apply CORS policy
app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
