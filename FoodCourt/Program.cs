<<<<<<< HEAD
﻿
using System.Text;
using Application.Contracts;
using Application.Services;
using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
=======
﻿using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.Identity;
>>>>>>> origin/main
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
<<<<<<< HEAD

            // Identity Services
            builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();

            // JWT Authentication
            builder.Services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(op =>
                {
                    op.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey
                        (
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"])
                            )
                    };
                });

=======
            builder.Services.AddScoped<IDbInitializer, DbInitializer>();
>>>>>>> origin/main

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
            app.UseAuthentication();

            app.MapGet("/", () => "✅ FoodCourt API — VERSION 1.4 ✅");
            app.MapControllers();

            app.Run();
        }
    }
}