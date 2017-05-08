using PDFFinder.BusinessLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFFinder.BusinessLayer.Implementation
{
    /// <summary>
    /// Основной класс для обработки PDF файла, включая считывание метаданных (Parser), анализ файла (Analizer), печать файла (Printer), просмотр файла (Viewer) и запись информации об открытии или печати в базу данных (Logger). Функция Execute (string fileName) - принимает имя файла, занимается выполнением вышеперечисленных операций.
    /// </summary>
    public class PdfManager : IPdfManager
    {
        private Model_PDFFinder context;
        public PdfManager()
        {
            context = new Model_PDFFinder();
        }
        public IPdfAnalizer Analizer
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IPdfLogger Logger
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IPdfParser Parser
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IPdfPrinter Printer
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IPdfViewer Viewer
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        /// <summary>
        /// Простой пример работы функции
        /// </summary>
        /// <param name="fileName"></param>
        public void Execute(string fileName)
        {
            string title = Parser.Parse(fileName);
            //Временная заглушка (названия процесса)
            string processName = "Some process name";
            bool availableForPrinting = Analizer.AvailableForPrinting(title, context);
            if (availableForPrinting)
            {
                Printer.Print(fileName);
                Logger.LogOpenForPrinting();
            }
            else
            {
                Viewer.View(fileName, processName);
                Logger.LogOpenForView();
            }
        }
    }
}
