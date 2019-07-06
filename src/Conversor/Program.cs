using Conversor.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO.Abstractions;

namespace Conversor
{
    class Program
    {
        private static IServiceProvider _serviceProvider;

        static void Main(string[] args)
        {
            ConfigureServiceProvider();

            //var logger = _serviceProvider.GetService<ILogger<Program>>();
            //logger.LogInformation("Aplicação iniciada com sucesso!");

            Console.WriteLine("Insira o caminho da pasta com as imagens.");
            var contentFolderPath = Console.ReadLine();

            Console.WriteLine("Insira o nome do arquivo que deseja gerar.");
            var resultFileName = Console.ReadLine();

            Console.WriteLine("Insira o caminho em que quer salvar o PDF gerado.");
            var resultPdfFolderPath = Console.ReadLine();

            var manager = _serviceProvider.GetService<ConvertionManager>();
            manager.StartProcess(contentFolderPath, resultFileName, resultPdfFolderPath);

            Console.ReadKey();
        }

        private static void ConfigureServiceProvider()
        {
            _serviceProvider = new ServiceCollection()
                .AddLogging(LogConfiguration())
                .AddSingleton<ConvertionManager>()
                .AddSingleton<IFileSystem, FileSystem>()
                .AddSingleton<ImagesReader>()
                .AddSingleton<PdfConverter>()
                .BuildServiceProvider();
        }

        private static Action<ILoggingBuilder> LogConfiguration() => configure 
            => configure
                .AddConsole()
                .SetMinimumLevel(LogLevel.Information);
    }
}
