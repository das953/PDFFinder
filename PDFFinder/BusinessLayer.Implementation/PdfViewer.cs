using PDFFinder.BusinessLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFFinder.BusinessLayer.Implementation
{
    /// <summary>
    /// Просмотр файла в стандартном обозревателе (решить проблему с возможностью выбора обозревателей) и открыть файл в выбранном
    /// </summary>
    public class PdfViewer : IPdfViewer
    {
        public void View(string fileName, string processName)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = processName;
            proc.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(processName);
            proc.StartInfo.Arguments = fileName;
            proc.Start();
        }
    }
}
