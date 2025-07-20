<<<<<<< Updated upstream
﻿using Domain.Contracts;
=======

using System.Text;
using Domain.Contracts;
>>>>>>> Stashed changes
using Domain.Entities;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Services.Abstractions.ICategoryService;
using Services.CategoryService;
using static System.Net.WebRequestMethods;

namespace FoodCourt
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpContextAccessor();
            // Add DbContexts
            builder.Services.AddDbContext<FoodCourtDbContext>((optionsbuilder =>
            {
                optionsbuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            }));

            builder.Services.AddDbContext<IdentityContext>((optionsbuilder =>
            {
                optionsbuilder.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            }));

            // Add Identity
            builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();

            // Add your services
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IDbInitializer, DbInitializer>();
<<<<<<< Updated upstream
=======
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IResturantService,ResturantService>();
            // JWT
            builder.Services.AddScoped<JwtService>();
            // Email Service
            builder.Services.AddScoped<EmailService>();


            // Authentication
            builder.Services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not found in configuration")))
                    };
                });

            // Facebook and google
            builder.Services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = builder.Configuration["Authentication:Google:ClientId"] ?? "";
                    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"] ?? "";
                });

            // Allow CORS --2
            builder.Services.AddCors(opt =>
            {
                opt.AddPolicy(corsPolicyName,
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:4200", "https://akelny-front.vercel.app");
                        policy.AllowAnyMethod();
                        policy.AllowAnyHeader();
                        policy.AllowCredentials();
                    });
            });
>>>>>>> Stashed changes

            var app = builder.Build();

            // Initialize database and roles
            using (var scope = app.Services.CreateScope())
            {
                var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
                await dbInitializer.InitializerIdentityAsync();
            }


            //Configure the HTTP request pipeline
           if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
           
            }
      
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapGet("/", () => "✅ FoodCourt API — VERSION 1.4 ✅");
            app.MapControllers();

            app.Run();
        }
    }
}