using PDFFinder.BusinessLayer.Implementation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
        public MainWindow()
        {
            ApplicationList = new ObservableCollection<AppDescription>(GetAssociatedApplications("pdf"));
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            ApplicationList.RemoveAt(0);
        }

        private IEnumerable<AppDescription> GetAssociatedApplications(string ext)
        {
            List<AppDescription> applications = new List<AppDescription>();
            FileAssociationManager manager = new FileAssociationManager();
            IEnumerable<string> progIdList = manager.ListOfProgids(ext);
            foreach (var progId in progIdList)
            {
                string appName = manager.GetApplicationName(progId);
                string appPath = manager.GetApplicationPath(progId);
                Icon appIcon = manager.ExtractIconFromFile(appPath);
                ImageSource imageSource;
                using (Bitmap bmp = appIcon.ToBitmap())
                {
                    var stream = new MemoryStream();
                    bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    imageSource = BitmapFrame.Create(stream);
                }
                applications.Add(new AppDescription { Name = appName, Path = appPath, Icon = imageSource });
            }
            return applications;
        }

        private void listViewApps_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = listViewApps.SelectedIndex;
            AppDescription app = ApplicationList[selectedIndex];
            string programPath = app.Path;
            Process proc = new Process();
            proc.StartInfo.FileName = programPath;
            proc.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(programPath);
            proc.Start();
        }
    }
}
