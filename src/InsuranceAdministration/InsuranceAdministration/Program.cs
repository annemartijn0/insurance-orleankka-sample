using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using InsuranceAdministration.Commands;
using InsuranceAdministration.Domain;
using InsuranceAdministration.Queries;
using Microsoft.Azure.Cosmos.Table;
using Orleankka;
using Orleankka.Client;
using Orleankka.Cluster;
using Orleans;
using Orleans.Hosting;

namespace InsuranceAdministration
{
    public static class Program
    {
        static bool resume;
        static IClientActorSystem system;

        public static async Task Main(string[] args)
        {
            resume = args.Length == 1 && args[0] == "resume";

            Console.WriteLine("Make sure you've started Azure storage emulator!");
            Console.WriteLine("Running example. Booting cluster might take some time ...\n");

            var account = CloudStorageAccount.DevelopmentStorageAccount;
            SS.Table = await SetupTable(account);

            var host = await new SiloHostBuilder()
                .ConfigureApplicationParts(x => x
                    .AddApplicationPart(Assembly.GetExecutingAssembly())
                    .WithCodeGeneration())
                .UseOrleankka()
                .Start();

            var client = await host.Connect();
            system = client.ActorSystem();

            try
            {
                (resume ? Resume() : Run()).Wait();
            }
            catch (AggregateException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.InnerException.Message);
            }

            Console.WriteLine("\nPress any key to terminate ...");
            Console.ReadKey(true);

            host.Dispose();
            Environment.Exit(0);
        }

        static async Task<CloudTable> SetupTable(CloudStorageAccount account)
        {
            var table = account
                .CreateCloudTableClient()
                .GetTableReference("Polisadministratie");

            await table.CreateIfNotExistsAsync();
            return table;
        }

        static async Task Run()
        {
            var policy = system.ActorOf<IPolicy>("36C89BAB-7482-4C65-9F8C-F102FB2F3A2A");

            await policy.Tell(new CreatePolicy
            {
                StartDate = new DateTime(2019, 1, 1),
                EndDate = new DateTime(2019, 12, 31),
                PolicyHolder = "1",
                Coverages = 
                {
                    new Coverage
                    {
                        Id = new Guid("62D57244-3A57-4A1F-8255-A97E8A74C535"),
                        StartDate = new DateTime(2019, 1, 1),
                        EndDate = new DateTime(2019, 12, 31),
                        CoverageCode = "12345",
                        SelfInsurance = new SelfInsurance
                        {
                            Mandatory = 385
                        },
                        InsuredPerson = "1"
                    },
                    new Coverage
                    {
                        Id = new Guid("CB2B5D2A-10B4-48B9-A6FA-248B39B45F3C"),
                        StartDate = new DateTime(2019, 1, 1),
                        EndDate = new DateTime(2019, 12, 31),
                        CoverageCode = "12345",
                        SelfInsurance = new SelfInsurance
                        {
                            Mandatory = 385
                        },
                        InsuredPerson = "2"
                    }
                }
            });
            await Print(policy);
        }

        static async Task Resume()
        {
            var policy = system.ActorOf<IPolicy>("36C89BAB-7482-4C65-9F8C-F102FB2F3A2A");

            await policy.Tell(new EndCoverage(
                new Guid("62D57244-3A57-4A1F-8255-A97E8A74C535"), 
                new DateTime(2019, 6, 30)
                ));
            await Print(policy);

            await Print(policy);
        }

        static async Task Print(ActorRef item)
        {
            var details = await item.Ask<PolicyDetails>(new GetDetails());

            PrintObject(details);
            foreach (var detailsCoverage in details.Coverages)
            {
                Console.WriteLine("-- coverage --");

                PrintObject(detailsCoverage);
            }
            Console.WriteLine("");
        }

        private static void PrintObject(object obj)
        {
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(obj))
            {
                string name = descriptor.Name;
                object value = descriptor.GetValue(obj);
                Console.WriteLine("{0}={1}", name, value);
            }
        }
    }
}
