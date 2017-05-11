using PDFFinder.BusinessLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFFinder.BusinessLayer.Implementation
{
    /// <summary>
    /// Записивыает в базу данных информацию и время открытия файла для просмотра или для печати (необходимо согласовать с разработчиком базы данных, какие именно данные будут записиваться)
    /// </summary>
    public class PdfLogger : IPdfLogger
    {
        public void LogOpenForPrinting()
        {
            throw new NotImplementedException();
        }

        public void LogOpenForView()
        {
            throw new NotImplementedException();
        }
    }
}
