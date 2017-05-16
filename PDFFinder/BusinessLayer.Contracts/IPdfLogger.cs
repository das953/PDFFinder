namespace PDFFinder.BusinessLayer.Contracts
{
    public interface IPdfLogger
    {
        void LogOpenForPrinting(string GroupName);
        void LogOpenForView(string GroupName);
    }
}