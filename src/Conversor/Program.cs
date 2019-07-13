using Conversor.Services;
using Converter.Models;
using iText.Kernel.Pdf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO.Abstractions;

namespace Conversor
{
    class Program
    {
        private static IServiceProvider _serviceProvider;
        private static UserInput _userInput;

        static void Main(string[] args)
        {
            Console.WriteLine("Insira o caminho da pasta com as imagens.");
            var contentFolderPath = Console.ReadLine();

            Console.WriteLine("Insira o nome do arquivo que deseja gerar.");
            var resultFileName = Console.ReadLine();

            Console.WriteLine("Insira o caminho em que quer salvar o PDF gerado.");
            var pathToSavePdfGenerated = Console.ReadLine();

            _userInput = new UserInput
            {
                ImagesPath = contentFolderPath,
                PathToSavePdfGenerated = pathToSavePdfGenerated,
                PdfFileName = resultFileName
            };

            try {
                ConfigureServiceProvider();

                var manager = _serviceProvider.GetService<ConvertionManager>();
                manager.StartProcess(contentFolderPath);
            }
            catch (Exception e) {

                ILogger<Program> logger = null;

                if (_serviceProvider != null)
                    logger = _serviceProvider.GetService<ILogger<Program>>();
                else
                    logger = new Logger<Program>(new LoggerFactory());

                logger.LogError("Uma exceção interna ocorreu.");
                logger.LogError($"Exceção: {e.Message}");
            }
            Console.ReadKey();
        }

        private static void ConfigureServiceProvider()
        {
            _serviceProvider = new ServiceCollection()
                .AddLogging(LogConfiguration())
                .AddSingleton<ConvertionManager>()
                .AddSingleton<IFileSystem, FileSystem>()
                .AddSingleton<ImagesReader>()
                .AddSingleton<PdfGenerator>()
                .AddSingleton(x => new PdfDocument(new PdfWriter(_userInput.GenerateGeneratedPdfCompletePath())))
                .BuildServiceProvider();
        }

        private static Action<ILoggingBuilder> LogConfiguration() => configure 
            => configure
                .AddConsole()
                .SetMinimumLevel(LogLevel.Information);
    }
}
