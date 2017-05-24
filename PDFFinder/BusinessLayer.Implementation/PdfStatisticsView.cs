using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;


namespace PDFFinder.BusinessLayer.Implementation
{
    class PdfStatisticsView
    {



        public static void ShowStatistics()
        {
            var src = new XElement("script",
 new XAttribute("src", "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"),
 new XAttribute("integrity", "Tc5IQib027qvyjSMfHjOMaLkfuWVxZxUPnCJA7l2mCWNIpG9mGCD8wGNIcPD7Txa"),
 new XAttribute("crossorigin", "anonymous"));
            src.Value = string.Empty;
            var html = new XElement("html",
 new XElement("head", 

 new XElement("link", 
 new XAttribute("rel", "stylesheet"), 
 new XAttribute("href", "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css"), 
 new XAttribute("integrity", "sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u"), 
 new XAttribute("crossorigin", "anonymous")),
 src

 
 )



 
);

            html.Elements().First().Add(
                new XElement("body",
                new XAttribute("style", "background: linear-gradient(#000066, #6666ff)"),

                new XElement("div",
                new XAttribute("class", "row"),

               new XElement("div", 
               new XAttribute("class", "col-md-8 col-md-offset-2"),
               
          

     new XElement("table",
      new XAttribute("cellpadding", "10"),
      new XAttribute("class", "table table-bordered"),
 

        new XElement("tr",
          new XAttribute("class", "primary"),
               new XAttribute("style", "color: white"),
     new XElement("th", "Group Name",
     new XAttribute("style", "text-align: center")),
      new XElement("th", "Count",
     new XAttribute("style", "text-align: center"))),


     new XElement("tr",
      new XAttribute("style", "background: #7b97ea"),
     new XElement("td", "TestG"),
     new XElement("td", "953")),

     new XElement("tr",
      new XAttribute("style", "background: #ffff99"),
     new XElement("td", "Ptaha"),
     new XElement("td", "0"))

     )
          ))
 )
                );
            
            using (FileStream fs = new FileStream($@"{Directory.GetCurrentDirectory()}\temp.html", FileMode.Create))
            {

                StreamWriter sw = new StreamWriter(fs);
                sw.Write(html);
                sw.Close();
            }



            System.Diagnostics.Process.Start($@"{Directory.GetCurrentDirectory()}\temp.html");

            #region DANGER!

            System.Threading.Thread.Sleep(1301);
            File.Delete($@"{Directory.GetCurrentDirectory()}\temp.html");

            #endregion
        }
    }
}
