using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;

namespace Conversor.Services
{
    public class ImagesReader
    {
        private const string filesExtension = "*.jpg";
        private readonly IFileSystem _fileSystem;

        public ImagesReader(IFileSystem fileSystem) => 
            _fileSystem = fileSystem;

        public List<string> GetListOfImages(string folderPath)
        {
            try
            {
                var imagesNames = _fileSystem.Directory.GetFiles(folderPath, filesExtension, SearchOption.TopDirectoryOnly);
                var imagesFullPath = new List<string>();

                return imagesNames.ToList();
            }
            catch (Exception e) {
                return null;
            }
        }
    }
}
