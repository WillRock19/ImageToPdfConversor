using Converter.Models;
using iText.Kernel.Pdf;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Conversor.Services
{
    public class ConvertionManager
    {
        private readonly ImagesReader _imagesReader;
        private readonly ILogger<ConvertionManager> _logger;
        private readonly PdfGenerator _pdfConverter;

        public ConvertionManager(
            ImagesReader imagesReader, 
            ILogger<ConvertionManager> logger, 
            PdfGenerator pdfConverter)
        {
            _logger = logger;
            _imagesReader = imagesReader;
            _pdfConverter = pdfConverter;
        }

        public void StartProcess(string imagesOriginalFolder)
        {
            _logger.LogInformation("Iniciando processo de conversão de imagem...");
            var imagesPaths = _imagesReader.GetListOfImages(imagesOriginalFolder);

            if (imagesPaths != null)
            {
                _logger.LogInformation($"Obtidas {imagesPaths.Count} imagens.");

                _pdfConverter.GenerateForImages(imagesPaths);

                _logger.LogInformation("Finalizado processo de conversão.");
                return;
            }

            _logger.LogError($"Uma exceção ocorreu durante a obtenção das imagens.");
            _logger.LogError($"Encerrando aplicação...");

            return;
        }

        public void StartProcessForSubdirectories(string principalDirectory)
        {
            _logger.LogInformation("Iniciando processo de obtenção de subdiretórios...");
            var subdirectoriesNames = _imagesReader.GetListOfSubDirectorys(principalDirectory);

            if (subdirectoriesNames != null)
            {
                _logger.LogInformation($"Obtidos {subdirectoriesNames.Count} subdiretórios.");

                foreach (var subdirectory in subdirectoriesNames)
                {
                    var fileInformations = new UserInput
                    {
                        ImagesPath = $@"{principalDirectory}\{subdirectory}",
                        PdfFileName = $"{subdirectory.Substring(subdirectory.LastIndexOf('\\') + 1)}",
                        PathToSavePdfGenerated = $@"{principalDirectory}\01.Pdfs"
                    };

                    var imagesPaths = _imagesReader.GetListOfImages(subdirectory);

                    if (imagesPaths.Any())
                    {
                        _logger.LogInformation($"Encontradas ${imagesPaths.Count()} no diretório {subdirectory}.");
                        var pdfDocument = new PdfDocument(new PdfWriter(fileInformations.GeneratedPdfCompletePath()));

                        _logger.LogInformation($"Mineirando imagens do diretório {subdirectory}.");
                        _pdfConverter.GenerateForImagesWithDocument(imagesPaths, pdfDocument);
                        _logger.LogInformation($"Finalizando imagens do do diretório {subdirectory}.");
                    }
                    else
                        StartProcessForSubdirectories(subdirectory);
                }

                _logger.LogInformation("Finalizado processo de conversão.");
                return;
            }

            _logger.LogError($"Uma exceção ocorreu durante a obtenção das imagens.");
            _logger.LogError($"Encerrando aplicação...");

            return;
        }
    }
}
