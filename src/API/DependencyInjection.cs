using Attender.Server.API.Configuration;
using Attender.Server.API.Constants;
using Attender.Server.API.Interceptors;
using Attender.Server.Application.Common.Models;
using Attender.Server.Infrastructure.Auth;
using Attender.Server.Infrastructure.Sms;
using FluentValidation;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace Attender.Server.API
{
    public static class DependencyInjection
    {
        public static void ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<TwilioOptions>(configuration.GetSection("Twilio"));
            services.Configure<AuthOptions>(configuration.GetSection("Auth"));
        }

        public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var issuer = configuration["Auth:Issuer"];
            var key = Encoding.ASCII.GetBytes(configuration["Auth:SecurityKey"]);

            var tokenParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256Signature },
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            };

            services.AddSingleton(tokenParameters);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = tokenParameters;
            });
        }

        public static void AddAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthConstants.Policy.RegisteredOnly,
                    policy => policy
                        .RequireClaim(ClaimTypes.NameIdentifier)
                        .RequireClaim(ClaimTypes.Name));
            });
        }

        public static void AddControllers(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Conventions.Add(new RouteTokenTransformerConvention(new DashParameterTransformer()));
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var error = context.ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => JsonConvert.DeserializeObject<Error>(e.ErrorMessage))
                        .First();

                    return new BadRequestObjectResult(error);
                };
            })
            .AddFluentValidation(fv =>
            {
                fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                fv.ImplicitlyValidateChildProperties = true;
            });

            ValidatorOptions.Global.CascadeMode = CascadeMode.Stop;
            services.AddTransient<IValidatorInterceptor, UseCustomErrorModelInterceptor>();
        }

        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Attender.Server", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer schema",
                    Name = "Authorization",
                    Scheme = "Bearer",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });

                c.CustomSchemaIds(type => type.FullName);
                c.SchemaFilter<SwaggerNamespaceSchemaFilter>();

                c.DescribeAllParametersInCamelCase();
                c.SupportNonNullableReferenceTypes();
                c.UseAllOfToExtendReferenceSchemas();
                c.AddFluentValidationRules();

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.Configure<FluentValidationSwaggerGenOptions>(options =>
                options.SetNotNullableIfMinLengthGreaterThenZero = true);
        }
    }
}
