using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Drawing.Printing;

namespace PDFFinder.BusinessLayer.Implementation
{
    /// <summary>
    /// Interaction logic for PageSettings.xaml
    /// </summary>
    public partial class PageSettings : Window
    {
        string _printerName = null;
        public PageSettings(string printerName, string paperSize)
        {
            InitializeComponent();
            _printerName = printerName;
            PrinterSettings settings = new PrinterSettings();
            settings.PrinterName = printerName;
            string defaultPaperSize = null;
            foreach (PaperSize pageSize in settings.PaperSizes)
            {
                string paperName = pageSize.PaperName;
                pageSizeList.Items.Add(paperName);
                if(paperSize == pageSize.PaperName)
                {
                    defaultPaperSize = paperSize;
                }
            }
            pageSizeList.SelectedItem = defaultPaperSize == null ? settings.DefaultPageSettings.PaperSize.PaperName : defaultPaperSize;
        }

        private void pageSizeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PrinterSettings settings = new PrinterSettings();
            settings.PrinterName = _printerName;
        }
    }
}
