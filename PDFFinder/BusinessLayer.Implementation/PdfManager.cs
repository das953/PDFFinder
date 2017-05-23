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
    /// Основной класс для обработки PDF файла, включая считывание метаданных (Parser), анализ файла (Analizer), печать файла (Printer), просмотр файла (Viewer) и запись информации об открытии или печати в базу данных (Logger). Функция Execute (string fileName) - принимает имя файла, занимается выполнением вышеперечисленных операций.
    /// </summary>
    public class PdfManager : IPdfManager
    {
        public IPdfAnalizer Analizer
        {
            get
            {
                return new PdfAnalizer();
            }
        }

        public IPdfLogger Logger
        {
            get
            {
                return new PdfLogger();
            }
        }

        public IPdfParser Parser
        {
            get
            {
                return new PdfParser();
            }
        }

        public IPdfPrinter Printer
        {
            get
            {
                return new PdfPrinter();
            }
        }

        public IPdfViewer Viewer
        {
            get
            {
                return new PdfViewer();
            }
        }
        /// <summary>
        /// Простой пример работы функции
        /// </summary>
        /// <param name="fileName"></param>
        public void Execute(string fileName)
        {
            string title = Parser.Parse(fileName);
            FileAssociationManager associationManager = new FileAssociationManager();

            string processName = associationManager.GetAssociatedApplication(".pdf").Path;
            using (var context = new Model_PDFFinder())
            {
                Report_Template printerSettings = Analizer.GetPrinterSettings(title, context);

                if (printerSettings != null || title != string.Empty)
                {
                    var NewGroup = new Group_Template
                    {
                        duplex = false,
                        paper_format = "A4",
                        group_name = title.Substring(0, 5),
                        printer_name = System.Drawing.Printing.PrinterSettings.InstalledPrinters[0]
                    };


                    if (printerSettings == null)
                    {
                        context.Group_Template.Add(NewGroup);
                        context.SaveChanges();
                    }
                        
                 
                    Printer.Print(fileName, printerSettings != null ? printerSettings : new Report_Template
                    {
                     duplex = NewGroup.duplex,
                     paper_format = NewGroup.paper_format,
                     printer_name = NewGroup.printer_name,
                     report_name = title
                    });
                    Logger.LogOpenForPrinting(title);
                    App.Current.Shutdown();
                }
                else
                {
                    Viewer.View(fileName, processName);
                    Logger.LogOpenForView();
                }
            }
        }
    }
}
