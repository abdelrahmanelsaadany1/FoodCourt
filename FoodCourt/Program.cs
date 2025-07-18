using System.Text;
using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence.Data;
using Services.Abstractions.ICategoryService;
using Services.Auth;
using Services.CategoryService;


namespace FoodCourt
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Allow CORS -- 1
            string txt = "";
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
            builder.Services.AddScoped<IDbInitializer, DbInitializer>();
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
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                    };
                });
            // Facebook and google
            builder.Services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
                });
            //.AddFacebook(options =>
            //{
            //    options.AppId = builder.Configuration["Authentication:Facebook:AppId"];
            //    options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
            //});

            // Allow CORS --2
            builder.Services.AddCors(opt =>
            {
                opt.AddPolicy(txt,
                    builder =>
                    {
                        //builder.AllowAnyOrigin();
                        // aw3a t3mel kda https://localhost:7129/ msh hyshta5l
                        builder.WithOrigins("http://localhost:4200", "https://akelny-front.vercel.app"); // a3mal comma we adef tany
                        //builder.WithMethods("Post", "get");
                        builder.AllowAnyMethod();
                        builder.AllowAnyHeader();
                    });
            });

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

            // Allow CORS -- 3
            app.UseCors(txt);

            app.MapGet("/", () => "✅ FoodCourt API — VERSION 1.5 ✅");
            app.MapControllers();

            app.Run();
        }
    }
}