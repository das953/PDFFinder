using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFFinder.BusinessLayer.Contracts
{
    /// <summary>
    /// Основной класс для обработки PDF файла, включая считывание метаданных (Parser), анализ файла (Analizer), печать файла (Printer), просмотр файла (Viewer) и запись информации об открытии или печати в базу данных (Logger). Функция Execute (string fileName) - принимает имя файла, занимается выполнением вышеперечисленных операций.
    /// </summary>
    public interface IPdfManager
    {
        IPdfLogger Logger { get; }
        IPdfParser Parser { get; }
        IPdfPrinter Printer { get; }
        IPdfViewer Viewer { get; }
        IPdfAnalizer Analizer { get; }
        void Execute(string fileName);
    }
}
