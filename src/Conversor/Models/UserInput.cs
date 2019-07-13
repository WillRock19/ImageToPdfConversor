namespace Converter.Models
{
    public class UserInput
    {
        public string ImagesPath { get; set; }

        public string PdfFileName { get; set; }

        public string PathToSavePdfGenerated { get; set; }

        public string GenerateGeneratedPdfCompletePath()
        {
            var cleanedFileName = PdfFileName.Replace(".pdf", string.Empty);
            return $@"{PathToSavePdfGenerated}\{cleanedFileName}.pdf";
        }
    }
}
