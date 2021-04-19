using Attender.Server.Application.Common.Interfaces;
using Attender.Server.Infrastructure.Auth;
using Attender.Server.Infrastructure.Blob;
using Attender.Server.Infrastructure.Persistence;
using Attender.Server.Infrastructure.Sms;
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
                provider.GetService<AttenderDbContext>() ??
                throw new ArgumentNullException(nameof(AttenderDbContext)));

            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<ITokensGenerator, TokensGenerator>();
            services.AddTransient<ITokensValidator, TokensValidator>();
            services.AddTransient<ISmsService, TwilioSmsService>();

            services.AddScoped<IBlobService, AzureBlobService>();
            services.AddScoped(_ =>
                new BlobServiceClient(configuration.GetConnectionString("AzureBlobStorage")));

            return services;
        }
    }
}
