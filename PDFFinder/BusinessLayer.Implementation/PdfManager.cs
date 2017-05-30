using PDFFinder.BusinessLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFFinder.BusinessLayer.Implementation
{
    using Model;
    using System.Windows;
    /// <summary>
    /// Основной класс для обработки PDF файла, включая считывание метаданных (Parser), анализ файла (Analizer), печать файла (Printer), просмотр файла (Viewer) и запись информации об открытии или печати в базу данных (Logger). Функция Execute (string fileName) - принимает имя файла, занимается выполнением вышеперечисленных операций.
    /// </summary>
    public class PdfManager : IPdfManager
    {
        private IPdfAnalizer _analizer;
        public IPdfAnalizer Analizer
        {
            get
            {
                if (_analizer == null)
                    _analizer = new PdfAnalizer();
                return _analizer;
            }
        }

        private IPdfLogger _logger;
        public IPdfLogger Logger
        {
            get
            {
                if (_logger == null)
                    _logger = new PdfLogger();
                return _logger;
            }
        }

        private IPdfParser _parser;
        public IPdfParser Parser
        {
            get
            {
                if (_parser == null)
                    _parser = new PdfParser();
                return _parser;
            }
        }

        private IPdfPrinter _printer;
        public IPdfPrinter Printer
        {
            get
            {
                if (_printer == null)
                    _printer = new PdfPrinter();
                return _printer;
            }
        }

        private IPdfViewer _viewer;
        public IPdfViewer Viewer
        {
            get
            {
                if (_viewer == null)
                    _viewer = new PdfViewer();
                return _viewer;
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
                
                if (printerSettings != null)
                {
                    try
                    {
                        Printer.Print(fileName, printerSettings);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
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
