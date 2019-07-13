using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;

namespace Conversor.Services
{
    public class ImagesReader
    {
        private const string FilesExtension = "*.jpg";
        private readonly IFileSystem _fileSystem;

        protected ImagesReader() { }

        public ImagesReader(IFileSystem fileSystem) => 
            _fileSystem = fileSystem;

        public virtual List<string> GetListOfImages(string folderPath)
        {
            try
            {
                var imagesNames = _fileSystem
                    .Directory.GetFiles(folderPath, FilesExtension, SearchOption.TopDirectoryOnly);

                return imagesNames.ToList();
            }
            catch (Exception e) {
                return null;
            }
        }
    }
}
