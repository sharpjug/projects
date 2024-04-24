using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stock_Scalper.analysis
{
    internal class stockinfo
    {
        private string url(string x) { return string.Format("https://stockanalysis.com/stocks/{0}/financials/", x); }


        public string[] Get_Info(string Ticker_Symbol, string ToGet)
        {
            string url_ = url(Ticker_Symbol);
            var web = new HtmlWeb();
            var doc = new HtmlAgilityPack.HtmlDocument();

            doc = web.Load(url_);

            HtmlNodeCollection tables = doc.DocumentNode.SelectNodes("//table");

            bool break_loop = false;
            bool log_next = false;
            int index = 0;
            string[] values = new string[8];

            if (tables != null)
            {
                foreach (HtmlNode row in tables.Descendants())
                {
                    if (break_loop) { break; }
                    if (!Stock_Analysis.instance.Got_Date && row.InnerText.Contains("Year")) { Stock_Analysis.instance.Year_String = row.InnerText; Stock_Analysis.instance.Got_Date = true; Stock_Analysis.instance.Sort_Date(); }
                    values = new string[Stock_Analysis.instance.data_amount];
                    if (row.Name == "tr")
                    {
                        foreach(HtmlNode cell in row.Descendants())
                        {
                            if (cell.Name == "td")
                            {
                                if (log_next)
                                {
                                    values[index] = cell.InnerText;
                                    index++;
                                    if (index == Stock_Analysis.instance.data_amount)
                                    {
                                        break_loop = true;
                                        break;
                                    }
                                }
                                if (cell.InnerText.Trim() == ToGet) { log_next = true; }
                            }
                        }
                    }
                }
            }
            return values;
        }
    }
}
