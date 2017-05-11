using PDFFinder.BusinessLayer.Implementation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

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
                MessageBox.Show("Parameters");
            }
            else
            {
                MessageBox.Show("Without parameters");
                PdfParser parser = new PdfParser();
                MessageBox.Show(parser.Parse("text.pdf"));
            }
        }
    }
}
