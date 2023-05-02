
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ContainersApiTask.Models;
using System.Text.Json.Serialization;
using System.Text;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text.Json;
using Serilog;

namespace ContainersApiTask
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug().WriteTo.Console()
                .WriteTo.File("logs.txt").CreateLogger();//, rollingInterval: RollingInterval.Day)
            
            //var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri"));
            //builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());

            // Add services to the container.

            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration["ConnectionStrings:LocalDB"]));
            //builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration["ConnectionStrings:AzureDb"]));
            builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
            //builder.Services.AddControllers().AddJsonOptions(x =>
            //              x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            builder.Services.AddControllers().AddJsonOptions(x =>
                           x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

         

            



            builder.Services.AddControllers().AddXmlDataContractSerializerFormatters();
            //builder.Services.AddSingleton<IDbInitService, DbInitService>();
            builder.Services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidAudience = builder.Configuration["token:audience"],
                    ValidIssuer = builder.Configuration["token:issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["token:key"]))
                };
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Containers API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
            });

            //builder.Logging.AddDbLogger(options =>
            //{
            //    builder.Configuration.GetSection("Logging").GetSection("Database").GetSection("Options").Bind(options);
            //});
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment() || !app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            //added
            app.UseAuthentication();
            //
            app.UseAuthorization();

            app.MapControllers();

            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
            //DbInitializer.Initialize(app);
            app.Run();
        }
    }
}