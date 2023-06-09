﻿using Attender.Server.Application.Common.Auth.Services;
using Attender.Server.Application.Common.Interfaces;
using Attender.Server.Application.Common.Sms.Services;
using Attender.Server.Infrastructure.Auth;
using Attender.Server.Infrastructure.Blob;
using Attender.Server.Infrastructure.Persistence;
using Attender.Server.Infrastructure.Sms;
using Azure.Identity;
using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Attender.Server.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AttenderDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<IAttenderDbContext>(provider =>
                provider.GetRequiredService<AttenderDbContext>());

            services.AddTransient<IAuthService, AuthService>();
            services.Configure<AuthOptions>(options => configuration.GetSection("Auth").Bind(options));

            services.AddTransient<TokensGenerator>();
            services.AddTransient<TokensValidator>();

            services.AddTransient<ISmsService, TwilioSmsService>();
            services.Configure<TwilioOptions>(options => configuration.GetSection("Twilio").Bind(options));

            services.AddScoped<IBlobService, AzureBlobService>();
            services.AddScoped(_ =>
                new BlobServiceClient(new Uri(configuration["AzureBlob:Url"]), new DefaultAzureCredential()));

            return services;
        }
    }
}
