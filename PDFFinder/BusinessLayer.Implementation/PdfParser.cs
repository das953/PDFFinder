using PDFFinder.BusinessLayer.Contracts;
using PDFMosaic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PDFFinder.BusinessLayer.Implementation
{
    /// <summary>
    /// Служит для считывания метаданных с файла - Игорь Назаров
    /// </summary>
    public class PdfParser : IPdfParser
    {
        public string MetaTitle;
        public string Parse(string fileName)
        {
            parsing(fileName);
            throw new NotImplementedException();
        }

        private void parsing(string filename)
        {
            try
            {
                PDFDocument document = new PDFDocument(filename);

                //Можливість змінювати мета данні у PDF документі 

                /*document.DocumentInfo.Subject = "Document Info";
                document.DocumentInfo.Title = "Custom title";
                document.DocumentInfo.Keywords = "pdf, PDFMosaic";
                document.DocumentInfo.Author = "PDF Mosaic";*/

                // PDFPage page = new PDFPage(PDFPaperFormat.A4);
                // PDFFont font = new PDFFont(PDFStandardFont.Helvetica, 16);
                // PDFBrush brush = new PDFSolidBrush();
                //page.Canvas.DrawString("Check document properties", font, brush, 100, 100);

                //document.Pages.Add(page);
                //document.Save("DocumentInfo.pdf", true);

                MetaTitle = document.DocumentInfo.Title;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
