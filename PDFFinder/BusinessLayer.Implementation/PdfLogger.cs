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
            var temp = dbcontext.Statisticas.Where(x => x.group_name == title.Substring(0, 5));
            if(temp.Count() == 0)
            {
                dbcontext.Statisticas.Add(new Statistica {
                    group_name = title.Substring(0, 5),
                    processed_files_count =1});
            }
            else
            {
                temp.First().processed_files_count++;
            }  
            dbcontext.SaveChanges();

        }
        public void LogOpenForView()
        {
            var dbcontext = new Model_PDFFinder();

            dbcontext.Statisticas.Where(x => x.group_name == "_NoGroup").First().processed_files_count++;

            dbcontext.SaveChanges();

            App.Current.Shutdown();
        }

    }
}
