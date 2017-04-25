using PDFFinder.BusinessLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFFinder.BusinessLayer.Implementation
{
    public class PdfManager : IPdfManager
    {
        public IPdfAnalizer Analizer
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IPdfLogger Logger
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IPdfParser Parser
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IPdfPrinter Printer
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IPdfViewer Viewer
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Execute(string fileName)
        {
            string title = Parser.Parse(fileName);
            bool availableForPrinting = Analizer.AvailableForPrinting(title);
            if (availableForPrinting)
                Printer.Print(fileName);
            else
                Viewer.View(fileName);
        }
    }
}
