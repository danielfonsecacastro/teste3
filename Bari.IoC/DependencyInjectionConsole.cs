using Bari.Domain.Interfaces;
using Bari.Domain.Services;
using Bari.Domain.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.IO;

namespace Bari.IoC
{
    public static class DependencyInjectionConsole
    {
        private static IServiceProvider _serviceProvider;
        private static Settings _settings;

        public static void Configure()
        {
            var services = new ServiceCollection();

            var configuration = BuildConfiguration();
            _settings = configuration.Get<Settings>();

            services
                .ConfigureInfra()
                .ConfigureServices();

            _serviceProvider = services.BuildServiceProvider();
        }


        private static IServiceCollection ConfigureInfra(this IServiceCollection services)
        {
            services.AddSingleton<ISettings>(_settings);
            services.AddSingleton<ILogger>(new LoggerConfiguration()
                 .WriteTo.Console()
                 .CreateLogger());

            return services;
        }
    
        private static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IMessageService, MessageService>();
            return services;
        }

        private static IConfiguration BuildConfiguration()
        {
            return new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile($"appsettings.json")
               .Build();
        }

        public static T GetRequiredService<T>()
        {
            return _serviceProvider.GetRequiredService<T>();
        }
    }
}
