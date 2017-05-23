using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spire.Pdf;
using Spire.Pdf.Annotations;
using Spire.Pdf.Widget;
using System.Drawing.Printing;
using System.Windows.Controls;
using System.Printing;
using System.Windows.Documents;
using System.Windows;
using PDFFinder.BusinessLayer.Contracts;
using PDFFinder.Model;


namespace PDFFinder.BusinessLayer.Implementation
{

    
    /// <summary>
    /// Служит для печати файла - Панибратюк Александр
    /// </summary>
    public class PdfPrinter : IPdfPrinter
    {
        Dictionary<string, PageMediaSizeName> pageSizes;
        public PdfPrinter()
        {
            //Dictionary for paper formats
            pageSizes = new Dictionary<string, PageMediaSizeName>{
                { "A4", PageMediaSizeName.ISOA4 },
                { "A3", PageMediaSizeName.ISOA3 },
                { "A5", PageMediaSizeName.ISOA5 }
            };
        }
        public void Print(string fileName, Report_Template printerSettings)
        {
            PdfDocument doc = new PdfDocument();
            doc.LoadFromFile(fileName);

            PrintDialog dialogPrint = new PrintDialog();

            //If printerSettings==null -> open default PrintDialog
            if (printerSettings!=null)
            {
                LocalPrintServer localPrintServer = new LocalPrintServer();

                //Get all printers
                PrintQueueCollection localPrinterCollection = localPrintServer.GetPrintQueues();

                if (localPrinterCollection.Count() == 0)
                {
                    MessageBox.Show("No available printers", "Printing error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                //Get printer from printerSettings (stored in database)
                PrintQueue printQueue = (from printer in localPrinterCollection where printer.Name == printerSettings.printer_name select printer).FirstOrDefault();

                if (printQueue != null)
                {
                    dialogPrint.PrintQueue = new PrintQueue(localPrintServer, printQueue.Name, PrintSystemDesiredAccess.AdministratePrinter);
                }
                else
                {
                    dialogPrint.PrintQueue = new PrintQueue(localPrintServer, localPrintServer.DefaultPrintQueue.Name, PrintSystemDesiredAccess.AdministratePrinter);
                }

                //Setting printer and pages
                PrintTicket deltaPrintTicket = GetPrintTicketFromPrinter(dialogPrint.PrintQueue, printerSettings);

                var result = dialogPrint.PrintQueue.MergeAndValidatePrintTicket(dialogPrint.PrintQueue.UserPrintTicket,
                    deltaPrintTicket);
                
                dialogPrint.PrintQueue.UserPrintTicket = result.ValidatedPrintTicket;
                dialogPrint.PrintQueue.DefaultPrintTicket = result.ValidatedPrintTicket;
                dialogPrint.PrintTicket = result.ValidatedPrintTicket;
                dialogPrint.PrintQueue.Commit();
            }

            PrintDocument printDoc = doc.PrintDocument;
            
            if (dialogPrint.ShowDialog() == true)
            {
                //Printer
                printDoc.PrinterSettings.PrinterName = dialogPrint.PrintQueue.Name;

                //Duplexing
                printDoc.PrinterSettings.Duplex = dialogPrint.PrintTicket.Duplexing == Duplexing.TwoSidedShortEdge ? Duplex.Vertical : Duplex.Simplex;

                //Page size
                PageMediaSize pageSize = dialogPrint.PrintTicket.PageMediaSize;
                string name = (from p in pageSizes where p.Value == pageSize.PageMediaSizeName select p.Key).FirstOrDefault();
                if(name!=null)
                {
                    printDoc.DefaultPageSettings.PaperSize = new PaperSize(name, (int)pageSize.Width, (int)pageSize.Height);
                }
                 
                try
                {
                    printDoc.Print();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Print Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private PrintTicket GetPrintTicketFromPrinter(PrintQueue printQueue, Report_Template printerSettings)
        {

            PrintTicket printTicket = printQueue.DefaultPrintTicket;
            PageMediaSize mediaSize = new PageMediaSize(pageSizes[printerSettings.paper_format]);

            PrintCapabilities printCapabilites = printQueue.GetPrintCapabilities();

            // Modify PrintTicket
            if (printCapabilites.CollationCapability.Contains(Collation.Collated))
            {
                printTicket.Collation = Collation.Collated;
            }
            if (printerSettings.duplex == true)
            {
                if (printCapabilites.DuplexingCapability.Contains(
                    Duplexing.TwoSidedShortEdge))
                {
                    printTicket.Duplexing = Duplexing.TwoSidedShortEdge;
                }
            }
            else
            {
                if (printCapabilites.DuplexingCapability.Contains(
                    Duplexing.OneSided))
                {
                    printTicket.Duplexing = Duplexing.OneSided;
                }
            }
            if (printCapabilites.StaplingCapability.Contains(Stapling.StapleDualLeft))
            {
                printTicket.Stapling = Stapling.StapleDualLeft;
            }
            PageMediaSize pageSize = printCapabilites.PageMediaSizeCapability.Where(p => p.PageMediaSizeName == pageSizes[printerSettings.paper_format]).FirstOrDefault();
            if (pageSize != null)
                printTicket.PageMediaSize = pageSize;
            printTicket.PageOrientation = PageOrientation.Portrait;
            return printTicket;
        }
    }
}
