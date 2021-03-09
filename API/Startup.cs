using System.Text;
using API.Middleware;
using Application.Features.Transaction.CheckTransactionAmount;
using Application.Features.Transaction.CommitTransaction;
using Application.Interfaces;
using Application.Mapping;
using Application.User.Login;
using Application.User.Registration;
using AutoMapper;
using Domain;
using Domain.Entities;
using EFData;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
			services.AddDbContext<DataContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            
            services.AddMediatR(typeof(LoginHandler).Assembly);

			services.AddMvc(option =>
				{
					option.EnableEndpointRouting = false;
					var policy = new AuthorizationPolicyBuilder()
						.RequireAuthenticatedUser()
						.Build();
					option.Filters.Add(new AuthorizeFilter(policy));
				}).SetCompatibilityVersion(CompatibilityVersion.Latest)
				.AddJsonOptions(options =>{ options.JsonSerializerOptions.IgnoreNullValues = true; })
				.AddFluentValidation();

            services.TryAddSingleton<ISystemClock, SystemClock>();

            var builder = services.AddIdentityCore<User>();
            var identityBuilder = new IdentityBuilder(builder.UserType, builder.Services);
            identityBuilder.AddEntityFrameworkStores<DataContext>();
            identityBuilder.AddSignInManager<SignInManager<User>>();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["TokenKey"]));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
                opt =>
                    {
                        opt.TokenValidationParameters = new TokenValidationParameters
                                                            {
                                                                ValidateIssuerSigningKey = true,
                                                                IssuerSigningKey = key,
                                                                ValidateAudience = false,
                                                                ValidateIssuer = false,
                                                            };
                    });

            services.AddScoped<IJwtGenerator, JwtGenerator>();
			
			var mapperConfig = new MapperConfiguration(mc =>
			{
				mc.AddProfile(new MappingProfile());
			});
			IMapper mapper = mapperConfig.CreateMapper();
			services.AddSingleton(mapper);
			
			services.AddTransient<IValidator<LoginQuery>, LoginQueryValidation>();
			services.AddTransient<IValidator<RegistrationCommand>, RegistrationValidation>();
			services.AddTransient<IValidator<CheckTransactionQuery>, CheckTransactionValidation>();
			services.AddTransient<IValidator<CommitTransactionCommand>, CommitTransactionValidation>();
			
			services.AddCors(options =>
			{
				options.AddDefaultPolicy(buildr =>
				{
					buildr.WithOrigins("http://localhost:4200");
					buildr.AllowAnyMethod();
					buildr.AllowAnyHeader();
					buildr.AllowCredentials();
				});
			});
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseAuthentication();
            app.UseCors();
            app.UseMvcWithDefaultRoute();
		}
    }
}
