using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;


namespace PDFFinder.BusinessLayer.Implementation
{
    /// <summary>
    /// Create and show "using System.Xml.Linq" html file 
    /// </summary>
    public class PdfStatisticsView
    {

        /// <summary>
        /// write html document into file and open it in browser
        /// </summary>
        public void ShowStatistics()
        {

            var html = CreateHtml();
            var filePath = $@"{Directory.GetCurrentDirectory()}\~TempStatistics.html";
            if (File.Exists(filePath))
                File.Delete(filePath);
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(html);
                sw.Close();
                File.SetAttributes(filePath, FileAttributes.Hidden);
            }



            System.Diagnostics.Process.Start(filePath);

        }

        /// <summary>
        /// create html document with styles and DB data
        /// </summary>
        /// <returns>
        /// html document
        /// </returns>
        XElement CreateHtml()
        {

            var src = new XElement("script",

 new XAttribute("src", "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"),
 new XAttribute("integrity", "Tc5IQib027qvyjSMfHjOMaLkfuWVxZxUPnCJA7l2mCWNIpG9mGCD8wGNIcPD7Txa"),
 new XAttribute("crossorigin", "anonymous"));
            src.Value = string.Empty;

            //html document
            var html = new XElement("html",

 //add head to html
 new XElement("head",

  new XElement("title", "MoonPDF Statistic"),


 new XElement("link",
 new XAttribute("rel", "stylesheet"),
 new XAttribute("href", "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css"),
 new XAttribute("integrity", "sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u"),
 new XAttribute("crossorigin", "anonymous")),

 //add script
 src,

 //add hover style for table row
 new XElement("style",
 "td:hover {background: white;}")
 ));

            //add body to html
            html.Elements().First().Add(GetTableFromDB());

            return html;
        }

        /// <summary>
        /// write data from DB in table
        /// </summary>
        /// <returns>
        /// html table
        /// </returns>
        XElement GetTableFromDB()
        {

            var DBcontext = new Model.Model_PDFFinder();
            var stat = DBcontext.Statisticas.ToList();


            //table with header
            var table =


 new XElement("table",
  new XAttribute("cellpadding", "10"),
  new XAttribute("class", "table table-bordered"),
new XElement("thead",

    new XElement("tr",
      new XAttribute("class", "primary"),
           new XAttribute("style", "color: white"),
 new XElement("th", "Group Name",
 new XAttribute("style", "text-align: center")),
  new XElement("th", "Count",
 new XAttribute("style", "text-align: center"))))

 );


            //body without data
            var body =

            new XElement("body",
            new XAttribute("style", "background: linear-gradient(#000066, #6666ff)"),

            new XElement("div",
            new XAttribute("class", "row"),

           new XElement("div",
           new XAttribute("class", "col-md-8 col-md-offset-2"),

           table

  ))
    );

            //add data to body
            if (stat.Count > 0)
            {
                table.Add(new XElement("tr",
                     new XAttribute("style", "background: #7b97ea"),
                    new XElement("td", $"{stat[0].group_name.Substring(1)}"),
                    new XElement("td", $"{stat[0].processed_files_count}")));

                for (int i = 1; i < stat.Count; i++)
                {

                    if (i % 2 != 0)
                    {
                        table.Add(new XElement("tr",
                     new XAttribute("style", "background: #ffff99"),
                    new XElement("td", $"{stat[i].group_name}"),
                    new XElement("td", $"{stat[i].processed_files_count}")));
                    }
                    else
                    {
                        table.Add(new XElement("tr",
                     new XAttribute("style", "background: #7b97ea"),
                 new XElement("td", $"{stat[i].group_name}"),
                    new XElement("td", $"{stat[i].processed_files_count}")));
                    }

                }
            }

            return body;

        }
    }
}
