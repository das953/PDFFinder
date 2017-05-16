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

namespace PDFFinder.BusinessLayer.Implementation
{
    /// <summary>
    /// Служит для печати файла - Панибратюк Александр
    /// </summary>
    public class PdfPrinter : IPdfPrinter
    {
        public void Print(string fileName, Report_Template printerSettings)
        {
            PdfDocument doc = new PdfDocument();
            doc.LoadFromFile(fileName);
            PrintDialog dialogPrint = new PrintDialog();
            dialogPrint.PrintQueue = new PrintQueue(new PrintServer(), "Отправить в OneNote 16");
            dialogPrint.PrintTicket = GetPrintTicketFromPrinter(printerSettings);

            if (dialogPrint.ShowDialog() == true)
            {
                
                //dialogPrint.PrintDocument(fixedDocument.DocumentPaginator, "Digital Scale");


                /*dialogPrint.PrintTicket.PageMediaSize = new PageMediaSize(PageMediaSizeName.);*/
                PrintDocument printDoc = doc.PrintDocument;
                printDoc.Print();
            }
        }
        private PrintTicket GetPrintTicketFromPrinter(Report_Template printerSettings)
        {
            PrintQueue printQueue = null;

            LocalPrintServer localPrintServer = new LocalPrintServer();

            // Retrieving collection of local printer on user machine
            PrintQueueCollection localPrinterCollection =
                localPrintServer.GetPrintQueues();

            System.Collections.IEnumerator localPrinterEnumerator =
                localPrinterCollection.GetEnumerator();

            if (localPrinterEnumerator.MoveNext())
            {
                // Get PrintQueue from first available printer
                printQueue = (PrintQueue)localPrinterEnumerator.Current;
                MessageBox.Show(printQueue.Name);
            }
            else
            {
                // No printer exist, return null PrintTicket
                return null;
            }

            // Get default PrintTicket from printer
            PrintTicket printTicket = printQueue.DefaultPrintTicket;

            PrintCapabilities printCapabilites = printQueue.GetPrintCapabilities();

            // Modify PrintTicket
            if (printCapabilites.CollationCapability.Contains(Collation.Collated))
            {
                printTicket.Collation = Collation.Collated;
            }

            if (printCapabilites.DuplexingCapability.Contains(
                    Duplexing.TwoSidedLongEdge))
            {
                printTicket.Duplexing = Duplexing.TwoSidedLongEdge;
            }

            if (printCapabilites.StaplingCapability.Contains(Stapling.StapleDualLeft))
            {
                printTicket.Stapling = Stapling.StapleDualLeft;
            }
            printTicket.PageMediaSize = new PageMediaSize(PageMediaSizeName.JISB4);
            printTicket.PageOrientation = PageOrientation.Unknown;
            printTicket.Duplexing = Duplexing.TwoSidedShortEdge;
            printTicket.PageResolution = new PageResolution(PageQualitativeResolution.Draft);
            return printTicket;
        }// end:GetPrintTicketFromPrinter()
    }
}
