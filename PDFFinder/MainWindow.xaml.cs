using Microsoft.Win32;
using PDFFinder.BusinessLayer.Implementation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PDFFinder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<AppDescription> ApplicationList { get; set; }
        public AppDescription DefaultApplication { get; set; }
        public FileAssociationManager AssociationManager { get; set; }
        public MainWindow()
        {
            AssociationManager = new FileAssociationManager();
            ApplicationList = new ObservableCollection<AppDescription>(AssociationManager.GetAssociatedApplications(".pdf"));
            if (ApplicationList.Count!=0)
            {
                DefaultApplication = AssociationManager.GetAssociatedApplication(".pdf"); 
            }
            BusinessLayer.Implementation.PdfLogger log = new PdfLogger();
        
            InitializeComponent();
            
        }
        
        private void listViewApps_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = listViewApps.SelectedIndex;
            AppDescription app = ApplicationList[selectedIndex];
            AssociationManager.SaveAssociatedApplication(app.ProgId, ".pdf");
            DefaultApplication = AssociationManager.GetAssociatedApplication(".pdf");
            imgDefault.Source = DefaultApplication.Icon;
            txtDefault.Text = DefaultApplication.Name;

            /*Process proc = new Process();
            proc.StartInfo.FileName = programPath;
            proc.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(programPath);
            proc.Start();*/
        }
    }
}
