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
        public void LogOpenForPrinting(string GroupName)
        {
            DbDataLoad(GroupName);
        }
        public void LogOpenForView(string GroupName)
        {
            DbDataLoad(GroupName);
        }
        void DbDataLoad(string GroupName)
        {
            var stat = new Model_PDFFinder();
            var dbcontext = (from cust in stat.Statisticas select cust);
            
            if(GroupName!=null)
            {

                foreach (var item in dbcontext)
                {
                    if (GroupName == item.group_name)
                    {
                        stat.Statisticas.Where(x => x.group_name == item.group_name).First().processed_files_count++;
                        break;
                        // и дальше запись в бд
                    }
                  
                }
              
            }
            else
            {
                stat.Statisticas.Where(x => x.group_name == "NoGroup").First().processed_files_count++;
        
            }
            stat.SaveChanges();
        }
        void PritnDataDb()
        {
            var stat = new Model_PDFFinder();
            var dbcontext = (from cust in stat.Statisticas select cust).ToList();

            foreach (var item in dbcontext)
            {
                Debug.WriteLine($"name {item.group_name}\ncount {item.processed_files_count}\nNoGroup {item}");
            }
        }
    }
}
