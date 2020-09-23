using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using Bogus.DataSets;
using Bogus.Extensions;
using CustomerProfileJsonDataGenerator.Interfaces;
using CustomerProfileJsonDataGenerator.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace CustomerProfileJsonDataGenerator
{
    class Program
    {
        private static readonly CancellationTokenSource CancellationSource = new CancellationTokenSource();

        static async Task Main(string[] args)
        {
            var cancellationToken = CancellationSource.Token;
            await CreateHostBuilder(args).Build().RunAsync(cancellationToken);
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<CustomerProfileGenerator>();

                    services.Configure<AzureBlobStorageServiceSettings>(hostContext.Configuration.GetSection("CustomerProfileGeneratorAzureBlobStorageService"));

                    services.AddTransient<IBlobStorageService, AzureBlobStorageService>();
                });
    }
}
