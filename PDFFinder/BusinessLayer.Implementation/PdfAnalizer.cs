using PDFFinder.BusinessLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFFinder.BusinessLayer.Implementation
{
    using Model;

    /// <summary>
    /// Анализирует метаданные файла. Функция bool AvailableForPrinting(string metaData) принимает строку метаданных (Title) и сравнивает ее с записями и группами в базе данных (context). Определяет или файл пригоден для печати.
    /// </summary>
    public class PdfAnalizer : IPdfAnalizer
    {
        public Report_Template GetPrinterSettings(string metaData, Model_PDFFinder context)
        {
            Report_Template report = context.Report_Template.Where(x => x.report_name == metaData).FirstOrDefault();
            if (report != null)
                return report;
            Group_Template group = context.Group_Template.Where(x => metaData.Contains(x.group_name)).FirstOrDefault();
            if(group==null)
                return null;
            List<Report_Template> reports = context.Report_Template.Where(x => x.report_name.Contains(group.group_name)).ToList();
            string printerName, paperFormat;
            bool? reportDuplex;
            if (reports.Count != 0)
            {
                Report_Template defaultReport = reports.First();

               printerName = reports.All(x => x.printer_name == defaultReport.printer_name) ? defaultReport.printer_name : group.printer_name;

                reportDuplex = reports.All(x => x.duplex == defaultReport.duplex) ? defaultReport.duplex : group.duplex;

               paperFormat = reports.All(x => x.paper_format == defaultReport.paper_format) ? defaultReport.paper_format : group.paper_format;
            }
            else
            {
                printerName = group.printer_name;

                reportDuplex = group.duplex;

                paperFormat = group.paper_format;
            }
            
            Report_Template printerSettings = new Report_Template()
            {
                report_name = metaData,
                printer_name = printerName,
                duplex = reportDuplex,
                paper_format = paperFormat
            };
            return printerSettings;
        }
    }
}
