namespace PDFFinder.BusinessLayer.Contracts
{
    /// <summary>
    /// Служит для считывания метаданных с файла - Игорь Назаров
    /// </summary>
    public interface IPdfParser
    {
        string Parse(string fileName);
    }
}