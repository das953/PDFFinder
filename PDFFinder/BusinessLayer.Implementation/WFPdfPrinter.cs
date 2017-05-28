using Spire.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Printing;
using PDFFinder.Model;
using PDFFinder.BusinessLayer.Contracts;

namespace PDFFinder.BusinessLayer.Implementation
{
    public class WFPdfPrinter : IPdfPrinter
    {
        public void Print(string fileName, Report_Template printerSettings)
        {
            PdfDocument document = new PdfDocument();
            document.LoadFromFile(fileName);

            //--------------------------------------------Print dialog initial settings--------------------------------------//
            PrintDialog dialogPrint = new PrintDialog();
            dialogPrint.AllowPrintToFile = true;
            dialogPrint.AllowSomePages = true;

            //Printer name
            if(PrinterSettings.InstalledPrinters.Cast<string>().Contains(printerSettings.printer_name))
                dialogPrint.PrinterSettings.PrinterName = printerSettings.printer_name;

            //From - To print settings
            dialogPrint.PrinterSettings.MinimumPage = 1;
            dialogPrint.PrinterSettings.MaximumPage = document.Pages.Count;
            dialogPrint.PrinterSettings.FromPage = 1;
            dialogPrint.PrinterSettings.ToPage = document.Pages.Count;

            PrintDocument printDoc = document.PrintDocument;

            //Paper format setting
            PaperSize paper = (from item in dialogPrint.PrinterSettings.PaperSizes.Cast<PaperSize>() where item.PaperName == printerSettings.paper_format select item).SingleOrDefault();

            if (paper != null)
            {
                printDoc.DefaultPageSettings.PaperSize = paper;
                dialogPrint.PrinterSettings.DefaultPageSettings.PaperSize = paper;
            }

            //Duplexing
            if (dialogPrint.PrinterSettings.CanDuplex && printerSettings.duplex == true)
            {
                dialogPrint.PrinterSettings.Duplex = Duplex.Vertical;
                printDoc.PrinterSettings.Duplex = Duplex.Vertical;
            }
            if (dialogPrint.ShowDialog() == DialogResult.OK)
            {
                printDoc.PrinterSettings.FromPage = dialogPrint.PrinterSettings.FromPage;
                printDoc.PrinterSettings.ToPage = dialogPrint.PrinterSettings.ToPage;
                printDoc.PrinterSettings.PrinterName = dialogPrint.PrinterSettings.PrinterName;
                printDoc.PrinterSettings.DefaultPageSettings.PaperSize = dialogPrint.PrinterSettings.DefaultPageSettings.PaperSize;
                printDoc.PrinterSettings.DefaultPageSettings.Landscape = dialogPrint.PrinterSettings.DefaultPageSettings.Landscape;
                printDoc.DefaultPageSettings.PaperSize = dialogPrint.PrinterSettings.DefaultPageSettings.PaperSize;
                printDoc.DefaultPageSettings.Landscape = dialogPrint.PrinterSettings.DefaultPageSettings.Landscape;
                dialogPrint.Document = printDoc;
                printDoc.Print();
            }
        }
    }
}
