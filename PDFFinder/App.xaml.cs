﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;

namespace PDFFinder
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        void App_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length == 1)
            {
                //PROCESSname
                //e.Args[1] - filename
                string ProcessName = null;
                string FileName = null;
                Process.Start(ProcessName, FileName);
                
            }
           
        }
    }
}
