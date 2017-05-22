namespace PDFFinder.BusinessLayer.Contracts
{
    /// <summary>
    /// Служит для считывания метаданных с файла - Игорь Назаров
    /// </summary>
    public interface IPdfParser
    {
        string MetaTitle { get; set; }
        string Parse(string fileName);
    }
}