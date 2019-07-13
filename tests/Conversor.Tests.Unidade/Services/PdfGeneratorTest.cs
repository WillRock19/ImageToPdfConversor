using Conversor.Services;
using FluentAssertions;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;

namespace Conversor.Tests.Unidade.Services
{
    class PdfGeneratorTest
    {
        private AutoMocker _autoMock;
        private PdfGenerator _pdfGenerator;

        [SetUp]
        public void Initialize()
        {
            _autoMock = new AutoMocker();

            _autoMock.Setup<PdfDocument, PdfPage>(x => x.AddNewPage(It.IsAny<PageSize>()))
                .Returns((PdfPage)null);

            _pdfGenerator = _autoMock.CreateInstance<PdfGenerator>();
        }

        [TestCase(TestName = "GenerateForImages deve criar um Pdf com a mesma quantidade de páginas que a quantidade de imagens")]
        public void GenerateForImagesShouldGeneratePdfWithSameNumberOfPagesAsTheImagesListCount()
        {
            false.Should().BeTrue();
        }
    }
}
