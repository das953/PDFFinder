using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFFinder.BusinessLayer.Contracts
{
    public interface IPdfManager
    {
        IPdfLogger Logger { get; }
        IPdfParser Parser { get; }
        IPdfPrinter Printer { get; }
        IPdfViewer Viewer { get; }
        IPdfAnalizer Analizer { get; }
        void Execute(string fileName);
    }
}
