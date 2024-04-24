using Stock_Scalper.analysis;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Stock_Scalper
{
    public partial class Stock_Analysis : Form
    {
        public static Stock_Analysis instance;

        public Stock_Analysis()
        {
            InitializeComponent();
            instance = this;
        }
        public bool Got_Date = false;
        public string Year_String = string.Empty;
        public int data_amount = 8;

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void Search()
        {
            stockinfo info = new stockinfo();
            string srch = "KO";
            Year_String = string.Empty;
            data_amount = 8;
            Got_Date = false;

            if (txtSearch.Text != string.Empty) { srch = txtSearch.Text; }


            Chart_Filler(info.Get_Info(srch, "Revenue"), ChartRevenue, "Revenue", Color.Orange);

            Chart_Filler(info.Get_Info(srch, "Net Income"), ChartNetIncome, "Net Income", Color.RosyBrown);

            Chart_Filler(info.Get_Info(srch, "Free Cash Flow"), ChartFreeCashFlow, "Free Cash Flow", Color.SeaGreen);

            Chart_Filler(info.Get_Info(srch, "EBITDA"), ChartEBITDA, "EBITDA", Color.Indigo);

            Chart_Filler(info.Get_Info(srch, "EPS (Basic)"), ChartEPS, "EPS", Color.Peru);

            Chart_Filler(info.Get_Info(srch, "Shares Outstanding (Basic)"), ChartSharesOutstanding, "Shares Outstanding", Color.Violet);
        }

        private void Chart_Filler(string[] values, Chart chart, string Title, Color color)
        {
            chart.Titles.Clear();
            chart.Series.Clear();
            chart.ChartAreas[0].RecalculateAxesScale();

            float[] float_values = String_Array_To_Float_Array(values);

            chart.Series.Add(Title);
            for(int i = data_amount - 1; -1 < i; i--)
            {
                chart.Series[Title].Points.AddXY(dates[i], float_values[i]);
            }
            chart.Titles.Add(Title);

            chart.Series[Title].Color = color;
            chart.Series[Title].IsVisibleInLegend = false;
            chart.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
            chart.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
        }

        private float[] String_Array_To_Float_Array(string[] values)
        {
            float[] int_values = new float[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                string value = values[i].Replace('"', ',').Replace(",", "");
                if (value == "-") { int_values[i] = 0; continue; }
                int_values[i] = float.Parse(value);
            }
            return int_values;
        }


        string[] dates = new string[8];
        public void Sort_Date()
        {
            string temp = Year_String.Remove(0, 4).Trim();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < temp.Length; i++)
            {
                if (i % 4 == 0)
                    sb.Append(' ');
                sb.Append(temp[i]);
            }
            int data = sb.ToString().Remove(0, 1).Split(' ').Length;

            dates = sb.ToString().Remove(0,1).Split(' ');

            List<string> testing_data = new List<string>();

            for (int i = 0; i < data; i++)
            {
                float number;
                if (float.TryParse(dates[i], out number))
                {
                    testing_data.Add(number.ToString());
                }
                else { break; }
            }
            dates = testing_data.ToArray();
            data_amount = dates.Length;
        }
    }
}
