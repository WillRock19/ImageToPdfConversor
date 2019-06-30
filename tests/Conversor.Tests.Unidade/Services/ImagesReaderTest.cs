﻿using Conversor.Services;
using FluentAssertions;
using Moq.AutoMock;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;

namespace Conversor.Tests.Unidade.Services
{
    public class ImagesReaderTest
    {
        public const string Image1Name = "Image1.jpg";
        public const string Image2Name = "Image2.jpg";
        public const string IgnoredImageName = "Image3.png";
        public const string IgnoredTextFileName = "myfile.txt";
        public const string TestDirectory = @"c:\test\";

        private AutoMocker _autoMocker;
        private ImagesReader _imagesReader;
        private MockFileSystem _mockFileSystem;

        [SetUp]
        public void Inicializar()
        {
            _autoMocker = new AutoMocker();

            _mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { $"{TestDirectory}{IgnoredTextFileName}", new MockFileData("Testing is meh.") },
                { $"{TestDirectory}{Image1Name}", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) },
                { $"{TestDirectory}{Image2Name}", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) },
                { $"{TestDirectory}{IgnoredImageName}", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) }
            });

            _imagesReader = new ImagesReader(_mockFileSystem);
        }

        [TestCase(TestName = "GetListOfImages deve retornar lista vazia caso não encontre imagens no diretório informado.")]
        public void DeveRetornarListaVaziaCasoNaoEncontreImagensNoDiretorioInformado()
        {
            _mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { $"{TestDirectory}{IgnoredTextFileName}", new MockFileData("Testing is meh.") },
            });

            var newImagesReader = new ImagesReader(_mockFileSystem);
            var imagens = newImagesReader.GetListOfImages(TestDirectory);

            imagens.Should().BeEmpty();
        }

        [TestCase(TestName = "GetListOfImages não deve retornar imagens de subdiretórios.")]
        public void DeveRetornarImagensApenasDoDiretorioDeBuscaAtual()
        {
            _mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { $@"{TestDirectory}{Image1Name}", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) },
                { $@"{TestDirectory}\subdiretorio\{Image2Name}", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) }
            });

            var newImagesReader = new ImagesReader(_mockFileSystem);
            var imagens = newImagesReader.GetListOfImages(TestDirectory);

            imagens.Count.Should().Be(1);
            imagens.Should().NotContain(x => x.Contains(Image2Name));
        }

        [TestCase(TestName = "GetListOfImages deve retornar lista com informações das imagens no diretório especificado.")]
        public void DeveRetornarListaComInformacoesDasImagens()
        {
            var imagens = _imagesReader.GetListOfImages(TestDirectory);

            imagens.Count.Should().Be(2);
            imagens.Should().Contain(x => x.Contains(Image1Name));
            imagens.Should().Contain(x => x.Contains(Image2Name));
        }

        [TestCase(TestName = "GetListOfImages deve retornar apenas imagens com extensão 'JPG'.")]
        public void DeveRetornarApenasImagensComExtensaoJPG()
        {
            var imagens = _imagesReader.GetListOfImages(TestDirectory);

            imagens.Should().NotContain(x => x.Equals(IgnoredImageName));
            imagens.Should().NotContain(x => x.Equals(IgnoredTextFileName));
        }

        [TestCase(TestName = "GetListOfImages deve retornar nulo caso uma exceção seja estourada")]
        public void DeveRetornarNuloCasoUmaExcecaoOcorra()
        {
            //TODO: implement rule
            false.Should().BeTrue();
        }
    }
}
