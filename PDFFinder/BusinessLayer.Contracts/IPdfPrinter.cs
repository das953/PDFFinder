namespace PDFFinder.BusinessLayer.Contracts
{
    /// <summary>
    /// Служит для печати файла - Панибратюк Александр
    /// </summary>
    public interface IPdfPrinter
    {
        void Print(string fileName);
    }
}