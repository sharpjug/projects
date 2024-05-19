using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ChartPriceScalper
{
    internal class Crypto
    {
        public double Price_Check_BTC(string Asset)
        {
            try
            {
                string url_ = @"https://api.binance.com/api/v3/ticker?symbol=" + Asset;
                HttpClient client = new HttpClient();
                var response = client.GetStringAsync(url_).Result;

                HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
                htmlDoc.LoadHtml(response);

                string Split = htmlDoc.Text.Split(new string[] { "lastPrice" }, StringSplitOptions.None)[1].Remove(0, 3).Split('"')[0];

                return double.Parse(Split);
            }
            catch { return 0; }
        }

        public float Price_Check_Crypto(string Asset) // High, Low
        {
            try
            {
                //TimeSpan t = DateTime.Now - new DateTime(1970, 1, 1);
                //int secondsSinceEpoch = (int)t.TotalSeconds;

                //string format_url = string.Format(@"https://api.bybit.com/public/linear/index-price-kline?symbol={0}&interval=1&limit=1&from={1}", Asset, secondsSinceEpoch - 60);
                string format_url = string.Format(@"https://api.bybit.com/v2/public/tickers?symbol={0}", Asset);

                string url_ = format_url;


                HttpClient client = new HttpClient();
                var response = client.GetStringAsync(url_).Result;

                HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
                htmlDoc.LoadHtml(response);

                string html_text = htmlDoc.Text.Replace("index_price", "|");
                string Split = html_text.Split('|')[1].Remove(0, 3).Split('"')[0];
                return float.Parse(Split);
            }
            catch { return 0; }
        }

        public (float, float) Price_Check_Crypto_History(string Asset, string Time)
        {
            string format_url = string.Format(@"https://api.binance.com/api/v3/klines?symbol={0}&interval=15m&limit=500&startTime={1}", Asset, Time);

            HttpClient client = new HttpClient();

            var response = client.GetStringAsync(format_url).Result;


            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(response);

            string html_text = htmlDoc.Text;


            string V = char.ToString('"');
            string[] prices = html_text.Substring(0, html_text.Length - 1).Split(']');

            float High = 0;
            float Low = 0;

            foreach (string x in prices)
            {
                if (x.Length > 1)
                {
                    string[] y = x.Substring(2).Replace(V, string.Empty).Split(',');
                    if (Low == 0) { Low = float.Parse(y[3]); };
                    if (float.Parse(y[2]) > High) { High = float.Parse(y[2]); }
                    if (float.Parse(y[3]) < Low) { Low = float.Parse(y[3]); }
                }
            }

            return (High, Low);
        }
    }
}
