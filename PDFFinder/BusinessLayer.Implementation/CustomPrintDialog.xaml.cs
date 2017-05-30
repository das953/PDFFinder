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
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace PDFFinder.BusinessLayer.Implementation
{
    /// <summary>
    /// Interaction logic for CustomPrintDialog.xaml
    /// </summary>
    public partial class CustomPrintDialog : Window, INotifyPropertyChanged
    {
        private Report_Template _customPrinterSettings;
        private PageSettings _pageSettingsDialog;
        private Dictionary<string, PrinterSettings> _printerSettings;
        private PrinterSettings _defaultPrinterSettings;

        #region Print Dialog Properties

        //List of available printers
        private ObservableCollection<string> _availablePrinters;
        public ObservableCollection<string> AvailablePrinters
        {
            get
            {
                if(_availablePrinters == null)
                {
                    _availablePrinters = new ObservableCollection<string>();
                    foreach (var printer in PrinterSettings.InstalledPrinters)
                    {
                        _availablePrinters.Add(printer.ToString());
                    }
                }
                return _availablePrinters;
            }
            set { _availablePrinters = value; }
        }

        //Currently selected printer
        private string _defaultPrinter;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        public string DefaultPrinter
        {
            get
            {
                if(!AvailablePrinters.Contains(_defaultPrinter))
                    _defaultPrinter = _defaultPrinterSettings.PrinterName;
                return _defaultPrinter;
            }
            set
            {
                _defaultPrinter = value;
                OnPropertyChanged("DefaultPrinter");
            }
        }

        //Duplex capability
        private bool _canDuplex;
        public bool CanDuplex
        {
            get
            {
                return _canDuplex;
            }
            set
            {
                _canDuplex = value;
                OnPropertyChanged("CanDuplex");
            }
        }

        public bool? Duplex
        {
            get
            {
                if (_printerSettings[DefaultPrinter].Duplex == System.Drawing.Printing.Duplex.Horizontal || _printerSettings[DefaultPrinter].Duplex == System.Drawing.Printing.Duplex.Vertical)
                    return true && CanDuplex;
                else
                    return false;
            }
            set
            {
                _printerSettings[DefaultPrinter].Duplex = _customPrinterSettings.duplex == true ? System.Drawing.Printing.Duplex.Vertical : System.Drawing.Printing.Duplex.Simplex;
                OnPropertyChanged("Duplex");
            }
        }

        //Max and min page
        private int _minPage;
        public int MinPage
        {
            get { return _minPage; }
            set
            {
                _minPage = value;
                OnPropertyChanged("MinPage");
            }
        }

        private int _maxPage;
        public int MaxPage
        {
            get { return _maxPage; }
            set
            {
                _maxPage = value;
                OnPropertyChanged("MaxPage");
            }
        }

        //Current print settings
        public PrinterSettings CurrentPrinterSettings { get; set; }

        #endregion

        public CustomPrintDialog(Report_Template customPrinterSettings)
        {
            _customPrinterSettings = customPrinterSettings;
            _defaultPrinterSettings = new PrinterSettings();

            _printerSettings = new Dictionary<string, PrinterSettings>();
            InitializePrinterSettings();
            if(_printerSettings.ContainsKey(DefaultPrinter))
            {
                PaperSize paperSize = (from item in _printerSettings[DefaultPrinter].PaperSizes.Cast<PaperSize>() where item.PaperName == customPrinterSettings.paper_format select item).FirstOrDefault();
                if(paperSize==null)
                {
                    paperSize = (from item in _printerSettings[DefaultPrinter].PaperSizes.Cast<PaperSize>() where item.PaperName == "A4" select item).FirstOrDefault();
                }
                _printerSettings[DefaultPrinter].DefaultPageSettings.PaperSize = paperSize;
                CurrentPrinterSettings = _printerSettings[DefaultPrinter];
                Duplex = _customPrinterSettings.duplex;
                MinPage = 1;
                MaxPage = 1; 
            }
            //Selecting default printer
            DefaultPrinter = customPrinterSettings.printer_name;
            
            InitializeComponent();
            rbtnAllPages.IsChecked = true;
        }

        private void InitializePrinterSettings()
        {
            foreach (string printerName in AvailablePrinters)
            {
                PrinterSettings settings = new PrinterSettings();
                settings.PrinterName = printerName;
                PaperSize paperSize = (from item in settings.PaperSizes.Cast<PaperSize>() where item.PaperName == "A4" select item).FirstOrDefault();
                if(paperSize!=null)
                    settings.DefaultPageSettings.PaperSize = paperSize;
                settings.DefaultPageSettings.Landscape = false;
                settings.PrinterName = printerName;
                _printerSettings.Add(printerName, settings);
            }
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            _pageSettingsDialog = new PageSettings(CurrentPrinterSettings, _customPrinterSettings);

            //Providing page properties for _printerSettings
            if (_pageSettingsDialog.ShowDialog()==true)
            {
                PaperSize paperSize = (from item in _printerSettings[DefaultPrinter].PaperSizes.Cast<PaperSize>() where item.PaperName == _pageSettingsDialog.DefaultPaperSize select item).SingleOrDefault();
                if (paperSize != null)
                    _printerSettings[DefaultPrinter].DefaultPageSettings.PaperSize = paperSize;
                _printerSettings[DefaultPrinter].DefaultPageSettings.Landscape = (bool)_pageSettingsDialog.IsLandscape;
            }
        }

        private void printersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DefaultPrinter = (sender as ComboBox).SelectedItem.ToString();
            CanDuplex = _printerSettings[DefaultPrinter].CanDuplex;
            Duplex = _customPrinterSettings.duplex;
        }
        
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void txt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1) || (sender as TextBox).Text.Length>=4)
                e.Handled = true;
        }

        private void rbtnSomePages_Checked(object sender, RoutedEventArgs e)
        {
            txtMinPage.IsEnabled = true;
            txtMaxPage.IsEnabled = true;
        }

        private void rbtnAllPages_Checked(object sender, RoutedEventArgs e)
        {
            txtMinPage.IsEnabled = false;
            txtMaxPage.IsEnabled = false;
        }
    }
}
