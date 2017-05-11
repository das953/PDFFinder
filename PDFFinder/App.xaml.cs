using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using PDFFinder.BusinessLayer.Implementation;

namespace PDFFinder
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        void App_Startup(object sender, StartupEventArgs e)
        {
            if(e.Args.Length==1)
            {
                PdfViewer pdfViewer = new PdfViewer();
                pdfViewer.View(null, e.Args[1]);
            }
            else
            {
                MessageBox.Show("Without parameters");
            }
        }
    }
}
