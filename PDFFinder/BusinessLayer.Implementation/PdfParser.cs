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
        private string _metaTitle;

        public string Parse(string fileName)
        {
            try
            {
                PDFDocument document = new PDFDocument(fileName);

                _metaTitle = document.DocumentInfo.Title;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return _metaTitle;
        }
    }
}
