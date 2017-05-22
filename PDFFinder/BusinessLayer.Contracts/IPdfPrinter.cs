namespace PDFFinder.BusinessLayer.Contracts
{

    using Model;
    /// <summary>
    /// Служит для печати файла - Панибратюк Александр
    /// </summary>
    public interface IPdfPrinter
    {
        void Print(string fileName, Report_Template printerSettings);
    }
}