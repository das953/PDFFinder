namespace PDFFinder.BusinessLayer.Contracts
{
    public interface IPdfParser
    {
        string Parse(string fileName);
    }
}