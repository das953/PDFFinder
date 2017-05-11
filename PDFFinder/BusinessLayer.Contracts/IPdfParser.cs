namespace PDFFinder.BusinessLayer.Contracts
{
    public interface IPdfParser
    {
        string MetaTitle { get; set; }
        string Parse(string fileName);
    }
}