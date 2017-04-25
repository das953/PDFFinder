namespace PDFFinder.BusinessLayer.Contracts
{
    public interface IPdfAnalizer
    {
        bool AvailableForPrinting(string metaData);
    }
}