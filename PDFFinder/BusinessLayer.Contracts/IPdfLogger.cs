namespace PDFFinder.BusinessLayer.Contracts
{
    /// <summary>
    /// Записивыает в базу данных информацию и время открытия файла для просмотра или для печати (необходимо согласовать с разработчиком базы данных, какие именно данные будут записиваться)
    /// </summary>
    public interface IPdfLogger
    {
        void LogOpenForPrinting(string GroupName);
        void LogOpenForView();
    }
}