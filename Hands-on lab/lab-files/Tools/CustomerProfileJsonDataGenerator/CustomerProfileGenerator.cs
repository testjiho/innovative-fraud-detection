using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using CustomerProfileJsonDataGenerator.Interfaces;
using CustomerProfileJsonDataGenerator.Models;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace CustomerProfileJsonDataGenerator
{
    internal class CustomerProfileGenerator : IHostedService
    {
        readonly IBlobStorageService _blobStorage;
        private readonly Dictionary<string, Task> _fileTasks = new Dictionary<string, Task>();
        private readonly List<AccountData> accounts = new List<AccountData>();
        private const string ContainerName = "mcw-cosmosdb";
        private const string ProfileFolder1 = "accounts";
        private const string ProfileFolder2 = "purchases";

        public CustomerProfileGenerator(
            IBlobStorageService blobStorage)
        {
            _blobStorage = blobStorage;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Starting to generate...");

            // Using randomizer seed to generate repeatable data sets.
            var randomizer = new Randomizer(8675309);
            var serializer = new JsonSerializer { Formatting = Formatting.Indented };

            // Get user account seed data:
            Console.WriteLine("Retrieving account data...");

            using (var reader = new StreamReader(File.OpenRead(@"account-data.csv")))
            {
                var header = reader.ReadLines()
                    .First();
                var lines = reader.ReadLines()
                    .Skip(1);

                // Instantiate an AccountData object from the CSV line and header data, using the supplied factory:
                accounts.AddRange(lines.Select(line => AccountData.FromString(line, header)));
            }

            // Create directories for both sets of files:
            Directory.CreateDirectory(ProfileFolder1);
            Directory.CreateDirectory(ProfileFolder2);

            _fileTasks.Add("1", CreateUserAccountSourceFiles(serializer, randomizer));
            _fileTasks.Add("2", CreateEcommerceSourceFiles(serializer, randomizer));

            var tasks = _fileTasks.Select(t => t.Value).ToList();
            while (tasks.Count > 0)
            {
                try
                {
                    Task.WhenAll(tasks).Wait();
                }
                catch (TaskCanceledException)
                {
                    //expected
                }

                tasks = _fileTasks.Where(t => !t.Value.IsCompleted).Select(t => t.Value).ToList();
            }

            Console.WriteLine("Finished.");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async Task CreateUserAccountSourceFiles(JsonSerializer serializer, Randomizer randomizer)
        {
            var userList = new List<User>();
            var fileNum = 1;

            const int recordsPerFile = 1000;

            foreach (var accountData in accounts)
            {
                var testUser = new Faker<User>()
                    // Optional: Call for objects that have complex initialization
                    .CustomInstantiator(f => new User(randomizer, accountData));
                    // Optional: After all rules are applied finish with the following action
                    //.FinishWith((f, u) => { Console.WriteLine($"User Created! Username={u.UserName}"); });
                userList.Add(testUser.Generate());

                if (userList.Count % recordsPerFile == 0)
                {
                    await WriteUserAccountsFile(userList, fileNum++, serializer);
                    userList.Clear();
                }
            }

            if (userList.Count > 0)
            {
                await WriteUserAccountsFile(userList, fileNum++, serializer);
            }
        }

        private async Task CreateEcommerceSourceFiles(JsonSerializer serializer, Randomizer randomizer)
        {
            var productPurchasesList = new List<ProductPurchases>();
            var fileNum = 1;

            const int recordsPerFile = 1000;

            foreach (var accountData in accounts)
            {
                var id = accountData.AccountID;
                var purchases = new Faker<ProductPurchases>()
                    // Optional: Call for objects that have complex initialization
                    .CustomInstantiator(f => new ProductPurchases(randomizer, id));
                var item = purchases.Generate();
                productPurchasesList.Add(item);

                if (productPurchasesList.Count % recordsPerFile == 0)
                {
                    await WriteProductPurchasesFile(productPurchasesList, fileNum++, serializer);
                    productPurchasesList.Clear();
                }
            }

            if (productPurchasesList.Count > 0)
            {
                await WriteProductPurchasesFile(productPurchasesList, fileNum++, serializer);
            }
        }

        private async Task WriteUserAccountsFile(List<User> userList, int fileNum, JsonSerializer serializer)
        {
            var fileName = $"{fileNum:0000}.json";
            await _blobStorage.SetFileContentAsString(ContainerName, @$"{ProfileFolder1}\{fileName}",
                JsonConvert.SerializeObject(userList, Formatting.Indented));
            Console.WriteLine($"Created user account file: {fileName}");
        }

        private async Task WriteProductPurchasesFile(List<ProductPurchases> productPurchasesList, int fileNum, JsonSerializer serializer)
        {
            var fileName = $"{fileNum:0000}.json";
            //await using var file = File.CreateText($@"{ProfileFolder2}\{fileName}");
            //serializer.Serialize(file, productPurchasesList);
            await _blobStorage.SetFileContentAsString(ContainerName, @$"{ProfileFolder2}\{fileName}",
                JsonConvert.SerializeObject(productPurchasesList, Formatting.Indented));
            Console.WriteLine($"Created product purchases file: {fileName}");
        }
    }
}