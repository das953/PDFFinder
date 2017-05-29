using PDFFinder.BusinessLayer.Contracts;
using PDFFinder.BusinessLayer.Implementation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace PDFFinder
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static List<CultureInfo> m_Languages = new List<CultureInfo>();

        public static List<CultureInfo> Languages
        {
            get
            {
                return m_Languages;
            }
        }

        public App()
        {
            m_Languages.Clear();
            m_Languages.Add(new CultureInfo("en-US")); //Нейтральная культура для этого проекта
            m_Languages.Add(new CultureInfo("ru-RU"));
            m_Languages.Add(new CultureInfo("uk-UA"));
            App.LanguageChanged += App_LanguageChanged;
        }


        private void Application_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            Language = PDFFinder.Properties.Settings.Default.DefaultLanguage;
        }

        private void App_LanguageChanged(Object sender, EventArgs e)
        {
            PDFFinder.Properties.Settings.Default.DefaultLanguage = Language;
            PDFFinder.Properties.Settings.Default.Save();
        }
        public static event EventHandler LanguageChanged;
        public static CultureInfo Language
        {
            get
            {
                return System.Threading.Thread.CurrentThread.CurrentUICulture;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                if (value == System.Threading.Thread.CurrentThread.CurrentUICulture) return;

                //1. Меняем язык приложения:
                System.Threading.Thread.CurrentThread.CurrentUICulture = value;

                //2. Создаём ResourceDictionary для новой культуры
                ResourceDictionary dict = new ResourceDictionary();
                switch (value.Name)
                {
                    case "ru-RU":
                        dict.Source = new Uri(String.Format("Resources/lang.{0}.xaml", value.Name), UriKind.Relative);
                        break;
                    case "uk-UA":
                        dict.Source = new Uri(String.Format("Resources/lang.{0}.xaml", value.Name), UriKind.Relative);
                        break;
                    default:
                        dict.Source = new Uri("Resources/lang.xaml", UriKind.Relative);
                        break;
                }

                //3. Находим старую ResourceDictionary и удаляем его и добавляем новую ResourceDictionary
                ResourceDictionary oldDict = (from d in Current.Resources.MergedDictionaries
                                              where d.Source != null && d.Source.OriginalString.StartsWith("Resources/lang.")
                                              select d).First();
                if (oldDict != null)
                {
                    int ind = Current.Resources.MergedDictionaries.IndexOf(oldDict);
                    Current.Resources.MergedDictionaries.Remove(oldDict);
                    Current.Resources.MergedDictionaries.Insert(ind, dict);
                }
                else
                {
                    Application.Current.Resources.MergedDictionaries.Add(dict);
                }

                //4. Вызываем ивент для оповещения всех окон.
                LanguageChanged(Application.Current, new EventArgs());
            }
        }

        
        void App_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length >= 1)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in e.Args)
                {
                    sb.Append(item);
                }
                string filename = sb.ToString();
                FileInfo fileInfo = new FileInfo(filename);
                if (fileInfo.Exists && fileInfo.Extension == ".pdf")
                {
                    IPdfManager pdfManager = new PdfManager();
                    pdfManager.Execute(filename);
                }
                else
                {
                    MessageBox.Show("File not exists or has invalid extension", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                Application.Current.Shutdown();
            }
            else
            {
                //PdfManager manager = new PdfManager();
                //manager.Execute(@"C:\Users\das953\Desktop\Test.pdf");
                //для тесту

                MainWindow config = new MainWindow();
                config.Show();
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            File.Delete($@"{Directory.GetCurrentDirectory()}\~TempStatistics.html");
            base.OnExit(e);
        }
    }
}
