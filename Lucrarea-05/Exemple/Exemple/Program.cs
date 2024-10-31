using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderWorkflow.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderWorkflow
{
    internal class Program
    {
        private static readonly string ConnectionString = "Server=localhost\\SQLExpress;Database=Student;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true";

        private static async Task Main(string[] args)
        {
            using ILoggerFactory loggerFactory = ConfigureLoggerFactory();
            var dbContextOptions = new DbContextOptionsBuilder<OrderContext>()
                .UseSqlServer(ConnectionString)
                .UseLoggerFactory(loggerFactory)
                .Options;

            using var context = new OrderContext(dbContextOptions);

            // Apelăm SeedDatabase pentru a popula baza de date
            await SeedDatabase(context);

            Console.WriteLine("Programul s-a terminat cu succes.");
        }

        private static async Task SeedDatabase(OrderContext context)
        {
            // Verificăm dacă există date în baza de date pentru a evita duplicatele
            if (!await context.Products.AnyAsync() &&
                !await context.OrderHeaders.AnyAsync() &&
                !await context.OrderLines.AnyAsync())
            {
                Console.WriteLine("Populăm baza de date cu date inițiale...");

                // 1. Adăugăm produse în tabelul Product fără a seta ProductId
                var products = new List<Product>
                {
                    new Product { Code = "PROD001", Stoc = 100 },
                    new Product { Code = "PROD002", Stoc = 50 },
                    new Product { Code = "PROD003", Stoc = 200 }
                };
                context.Products.AddRange(products);
                await context.SaveChangesAsync();

                // 2. Adăugăm comenzi în tabelul OrderHeader fără a seta OrderId
                var orderHeaders = new List<OrderHeader>
                {
                    new OrderHeader { Address = "123 Main St, City A", Total = 300.00m },
                    new OrderHeader { Address = "456 Elm St, City B", Total = 450.00m }
                };
                context.OrderHeaders.AddRange(orderHeaders);
                await context.SaveChangesAsync();

                // 3. Adăugăm linii de comandă în tabelul OrderLine fără a seta OrderLineId
                var orderLines = new List<OrderLine>
                {
                    new OrderLine { OrderId = orderHeaders[0].OrderId, ProductId = products[0].ProductId, Quantity = 2, Price = 100.00m },
                    new OrderLine { OrderId = orderHeaders[0].OrderId, ProductId = products[1].ProductId, Quantity = 1, Price = 200.00m },
                    new OrderLine { OrderId = orderHeaders[1].OrderId, ProductId = products[0].ProductId, Quantity = 3, Price = 100.00m },
                    new OrderLine { OrderId = orderHeaders[1].OrderId, ProductId = products[2].ProductId, Quantity = 1, Price = 150.00m }
                };
                context.OrderLines.AddRange(orderLines);
                await context.SaveChangesAsync();

                Console.WriteLine("Datele inițiale au fost adăugate cu succes în baza de date.");
            }
            else
            {
                Console.WriteLine("Baza de date conține deja date.");
            }
        }

        private static ILoggerFactory ConfigureLoggerFactory()
        {
            return LoggerFactory.Create(builder =>
                builder.AddSimpleConsole(options =>
                {
                    options.IncludeScopes = true;
                    options.SingleLine = true;
                    options.TimestampFormat = "hh:mm:ss ";
                }).AddDebug());
        }
    }
}
