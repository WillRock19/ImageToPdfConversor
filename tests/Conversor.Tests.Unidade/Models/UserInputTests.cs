using Converter.Models;
using FluentAssertions;
using NUnit.Framework;

namespace Conversor.Tests.Unidade.Models
{
    class UserInputTests
    {
        private const string PathToSavePdfGenerated = @"C:\Test\Local_To_Save_Pdf_Files";
        private const string PdfFileName = @"PdfDefaultFileName";
        private string _expectedPdfFilePath;

        [SetUp]
        public void Initialize() => 
            _expectedPdfFilePath = $@"{PathToSavePdfGenerated}\{PdfFileName}.pdf";

        [TestCase(TestName = "GenerateGeneratedPdfCompletePath deve retornar caminho do PDF corretamente")]
        public void GenerateGeneratedPdfCompletePathShouldReturnPdfPathCorrectly()
        {
            var userInput = new UserInput
            {
                PathToSavePdfGenerated = PathToSavePdfGenerated,
                PdfFileName = PdfFileName
            };

            userInput.GeneratedPdfCompletePath().Should().Be(_expectedPdfFilePath);
        }

        [TestCase(TestName = "GenerateGeneratedPdfCompletePath deve retornar caminho correto se nome do arquivo tiver extensão '.pdf'")]
        public void GenerateGeneratedPdfCompletePathShouldReturnPdfPathCorrectlyIfFileNameHasExtensionOfFile()
        {
            var userInput = new UserInput
            {
                PathToSavePdfGenerated = PathToSavePdfGenerated,
                PdfFileName = $"{PdfFileName}.pdf"
            };

            userInput.GeneratedPdfCompletePath().Should().Be(_expectedPdfFilePath);
        }
    }
}
