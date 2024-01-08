using Domain.Models;
using LanguageExt;
using static LanguageExt.Prelude;
using proiectPSSC.Domain.Commands;
using proiectPSSC.Domain.Models;
using proiectPSSC.Domain.Workflows;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using Microsoft.Extensions.Logging.Console;
using Microsoft.EntityFrameworkCore;
using proiectPSSC.Data;
using proiectPSSC.Data.DataRepositories;

namespace proiectPSSC;

class Program
{
    private static string ConnectionString = "Server=127.0.0.1,1433;User Id=sa;Password=P@ssw0rd123;TrustServerCertificate=True;Database=PSSC";

    static async Task Main(string[] args)
    {
        using ILoggerFactory loggerFactory = ConfigureLoggerFactory();
        ILogger<AddProductsToCartWorkflow> logger = loggerFactory.CreateLogger<AddProductsToCartWorkflow>();
        ILogger<InvoiceCartWorkflow> logger1 = loggerFactory.CreateLogger<InvoiceCartWorkflow>();

        var listOfGrades = ReadListOfProducts().ToArray();
        PaymentCartCommand command = new PaymentCartCommand(listOfGrades);

        var dbContextBuilder = new DbContextOptionsBuilder<ProductsContext>()
                                            .UseSqlServer(ConnectionString)
                                            .UseLoggerFactory(loggerFactory);

        var dbContextBuilder1 = new DbContextOptionsBuilder<InvoiceContext>()
                                            .UseSqlServer(ConnectionString)
                                            .UseLoggerFactory(loggerFactory);

        ProductsContext productsContext = new ProductsContext(dbContextBuilder.Options);
        ClientsRepository clientsRepository = new ClientsRepository(productsContext);
        ProductsRepository productsRepository = new ProductsRepository(productsContext);
        InvoiceContext invoiceContext = new InvoiceContext(dbContextBuilder1.Options);
        InvoiceRepository invoiceRepository = new InvoiceRepository(invoiceContext);

        AddProductsToCartWorkflow workflow = new AddProductsToCartWorkflow(clientsRepository, productsRepository, logger);
        var result = await workflow.ExecuteAsync(command);

        InvoiceCartWorkflow workflow1 = new InvoiceCartWorkflow(invoiceRepository, logger1);
        //var result1 = await workflow1.GenerateInvoice();

        result.Match(
                whenPayCartFailedEvent: @event =>
                {
                    Console.WriteLine($"Payment failed: {@event.Reason}");
                    return @event;
                },
                whenPayCartSucceededEvent: @event =>
                {
                    Console.WriteLine($"Payment succeeded.");

                    Console.WriteLine(@event.Csv);
                    return @event;
                }
            );
    }

    private static ILoggerFactory ConfigureLoggerFactory()
    {
        return LoggerFactory.Create(builder =>
                            builder.AddSimpleConsole(options =>
                            {
                                options.IncludeScopes = true;
                                options.SingleLine = true;
                                options.TimestampFormat = "hh:mm:ss ";
                            })
                            .AddProvider(new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider()));
    }

    private static List<UnvalidatedProduct> ReadListOfProducts()
    {
        List<UnvalidatedProduct> listOfProducts = new();
        do
        {
            //read registration number and grade and create a list of products
            var codeNumber = ReadValue("Client Code Number: ");
            if (string.IsNullOrEmpty(codeNumber))
            {
                break;
            }

            var quantity = ReadValue("Quantity: ");
            if (string.IsNullOrEmpty(quantity))
            {
                break;
            }

            var price = ReadValue("Price: ");
            if (string.IsNullOrEmpty(price))
            {
                break;
            }

            listOfProducts.Add(new(codeNumber, quantity, price));
        } while (true);
        return listOfProducts;
    }

    private static string? ReadValue(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine();
    }
}
