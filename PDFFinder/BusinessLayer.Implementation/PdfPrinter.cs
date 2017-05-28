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
        public void Print(string fileName, Report_Template printerSettings)
        {
            PdfDocument doc = new PdfDocument();
            doc.LoadFromFile(fileName);

            CustomPrintDialog printDialog = new CustomPrintDialog(printerSettings);
            printDialog.MinPage = 1;
            printDialog.MaxPage = doc.Pages.Count;
            if(printDialog.ShowDialog() == true)
            {
                doc.PrintFromPage = printDialog.MinPage < 1 ? 1 : printDialog.MinPage;
                doc.PrintToPage = printDialog.MaxPage > doc.Pages.Count ? doc.Pages.Count : printDialog.MaxPage;
                //Set the name of the printer which is to print the PDF
                doc.PrinterName = printDialog.DefaultPrinter;
                doc.PageSettings.Orientation = printDialog.CurrentPrinterSettings.DefaultPageSettings.Landscape == true ? PdfPageOrientation.Landscape : PdfPageOrientation.Portrait;
                doc.PageSettings.Size = new System.Drawing.SizeF(printDialog.CurrentPrinterSettings.DefaultPageSettings.PaperSize.Width, printDialog.CurrentPrinterSettings.DefaultPageSettings.PaperSize.Height);
                PrintDocument printDoc = doc.PrintDocument;
                printDoc.PrinterSettings.Duplex = printDialog.Duplex == true ? Duplex.Vertical : Duplex.Simplex;
                printDoc.Print();
            }
        }   
    }
}
