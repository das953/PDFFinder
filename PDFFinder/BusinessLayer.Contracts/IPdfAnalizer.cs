namespace PDFFinder.BusinessLayer.Contracts
{
    using Model;

    /// <summary>
    /// Анализирует метаданные файла. Функция bool AvailableForPrinting(string metaData)
    /// принимает строку метаданных (Title) и сравнивает ее с записями и группами в базе данных.
    /// Определяет или файл пригоден для печати.
    /// </summary>
    public interface IPdfAnalizer
    {
        Report_Template GetPrinterSettings(string metaData, Model_PDFFinder context);
    }
}