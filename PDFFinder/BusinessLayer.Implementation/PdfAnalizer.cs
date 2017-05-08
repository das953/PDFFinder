using PDFFinder.BusinessLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFFinder.BusinessLayer.Implementation
{
    /// <summary>
    /// Анализирует метаданные файла. Функция bool AvailableForPrinting(string metaData) принимает строку метаданных (Title) и сравнивает ее с записями и группами в базе данных (context). Определяет или файл пригоден для печати.
    /// </summary>
    public class PdfAnalizer : IPdfAnalizer
    {
        public Report_Template GetPrinterSettings(string metaData, Model_PDFFinder context)
        {
            throw new NotImplementedException();
        }
    }
}
