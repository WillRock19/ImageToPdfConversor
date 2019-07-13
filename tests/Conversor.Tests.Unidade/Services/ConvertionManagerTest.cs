using Conversor.Services;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using System.Collections.Generic;

namespace Conversor.Tests.Unidade.Services
{
    class ConvertionManagerTest
    {
        private const string ImagesFolderPath = @"C:\Images_folder";
        private const string PdfFileName = @"FileName";
        private const string FolderToPlacePdf = @"C:\Folder_To_Place_Pdf";

        private AutoMocker _autoMock;
        private ConvertionManager _convertionManager;
        private new List<string> _imagesPathList;

        [SetUp]
        public void Initialize()
        {
            _imagesPathList = new List<string>()
            {
                "path1",
                "path2",
                "path3"
            };

            _autoMock = new AutoMocker();

            _autoMock
                .Setup<ImagesReader, List<string>>(x => x.GetListOfImages(ImagesFolderPath))
                .Returns(_imagesPathList);

            _autoMock.Setup<PdfGenerator>(x => x.GenerateForImages(_imagesPathList));

            _convertionManager = _autoMock.CreateInstance<ConvertionManager>();
        }

        [TestCase(TestName = "StartProcess deve chamar GetListOfImages do serviço imagesReader")]
        public void StartProcessShouldCallGetListOfImages()
        {
            _convertionManager.StartProcess(ImagesFolderPath);

            _autoMock.GetMock<ImagesReader>()
                .Verify(x => x.GetListOfImages(ImagesFolderPath), Times.Once); 
        }

        [TestCase(TestName = "StartProcess deve chamar GenerateForImages caso lista de imagens não seja nula")]
        public void StartProcessShouldCallGenerateForImagesIfImagesListIsNotNull()
        {
            _convertionManager.StartProcess(ImagesFolderPath);

            _autoMock.GetMock<PdfGenerator>()
                .Verify(x => x.GenerateForImages(_imagesPathList), Times.Once);
        }

        [TestCase(TestName = "StartProcess não deve chamar GenerateForImages caso lista de imagens seja nula")]
        public void StartProcessShouldNotCallGenerateForImagesIfImagesListIsNull()
        {
            _autoMock
                .Setup<ImagesReader, List<string>>(x => x.GetListOfImages(ImagesFolderPath))
                .Returns((List<string>)null);

            _convertionManager.StartProcess(ImagesFolderPath);

            _autoMock.GetMock<PdfGenerator>()
                .Verify(x => x.GenerateForImages(It.IsAny<List<string>>()), Times.Never);
        }

        [TestCase(TestName = "StartProcess deve loggar erro caso lista de imagens seja nula")]
        public void StartProcessShouldLogErrorIfImagesListIsNull()
        {
            _autoMock
                .Setup<ImagesReader, List<string>>(x => x.GetListOfImages(ImagesFolderPath))
                .Returns((List<string>)null);

            false.Should().BeTrue();
        }
    }
}
