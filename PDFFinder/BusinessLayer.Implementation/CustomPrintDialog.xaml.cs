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
using PDFFinder.Model;

namespace PDFFinder.BusinessLayer.Implementation
{
    /// <summary>
    /// Interaction logic for CustomPrintDialog.xaml
    /// </summary>
    public partial class CustomPrintDialog : Window
    {
        Report_Template _printerSettings = null;
        public CustomPrintDialog(Report_Template printerSettings)
        {
            InitializeComponent();
            _printerSettings = printerSettings;
            foreach (string printer in PrinterSettings.InstalledPrinters)
                printersList.Items.Add(printer.ToString());
            PrinterSettings settings = new PrinterSettings();
            if (printerSettings==null)
                printersList.SelectedItem = settings.PrinterName;
            else
            {
                foreach (string printer in PrinterSettings.InstalledPrinters)
                {
                    if(printer==printerSettings.printer_name)
                    {
                        printersList.SelectedItem = printerSettings.printer_name;
                        break;
                    }
                }
            }
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            string printerName = printersList.SelectedItem.ToString();
            PageSettings settings = new PageSettings(printerName, _printerSettings.paper_format);
            settings.ShowDialog();
        }
    }
}
