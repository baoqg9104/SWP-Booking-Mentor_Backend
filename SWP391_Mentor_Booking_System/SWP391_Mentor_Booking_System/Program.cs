using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SWP391_Mentor_Booking_System_Data;
using SWP391_Mentor_Booking_System_Data.Repositories;
using SWP391_Mentor_Booking_System_Service.Service;

var builder = WebApplication.CreateBuilder(args);

// Configure the database context (EF Core) using a connection string.
builder.Services.AddDbContext<SWP391_Mentor_Booking_System_DBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowSpecificOrigin",
        builder =>
            builder
                .WithOrigins("http://localhost:5173") // Add your React app's URL
                .AllowAnyHeader()
                .AllowAnyMethod()
    );
});

//builder
//    .Services.AddControllers()
//    .AddJsonOptions(options =>
//    {
//        options.JsonSerializerOptions.ReferenceHandler = System
//            .Text
//            .Json
//            .Serialization
//            .ReferenceHandler
//            .Preserve;
//    });

builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            options.DefaultChallengeScheme =
            options.DefaultForbidScheme =
            options.DefaultScheme =
            options.DefaultSignInScheme =
            options.DefaultSignOutScheme =
                JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SigningKey"])
            ),
        };
        options.IncludeErrorDetails = true;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("MentorOnly", policy => policy.RequireRole("Mentor"));
    options.AddPolicy("StudentOnly", policy => policy.RequireRole("Student"));
    options.AddPolicy("StudentOrAdmin", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("Student") || context.User.IsInRole("Admin")));
    options.AddPolicy("StudentOrMentor", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("Student") || context.User.IsInRole("Mentor")));
    options.AddPolicy("MentorOrAdmin", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("Mentor") || context.User.IsInRole("Admin")));
    options.AddPolicy("AllPolicy", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("Student") || context.User.IsInRole("Admin") || context.User.IsInRole("Mentor")));
});

// Add services to the container.
builder.Services.AddControllers();

// Configure Swagger/OpenAPI.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer",
        }
    );
    option.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer",
                    },
                },
                new string[] { }
            },
        }
    );
});

// Add repository and services to DI container
builder.Services.AddScoped<AuthRepository>();
builder.Services.AddScoped<MentorRepository>();
builder.Services.AddScoped<RefreshTokenRepository>(); // Đăng ký RefreshTokenRepository
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<MentorSlotService>();
builder.Services.AddScoped<MentorService>();
builder.Services.AddScoped<StudentService>();
builder.Services.AddScoped<StudentRepository>();
builder.Services.AddScoped<SemesterService>();
builder.Services.AddScoped<TopicService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use CORS
app.UseCors("AllowSpecificOrigin");

// Enable HTTPS redirection
app.UseHttpsRedirection();

// Enable authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

// Map controllers to routes
app.MapControllers();

// Run the application
app.Run();
