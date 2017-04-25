namespace PDFFinder.BusinessLayer.Contracts
{
    public interface IPdfLogger
    {
        void LogOpenForPrinting();
        void LogOpenForView();
    }
}