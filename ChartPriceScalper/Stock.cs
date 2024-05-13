using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChartPriceScalper
{
    internal class Stock
    {
        private string url(string x) { return (@"https://finance.yahoo.com/quote/" + x); }


        public float Price_Check_MarketWatch(string Ticker_Symbol)
        {
            float Price = 0;
            string url_ = @"https://www.marketwatch.com/investing/stock/" + Ticker_Symbol;

            bool Ticker_Found = false;
            HtmlNode supreme_node;
            var web = new HtmlWeb();
            var doc = new HtmlAgilityPack.HtmlDocument();

            doc = web.Load(url_);
            HtmlNodeCollection names = doc.DocumentNode.SelectNodes("//h2//bg-quote");
            if (names != null)
            {
                float.TryParse(names[0].InnerText, out Price);
                foreach (HtmlNode node in names)
                {
                    float.TryParse(node.InnerText, out Price);

                    foreach (HtmlAttribute a in node.GetAttributes())
                    {
                        if (a.Value.ToString() == Ticker_Symbol)
                        {
                            supreme_node = node;
                            float.TryParse(supreme_node.InnerText, out Price);
                            Ticker_Found = true;
                            break;
                        }
                    }
                    if (Ticker_Found) { break; }
                }
            }
            return Price;
        }

        public float Price_Check(string Ticker_Symbol)
        {
            string url_ = url(Ticker_Symbol);
            float Price = 0;
            bool Ticker_Found = false;
            HtmlNode supreme_node;
            var web = new HtmlWeb();
            var doc = new HtmlAgilityPack.HtmlDocument();

            int error_tries = 0;

            try
            {
                doc = web.Load(url_);
                HtmlNodeCollection names = doc.DocumentNode.SelectNodes("//fin-streamer");
                if (names != null)
                {
                    foreach (HtmlNode node in names)
                    {
                        foreach (HtmlAttribute a in node.GetAttributes())
                        {
                            if (a.Value.ToString() == "livePrice svelte-mgkamr") // The class name where the active price is located
                            {
                                supreme_node = node;
                                float.TryParse(supreme_node.InnerText, out Price);
                                Ticker_Found = true;
                                break;
                            }
                        }
                        if (Ticker_Found) { break; }
                    }
                }
                return Price;
            }
            catch
            {
                if (error_tries < 3)
                {
                    error_tries++;
                    return Price_Check(Ticker_Symbol);
                }
                else { return 0; }
            }
        }


        public string[] Price_Check_Advanced(string Ticker_Symbol)
        {
            string url_ = url(Ticker_Symbol);
            bool Ticker_Found = false;
            HtmlNode supreme_node;
            var web = new HtmlWeb();
            var doc = new HtmlAgilityPack.HtmlDocument();

            string[] result_range = new string[6];
            int index = 0;

            doc = web.Load(url_);
            HtmlNodeCollection names = doc.DocumentNode.SelectNodes("//fin-streamer");
            if (names != null)
            {
                foreach (HtmlNode node in names)
                {
                    foreach (HtmlAttribute a in node.GetAttributes())
                    {
                        if (a.Value.ToString() == Ticker_Symbol)
                        {
                            supreme_node = node;

                            if (supreme_node.InnerText != string.Empty)
                            {
                                if (index == 6) { break; }
                                result_range[index] = supreme_node.InnerText;
                                index++;
                            }
                        }
                    }
                    if (Ticker_Found) { break; }
                }
            }
            return result_range;
        }
    }
}
