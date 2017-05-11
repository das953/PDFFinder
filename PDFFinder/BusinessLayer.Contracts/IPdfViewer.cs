namespace PDFFinder.BusinessLayer.Contracts
{
    /// <summary>
    /// Просмотр файла в стандартном обозревателе (решить проблему с возможностью выбора обозревателей) 
    /// и открыть файл в выбранном - Бохенко Виталий
    /// </summary>
    public interface IPdfViewer
    {
        void View(string fileName, string processName);
    }
}