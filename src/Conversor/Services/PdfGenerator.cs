using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System.Collections.Generic;
using System.Linq;

namespace Conversor.Services
{
    public class PdfGenerator
    {
        protected PdfDocument _pdfDocument { get; set; }

        protected PdfGenerator() { }

        public PdfGenerator(PdfDocument pdfDocument)
        {
            _pdfDocument = pdfDocument;
        }

        //public async Task GenerateForImages()
        //{
        //    await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
        //    var browser = await Puppeteer.LaunchAsync(new LaunchOptions
        //    {
        //        Headless = true
        //    });
        //    var page = await browser.NewPageAsync();
        //    await page.GoToAsync(@"https://www.omelete.com.br/");

        //    await page.PdfAsync(System.IO.Path.Combine(@"C:\teste_pdf\", @"teste2.pdf"));

        //    await page.GoToAsync(@"C:/William/6.Estudos/React/my-app/public/images/civil_War.jpg");
        //    await page.PdfAsync(System.IO.Path.Combine(@"C:\teste_pdf\", @"teste2.pdf"));

        //}


        public virtual void GenerateForImages(List<string> imagesPath)
        {
            var document = GenerateInitialDocumentPage(imagesPath.FirstOrDefault());

            foreach (var imagePath in imagesPath)
            {
                var image = new Image(ImageDataFactory.Create(imagePath));
                _pdfDocument.AddNewPage(new PageSize(image.GetImageWidth(), image.GetImageHeight()));
                document.Add(image);
            }
            document.Close();
        }

        public virtual void GenerateForImagesWithDocument(List<string> imagesPath, PdfDocument documentToUse)
        {
            var firstImage = new Image(ImageDataFactory.Create(imagesPath.FirstOrDefault()));
            var document = new Document(documentToUse, 
                new PageSize(firstImage.GetImageWidth(), firstImage.GetImageHeight()));

            foreach (var imagePath in imagesPath)
            {
                var image = new Image(ImageDataFactory.Create(imagePath));
                _pdfDocument.AddNewPage(new PageSize(image.GetImageWidth(), image.GetImageHeight()));
                document.Add(image);
            }
            document.Close();
        }

        private Document GenerateInitialDocumentPage(string firstImagePath)
        {
            var firstImage = new Image(ImageDataFactory.Create(firstImagePath));
            return new Document(_pdfDocument, new PageSize(firstImage.GetImageWidth(), firstImage.GetImageHeight()));
        }
    }
}
