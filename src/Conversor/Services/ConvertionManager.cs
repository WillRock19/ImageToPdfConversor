using Microsoft.Extensions.Logging;

namespace Conversor.Services
{
    public class ConvertionManager
    {
        private readonly ImagesReader _imagesReader;
        private readonly ILogger<ConvertionManager> _logger;
        private readonly PdfConverter _pdfConverter;

        public ConvertionManager(ImagesReader imagesReader, ILogger<ConvertionManager> logger, PdfConverter pdfConverter)
        {
            _logger = logger;
            _imagesReader = imagesReader;
            _pdfConverter = pdfConverter;
        }

        public void StartProcess(string imagesOriginalFolder, string pdfFileName, string folderToPlacePdf)
        {
            _logger.LogInformation("Iniciando processo de conversão de imagem...");
            var imagesPaths = _imagesReader.GetListOfImages(imagesOriginalFolder);

            if (imagesPaths != null)
            {
                _logger.LogInformation($"Obtidas {imagesPaths.Count} imagens.");

                /*IMPLEMENTAR LÓGICA QUE GERA O PDF*/

                _logger.LogInformation("Finalizado processo de conversão.");
            }

            _logger.LogError($"Uma exceção ocorreu durante a obtenção das imagens.");
            _logger.LogError($"Encerrando aplicação...");

            return;
        }
    }
}
