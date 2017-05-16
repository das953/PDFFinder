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
        public string Parse(string fileName)
        {
            try
            {
                PDFDocument document = new PDFDocument(fileName);
                return document.DocumentInfo.Title;
            }
            catch
            {
                throw;
            }
            
        }
    }
}
