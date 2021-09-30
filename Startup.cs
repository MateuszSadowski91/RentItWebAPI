using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using RentItAPI.Entities;
using RentItAPI.Middleware;
using RentItAPI.Models;
using RentItAPI.Models.Validators;
using RentItAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using FluentEmail.MailKitSmtp;

namespace RentItAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var from = Configuration.GetSection("Email")["From"];
            var mailSender = Configuration.GetSection("Gmail")["Sender"];
            var mailPassword = Configuration.GetSection("Gmail")["Password"];
            var mailPort = Convert.ToInt32(Configuration.GetSection("Gmail")["Port"]);
            var server = Configuration.GetSection("Gmail")["Server"];
            services
                .AddFluentEmail(mailSender, from)
                .AddRazorRenderer()
                .AddMailKitSender(new SmtpClientOptions
                {
                    Server = server,
                    Port = mailPort,
                    UseSsl = true,
                    RequiresAuthentication = true,
                    User = mailSender,
                    Password = mailPassword,
                });
            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IRequestService, RequestService>();
            services.AddScoped<IValidator<RequestQuery>, RequestQueryValidator>();
            services.AddScoped<IValidator<ReservationQuery>, ReservationQueryValidator>();
            services.AddScoped<IValidator<ItemQuery>, ItemQueryValidator> ();
            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IBusinessService, BusinessService>();
            services.AddAutoMapper(this.GetType().Assembly);
            services.AddHttpContextAccessor();
            services.AddScoped<IUserContextService, UserContextService>();

            var authenticationSettings = new AuthenticationSettings();
            Configuration.GetSection("Authentication").Bind(authenticationSettings);
            services.AddSingleton(authenticationSettings);
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = "Bearer";
                option.DefaultScheme = "Bearer";
                option.DefaultChallengeScheme = "Bearer";
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = authenticationSettings.JwtIssuer,
                    ValidAudience = authenticationSettings.JwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)),
                };
            });
            services.AddControllers().AddFluentValidation();
            services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("RentItDBConnection")));
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ErrorHandlingMiddleWare>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<DBSeeder>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RentItAPI", Version = "v1" });
            });
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DBSeeder seeder)
        {
            app.UseStaticFiles();
            seeder.Seed();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RentItAPI v1"));
            }
            app.UseMiddleware<ErrorHandlingMiddleWare>();
            app.UseAuthentication();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
