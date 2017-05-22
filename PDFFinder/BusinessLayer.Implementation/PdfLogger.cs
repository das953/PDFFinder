using PDFFinder.BusinessLayer.Contracts;
using System;
using System.Linq;
using System.Diagnostics;

namespace PDFFinder.BusinessLayer.Implementation
{
    using Model;

    /// <summary>
    /// Записивыает в базу данных информацию и количество открытия файла для просмотра
    /// или для печати (необходимо согласовать с разработчиком базы данных, какие 
    /// именно данные будут записиваться)
    /// </summary>
    public class PdfLogger : IPdfLogger
    {
        public void LogOpenForPrinting(string title)
        {

            var dbcontext = new Model_PDFFinder();
            dbcontext.Statisticas.Where(x => x.group_name == title.Substring(0,5)).First().processed_files_count++;

            dbcontext.SaveChanges();

        }
        public void LogOpenForView()
        {
            var dbcontext = new Model_PDFFinder();

            dbcontext.Statisticas.Where(x => x.group_name == "NoGroup").First().processed_files_count++;

            dbcontext.SaveChanges();

            App.Current.Shutdown();
        }

    }
}
