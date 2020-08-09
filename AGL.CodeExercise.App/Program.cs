using System;
using System.IO;
using AGL.CodeExercise.Common.ApplicationConfiguration;
using AGL.CodeExercise.ServiceManagers;
using AGL.CodeExercise.ServiceManagers.ServiceManagers;
using AGL.CodeExercise.Services.Pets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AGL.CodeExercise.App
{
    class Program
    {
        static IConfigurationRoot _configurationRoot;
        static IServiceCollection _services;

        static void Main(string[] args)
        {
            //Initiate ConfigurationRoot from ConfigurationBuilder
            InitiateConfigurationRoot();
            //Initiate container for injection
            InitiateContainer();
            using var serviceProvider = _services.BuildServiceProvider();
            //Starting the entry controller and get the cats grouped by owners
            serviceProvider.GetService<IPetsController>().GetCatsPerOwnerGender().Wait();
        }

        /// <summary>
        /// Initiates the configuration root using config builder and define appsettings.json.
        /// </summary>
        private static void InitiateConfigurationRoot()
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            _configurationRoot = builder.Build();
        }

        /// <summary>
        /// Initiates the container for DI
        /// </summary>
        private static void InitiateContainer()
        {
            _services = new ServiceCollection();

            _services.AddSingleton(Console.Out);
            _services.AddSingleton(Console.In);

            _services.AddSingleton(_configurationRoot);
            _services.AddOptions();

            _services.AddSingleton(new LoggerFactory());
            _services.AddLogging();

            _services.AddHttpClient<IOwnerPetsService, OwnerPetsService>();
            _services.AddTransient<IOwnerPetsServiceManager, OwnerPetsServiceManager>();
            _services.AddTransient<IPetsController, PetsController>();

            _services.Configure<AppSettings>(options => _configurationRoot.GetSection("AppSettings").Bind(options));
            _services.AddSingleton(sp => sp.GetService<IOptions<AppSettings>>().Value);
        }
    }
}
