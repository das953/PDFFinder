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
using System.Collections.ObjectModel;
using PDFFinder.Model;

namespace PDFFinder.BusinessLayer.Implementation
{
    /// <summary>
    /// Interaction logic for PageSettings.xaml
    /// </summary>
    public partial class PageSettings : Window
    {
        readonly PrinterSettings _printerSettings;
        readonly Report_Template _customPrinterSettings;

        //List of available page sizes
        private ObservableCollection<string> _availablePaperSizes;
        public ObservableCollection<string> AvailablePaperSizes
        {
            get
            {
                if (_availablePaperSizes == null)
                {
                    _availablePaperSizes = new ObservableCollection<string>();
                    foreach (PaperSize paperSize in _printerSettings.PaperSizes)
                    {
                        _availablePaperSizes.Add(paperSize.PaperName);
                    }
                }
                return _availablePaperSizes;
            }
            set { _availablePaperSizes = value; }
        }

        public string DefaultPaperSize { get; set; }

        public bool? IsLandscape { get; set; }

        public PageSettings(PrinterSettings printerSettings, Report_Template customPrinterSettings)
        {
            _printerSettings = printerSettings;
            _customPrinterSettings = customPrinterSettings;
            DefaultPaperSize = printerSettings.DefaultPageSettings.PaperSize.PaperName;
            IsLandscape = printerSettings.DefaultPageSettings.Landscape;
            InitializeComponent();
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
