//using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;
using ChartPriceScalper.functions;

namespace ChartPriceScalper
{
    public partial class Scalper : Form
    {
        public static Scalper instance;

        public Scalper()
        {
            InitializeComponent();
            this.dataGridView1.MouseWheel += new MouseEventHandler(this.mousewheel);
            instance = this;
            data.Load_Info();
            Get_Current_Time();
        }
        // Asset 0 | Price/Alert 1 | Price 2 | Crypto/Stock 3 | AboveBelow 4 | Alerted 5 | Index 6

        #region Variables

        string String_MainAsset = string.Empty;
        Thread thread;
        Thread Alert_Thread;
        Thread Stocks;
        bool ThreadRun = false;
        bool Alert_ = false;
        bool AlertGoing = false;
        IList<Action> action = new List<Action>();
        List<string> info = new List<string>();
        int Row_Selected = 0;
        (string, string) messagebox_text;
        bool Running = false;
        bool StocksRunning = false;
        int index_adv_search_type = 0;
        Turn turn = new Turn();
        bool ToRun = false;

        static bool Check_Stocks = true;


        DataTable dt = new DataTable();
        DataView dv = new DataView();
        Data data = new Data();
        Crypto crypto = new Crypto();
        Stock stock = new Stock();

        // List View Choice - Default is Clear
        // 0 - Clear / 1 - Alerted / 2 - How Near
        public byte view = 0;
        public byte Stock_Data = 0; // 0 - Yahoo / 1 - MarketWatch

        #endregion

        //private void Load_Popup()
        //{
        //    Notifier.TitleText = "Alert!";
        //    Notifier.ContentFont = new Font("Segoe UI", 22, FontStyle.Bold);
        //    Notifier.Image = SystemIcons.Exclamation.ToBitmap();
        //    Notifier.TitleColor = Color.White;
        //    Notifier.BodyColor = Color.Black;
        //    Notifier.HeaderColor = Color.Gray;
        //    Notifier.ContentColor = Color.White;
        //}


        #region Buttons

        private void btn_Click(object sender, EventArgs e) { Add(); }

        private bool Exists(string Asset, string AssetClass)
        {
            //string format_url_crypto = string.Format(@"https://api-testnet.bybit.com/v2/public/tickers?symbol={0}", Asset);
            string format_url_crypto = string.Format(@"https://api.binance.com/api/v3/klines?symbol={0}&interval=15m&limit=500&startTime={1}", Asset, Get_Current_Time()+180000);
            try
            {
                HttpClient client = new HttpClient();

                var response = client.GetStringAsync(format_url_crypto).Result;
                client.Dispose();
                return true;
            }
            catch
            {
                MessageBox.Show("Sorry, cannot find the asset you are looking for.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private async void Add()
        {
            StocksRunning = false;
            Running = false;
            await Task.Delay(1000);
            turn.Off();
            await Task.Delay(1000);
            string AboveBelow = string.Empty;
            float Price = 0;
            string Time = Get_Current_Time();

            // Check if asset is Above|Below

            if (txtPrice.Text.Contains('x'))
            {
                var x = txtPrice.Text.Split('x');
                int last_num_index = x[0].Length - 1;
                int num_zero = int.Parse(x[0][last_num_index].ToString());
                string new_num = x[0].Substring(0, last_num_index);
                if (new_num == string.Empty) { new_num = "0."; }
                for (int i = 0; i < num_zero; i++)
                {
                    new_num = new_num + "0";
                }
                new_num = new_num + x[1];
                txtPrice.Text = new_num;
            }

            if (String_MainAsset == "Crypto")
            {
                if (!Exists(txtAsset.Text, "Crypto")) { return; }
                if (crypto.Price_Check_Crypto(txtAsset.Text) > float.Parse(txtPrice.Text)) { AboveBelow = "Below"; }
                else { AboveBelow = "Above"; }
            }
            else if (String_MainAsset == "BTC")
            {
                if (crypto.Price_Check_BTC(txtAsset.Text) > float.Parse(txtPrice.Text)) { AboveBelow = "Below"; }
                else { AboveBelow = "Above"; }
            }
            else
            {
                Price = stock.Price_Check(txtAsset.Text);
                if (Price > float.Parse(txtPrice.Text)) { AboveBelow = "Below"; }
                else { AboveBelow = "Above"; }
            }

            while (true)
            {
                if (Running | StocksRunning | ToRun) { await Task.Delay(100); }
                else { break; }
            }

            // Asset | Price/Alert | Price | Crypto/Stock | AboveBelow | Alerted | Time, Index
            dv.Table.Rows.Add(txtAsset.Text, txtPrice.Text, 0, String_MainAsset, AboveBelow, false, Time, 0);
            await Task.Delay(100);
            Order_Rows();
            Form_Asset_List();
            await Task.Delay(1000);
            turn.On();
            turn.Reset();
            if (view == 1) { Colour_All_Rows_If_Alerted(); }
            if (String_MainAsset == "Stock") { dv.Table.Rows[dv.Table.Rows.Count - 1][2] = Price; }
            if (view == 2) { Colour_All_Near(); }
        }

        private void btnRemove_Click(object sender, EventArgs e) { Remove(); }

        private async void Remove()
        {
            StocksRunning = false;
            Running = false;
            bool Loop = true;
            await Task.Delay(1000);
            turn.Off();
            await Task.Delay(1000);
            if (thread == null && Stocks == null) { Loop = false; }
            while (Loop)
            {
                if (Running | StocksRunning | ToRun) { await Task.Delay(100); }
                else { break; }
            }
            int row = int.Parse(dataGridView1.Rows[Row_Selected].Cells[7].Value.ToString());
            Deleted_Updated(row);
            dv.Table.Rows.RemoveAt(row);
            Order_Rows();
            Form_Asset_List();
            await Task.Delay(1000);
            turn.On();
            turn.Reset();
        }

        private void Deleted_Updated(int row)
        {
            string l1 = lblRemoved1.Text;
            string l2 = lblRemoved2.Text;

            string asset = dv.Table.Rows[row][0].ToString();
            string Alert = dv.Table.Rows[row][1].ToString();
            string mainasset = dv.Table.Rows[row][3].ToString();
            string move = dv.Table.Rows[row][4].ToString();
            string alerted = dv.Table.Rows[row][5].ToString();
            string time = dv.Table.Rows[row][6].ToString();
            string mainstring = string.Format("{0} {1} {2} {3} {4} {5} {6}", asset, Alert, 0, mainasset, move, alerted, time);

            if (l1 == string.Empty && l2 == string.Empty)
            {
                lblRemoved1.Text = mainstring;
            }
            else if (l1 != string.Empty && l2 == string.Empty)
            {
                lblRemoved2.Text = lblRemoved1.Text;
                lblRemoved1.Text = mainstring;
            }
            else
            {
                string temp = lblRemoved1.Text;
                lblRemoved1.Text = mainstring;
                lblRemoved2.Text = temp;
            }
        }

        private void btnBTC_Click(object sender, EventArgs e)
        {
            String_MainAsset = "BTC";
            lblSC.Text = "BTC";
            //decimal num = Convert.ToDecimal(Price_Check_BTC(txtAsset.Text));
            //float num2 = float.Parse(num.ToString());
            //MessageBox.Show(num2.ToString());
        }
        //private void btnCrypto_Click(object sender, EventArgs e)
        //{
        //    if()
        //    String_MainAsset = "Crypto";
        //    lblSC.Text = "Crypto";
        //}

        private void btnCrypto_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                String_MainAsset = "Crypto";
                lblSC.Text = "Crypto";
            }
            else if(e.Button == MouseButtons.Right)
            {
                // Change API connection string
            }
        }


        private void btnStock_Click(object sender, EventArgs e)
        {
            String_MainAsset = "Stock";
            lblSC.Text = "Stock";
            //Price_Check_Stock_MarketWatch(txtAsset.Text);
        }

        #endregion

        #region Asset_Formation

        List<Tuple<string, string>> asset_list = new List<Tuple<string, string>>();

        private void Order_Rows()
        {
            for (int i = 0; i < dv.Table.Rows.Count; i++)
            {
                dv.Table.Rows[i][7] = i;
            }
        }

        private void Form_Asset_List()
        {
            asset_list.Clear();
            bool not_empty = false;
            for (int i = 0; i < dv.Table.Rows.Count; i++)
            {
                string asset = dv.Table.Rows[i][0].ToString();

                string identity = dv.Table.Rows[i][3].ToString();
                if (!not_empty) { asset_list.Add(Tuple.Create(asset, identity)); not_empty = true; }
                else
                {
                    bool Valid = asset_list.All(item => item.Item1 != asset);
                    if (Valid)
                    {
                        asset_list.Add(Tuple.Create(asset, identity));
                    }
                }
            }
        }



        #endregion

        class Main_
        {
            int Zero_Checker = 0;
            Sound sound = new Sound();

            private async void ShowAlert()
            {
                instance.Alert_ = true;
                instance.action[0].Invoke();
                instance.AlertGoing = true;

                while (instance.AlertGoing)
                {
                    sound.Play();
                }
                instance.Alert_ = false;
            }

            void Change_Checkbox() { if (instance.Stock_Data == 0) { instance.Stock_Data = 1; } else { instance.Stock_Data = 0; }; }

            public async void Run()
            {
                await Task.Delay(100);
                //instance.Running = true;
                
                instance.ToRun = true;
                while (instance.ToRun)
                {
                    // Asset | Price/Alert | Price | Crypto/Stock | AboveBelow | Alerted | Index
                    foreach (Tuple<string, string> x in instance.asset_list) // Asset, Identity
                    {
                        string asset = x.Item1;
                        string Identity = x.Item2; // What type Crypto, BTC, Stock

                        if (!Check_Stocks && Identity == "Stock") { continue; }

                        bool BTC = false;
                        float Price = 0;

                        Price = Get_Price(asset, Identity);

                        void Func_()
                        {
                            for (int i = 0; i < instance.dv.Table.Rows.Count; i++)
                            {
                                if (instance.dv.Table.Rows[i][0].ToString() == asset)
                                {
                                    if (Price == 0) { Price = float.Parse(instance.dv.Table.Rows[i][2].ToString()); }
                                    if (!BTC) { instance.dv.Table.Rows[i][2] = Price; }
                                    else { instance.dv.Table.Rows[i][2] = Price.ToString("N8"); }
                                    //MessageBox.Show(x.Item1 + " " + x.Item2 + " " + Price.ToString(), "Testing");
                                    Check_Alert(i);
                                }
                            }
                        }
                        Func_();

                        if (!instance.ThreadRun) { instance.ToRun = false; break; }
                    }
                    await Task.Delay(5);
                }
                instance.Running = false;
            }

            private float Get_Price(string Asset, string Identity)
            {
                float Price = 0;

                if (Identity == "Crypto") { Price = instance.crypto.Price_Check_Crypto(Asset); }
                else if (Identity == "BTC") { Price = float.Parse(instance.crypto.Price_Check_BTC(Asset).ToString()); }
                else
                {
                    if (instance.Stock_Data == 1)
                    {
                        Price = instance.stock.Price_Check_MarketWatch(Asset);
                        if (Price != 0) { Zero_Checker = 0; }
                        else if (Price == 0 | Price > 2000) { Price = instance.stock.Price_Check(Asset); Zero_Checker = +1; }
                        //if (Price > 3000) { return 0; }
                    }
                    else if (instance.Stock_Data == 0)
                    {
                        Price = instance.stock.Price_Check(Asset);
                        if (Price != 0) { Zero_Checker = 0; }
                        else if (Price == 0 | Price > 2000) { Price = instance.stock.Price_Check_MarketWatch(Asset); Zero_Checker = +1; }
                        //if (Price > 3000) { return 0; }
                    }
                }

                if (Zero_Checker == 3) { Change_Checkbox(); Zero_Checker = 0; }


                return Price;
            }

            private void Check_Alert(int i)
            {
                DataRow row = instance.dv.Table.Rows[i];

                string Asset = row[0].ToString();
                float PriceAlert = float.Parse(row[1].ToString());
                float Price = float.Parse(row[2].ToString());
                string MainAsset = row[3].ToString();
                string AboveBelow = row[4].ToString();


                if (bool.Parse(row[5].ToString()) == false)
                {
                    if (instance.view == 2)
                    {
                        Color c = instance.Colour_Scheme_Price(PriceAlert, AboveBelow, Price);

                        instance.Colour_Row_Text(i, c);
                    }

                    //instance.PopupText_Button = instance.PopupText(Asset, AboveBelow, PriceAlert.ToString());

                    if (!instance.ThreadRun) { return; }

                    if (MainAsset == "Crypto")
                    {
                        var New_Price = instance.crypto.Price_Check_Crypto_History(Asset, row[6].ToString()); // High,Low
                        if (AboveBelow == "Above")
                        {
                            Price = New_Price.Item1;
                        }
                        else
                        {
                            Price = New_Price.Item2;
                        }
                    }


                    //Check above| below against price
                    if (AboveBelow == "Above") // When price goes above
                    {
                        if (Price > PriceAlert && !instance.Alert_ && Price != 0)
                        {
                            instance.txtSearch.Invoke((MethodInvoker)(() => instance.txtSearch.Text = string.Empty));
                            instance.action[1].Invoke();
                            instance.messagebox_text.Item1 = Asset;
                            instance.messagebox_text.Item2 = PriceAlert.ToString();
                            instance.Colour_Row(i);
                            ShowAlert();
                            row[5] = true;
                        }
                    }
                    else // When price goes below
                    {
                        if (Price < PriceAlert && !instance.Alert_ && Price != 0)
                        {
                            instance.txtSearch.Invoke((MethodInvoker)(() => instance.txtSearch.Text = string.Empty));
                            instance.action[1].Invoke();
                            instance.messagebox_text.Item1 = Asset;
                            instance.messagebox_text.Item2 = PriceAlert.ToString();
                            instance.Colour_Row(i);
                            ShowAlert();
                            row[5] = true;
                        }
                    }

                    long time = long.Parse(instance.Get_Current_Time());
                    long alert_time = long.Parse(row[6].ToString());
                    if ((time - alert_time) > 3600000) // 60 Minutes
                    {
                        instance.dataGridView1.Rows[i].Cells[6].Value = (time + 1800000).ToString();
                    }
                }
            }
        }



        #region SaveAndLoad

        private void btnSave_Click(object sender, EventArgs e)
        {
            Save_Info();
        }

        private void Save(string[] lis, string path)
        {
            using (var fs = new StreamWriter(path))
            {
                foreach (string line in lis)
                {
                    fs.WriteLine(line);
                }
                fs.Close();
                fs.Dispose();
            }
        }

        private void Save_Info()
        {
            // Seperator = |
            string temp_info = string.Empty;
            info.Clear();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                for (int x = 0; x < 7; x++)
                {
                    temp_info += dataGridView1.Rows[i].Cells[x].Value.ToString() + "|";
                }
                info.Add(temp_info);
                temp_info = string.Empty;
            }
            Save(info.ToArray(), "Alerts.txt");
        }

        private static IEnumerable<string> ReadLines(string path)
        {
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 0x1000, FileOptions.SequentialScan))
            using (var sr = new StreamReader(fs, Encoding.UTF8))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    yield return line;
                }
                sr.Close();
            }
        }

        private void Load_Info()
        {
            var lines = ReadLines("Alerts.txt").ToArray();
            foreach (string line in lines)
            {
                var nline = line.Split('|');
                dataGridView1.Rows.Add(nline[0], nline[1], nline[2], nline[3], nline[4], nline[5]);
            }
        }

        #endregion

        #region Alert
        private void MessageAlertBox()
        {
            string text = string.Format("{0} is crossing {1}!", messagebox_text.Item1, messagebox_text.Item2);
            DialogResult result = MessageBox.Show(text, "Alert!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            if (result == DialogResult.OK)
            {
                AlertGoing = false;
            }
            else
            {
                AlertGoing = false;
            }
            Alert_Thread.Join();
            Alert_Thread = null;
        }

        private void Alert_Start()
        {
            Alert_Thread = new Thread(MessageAlertBox);
            Alert_Thread.IsBackground = true;
            Alert_Thread.Start();
        }

        #endregion

        #region Colouring

        public void Colour_All_Rows_If_Alerted()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                bool Alerted = bool.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString());
                DataGridViewRow irow = dataGridView1.Rows[i];
                if (Alerted) { irow.DefaultCellStyle.ForeColor = Color.MediumVioletRed; }
                else { irow.DefaultCellStyle.ForeColor = Color.LimeGreen; }
            }
        }

        public void Colour_All_Nothing()
        {
            //dataGridView1.SelectAll();
            //dataGridView1.CurrentRow.DefaultCellStyle.SelectionForeColor = Color.FromArgb(192, 255, 255);
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewRow irow = dataGridView1.Rows[i];
                irow.DefaultCellStyle.ForeColor = Color.Empty;
            }
        }

        private Color Colour_Scheme_Price(float Alert, string Heading, float Price)
        {
            double pBelow(float x) { return Alert * (1 + (x / 100)); };
            double pAbove(float x) { return Alert * (1 - (x / 100)); };
            double pPrice(float x) { if (Heading == "Above") { return pAbove(x); } else { return pBelow(x); } }

            Color nColor = Color.White;

            double FirstPercent = pPrice(2);
            double SecondPercentage = pPrice(5);
            double ThirdPercentage = pPrice(10);


            if (Heading == "Above")
            {
                if (Price > FirstPercent) { nColor = Color.IndianRed; }
                else if (Price > SecondPercentage && Price < FirstPercent) { nColor = Color.Orange; }
                else if (Price > ThirdPercentage && Price < SecondPercentage) { nColor = Color.Yellow; }
                else { nColor = Color.Aqua; }
            }
            else
            {
                if (Price < FirstPercent) { nColor = Color.IndianRed; }
                else if (Price < SecondPercentage && Price > FirstPercent) { nColor = Color.Orange; }
                else if (Price < ThirdPercentage && Price > SecondPercentage) { nColor = Color.Yellow; }
                else { nColor = Color.Aqua; }
            }

            return nColor;
        }

        public void Colour_All_Near()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewRow irow = dataGridView1.Rows[i];
                // Price
                float Alert = float.Parse(irow.Cells[1].Value.ToString());
                // Get Above|Below
                string Heading = irow.Cells[4].Value.ToString();

                float Price = float.Parse(irow.Cells[2].Value.ToString());

                Color c = Colour_Scheme_Price(Alert, Heading, Price);


                irow.DefaultCellStyle.ForeColor = c;
                //this.dataGridView1.DefaultCellStyle.Font = new Font("Tahoma", 15);
            }
        }

        private void Colour_Row(int row)
        {
            DataGridViewRow irow = dataGridView1.Rows[row];
            irow.DefaultCellStyle.BackColor = Color.Green;
        }

        private void Colour_Row_Text(int row, Color color)
        {
            if (txtSearch.Text == string.Empty)
            {
                try
                {
                    DataGridViewRow irow = dataGridView1.Rows[row];
                    irow.DefaultCellStyle.ForeColor = color;
                }
                catch { }
            }
        }


        #endregion

        #region Checkboxes

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                Main_ m = new Main_();
                thread = new Thread(() => m.Run());
                thread.IsBackground = true;
                ThreadRun = true;
                thread.Start();
            }
            else
            {
                ThreadRun = false;
                thread.Join();
                thread = null;
                GC.Collect();
            }
        }

        private void cBoxStocks_CheckedChanged(object sender, EventArgs e) // Don't Check Stocks 
        {
            if (cBoxStocks.Checked) { Check_Stocks = false; } // Turn Off Checker
            else { Check_Stocks = true; }
        }

        #endregion

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Row_Selected = dataGridView1.CurrentCell.RowIndex;
            string asset = dataGridView1.Rows[Row_Selected].Cells[0].Value.ToString();
            string alert = dataGridView1.Rows[Row_Selected].Cells[1].Value.ToString();
            lblSelected.Text = asset + " " + alert;
            if(view == 2)
            {
                Color x = dataGridView1.Rows[Row_Selected].DefaultCellStyle.ForeColor;
                dataGridView1.CurrentRow.DefaultCellStyle.SelectionForeColor = x;
            }
            else
            {
                if(dataGridView1.CurrentRow.DefaultCellStyle.SelectionForeColor != Color.FromArgb(192, 255, 255))
                dataGridView1.CurrentRow.DefaultCellStyle.SelectionForeColor = Color.FromArgb(192, 255, 255);
            }
            if (dataGridView1.CurrentRow.DefaultCellStyle.BackColor != Color.FromArgb(64, 64, 64))
            {
                dataGridView1.CurrentRow.DefaultCellStyle.BackColor = Color.FromArgb(64, 64, 64);
            }
        }

        void TurnOff_Crypto() { checkBox1.Checked = false; }
        void TurnOff_Stocks() { cBoxStocks.Checked = false; }

        private void Scalper_Load(object sender, EventArgs e)
        {
            btnExit.BackgroundImage = Bitmap.FromFile("X.png");
            dataGridView1.ScrollBars = ScrollBars.None;
            action.Add(() => Alert_Start()); // Turn on messagebox [0]
            action.Add(() => Clear_Search());
            action.Add(() => TurnOff_Crypto()); //2
            action.Add(() => TurnOff_Stocks()); //3

            instance.Order_Rows();
            instance.Form_Asset_List();

            // For Searching Asset
            LView_Setup();

            btnSettings.BackgroundImage = Properties.Resources.Settings1;


        }

        private void Clear_Search() { dv.RowFilter = null; }

        private async void Terminate()
        {
            dv.RowFilter = null;
            Save_Info();
            Application.Exit();
            ThreadRun = false;
            checkBox1.Checked = false;
            await Task.Delay(1000);
            Save_Info();
            if (Alert_Thread != null)
            {
                if (Alert_Thread.ThreadState == ThreadState.Running) { Alert_Thread.Join(); }
            }
            if (thread != null)
            {
                if (thread.ThreadState == ThreadState.Running) { thread.Join(); }
            }
            Application.Exit();
        }

        private void txtAsset_TextChanged(object sender, EventArgs e)
        {
            string txt = txtAsset.Text;
            if (txt != string.Empty)
            {
                char last = Convert.ToChar(txt.Remove(0, txt.Length - 1));
                if (char.IsLower(last)) { last = char.ToUpper(last); }
                txt = txt.Substring(0, txt.Length - 1) + last;
                txtAsset.Text = txt;
                txtAsset.SelectionStart = txtAsset.Text.Length;
                txtAsset.SelectionLength = 0;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Terminate();
        }

        #region Dragable

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void headerPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void lblTitle_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        #endregion

        // Scroll 3 rows with one roller

        private void mousewheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0 && dataGridView1.FirstDisplayedScrollingRowIndex > 0)
            {
                dataGridView1.FirstDisplayedScrollingRowIndex--;
            }
            else if (e.Delta < 0)
            {
                dataGridView1.FirstDisplayedScrollingRowIndex++;
            }
        }

        class Turn
        {
            bool WasChecked = false;
            bool StocksChecked = false;

            public void On()
            {
                if (WasChecked) { instance.checkBox1.Checked = true; }
                if (StocksChecked) { instance.cBoxStocks.Checked = true; }
            }
            public void Off()
            {
                instance.ThreadRun = false; instance.StocksRunning = false;
                if (instance.checkBox1.Checked) { WasChecked = true; instance.checkBox1.Checked = false; }
                if (instance.cBoxStocks.Checked) { StocksChecked = true; instance.cBoxStocks.Checked = false; }
            }

            public void Reset()
            {
                WasChecked = false;
                StocksChecked = false;
            }
        }


        class Data
        {
            private string[] lines;
            public int Rows = 0;
            DataTable table = new DataTable();

            private static IEnumerable<string> ReadLines(string path)
            {
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 0x1000, FileOptions.SequentialScan))
                using (var sr = new StreamReader(fs, Encoding.UTF8))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        yield return line;
                    }
                    sr.Close();
                }
            }

            private void Line_Sorter_Adder()
            {
                string[] temp_split;
                string Asset, Alert, Price, MainAsset, AboveBelow, Alerted, Time;
                try { instance.dataGridView1.Rows.Clear(); } catch { }
                table.Clear();
                int Rows = 0;

                foreach (string line in lines)
                {
                    temp_split = line.Split('|');

                    Asset = temp_split[0];
                    Alert = temp_split[1];
                    Price = temp_split[2];
                    MainAsset = temp_split[3];
                    AboveBelow = temp_split[4];
                    Alerted = temp_split[5];
                    Time = temp_split[6];

                    table.Rows.Add(Asset, Alert, Price, MainAsset, AboveBelow, Alerted, Time, Rows);
                    Rows++;
                }
            }

            private void Create_Datatable()
            {
                // Asset | Price/Alert | Price | Crypto/Stock | AboveBelow | Alerted | Index
                table.Clear();
                string[] To_Add = { "Asset", "Alert", "Price", "MainAsset", "AboveBelow", "Alerted","Time", "Index" };
                for (int i = 0; i < To_Add.Length; i++)
                {
                    table.Columns.Add(To_Add[i]);
                }
            }

            //public void Save_Info(bool Backup = false)
            //{
            //    List<string> info_list = new List<string>();

            //    instance.dv.RowFilter = null;
            //    instance.dt = instance.dv.ToTable();
            //    foreach (DataRow cell in instance.dt.Rows)
            //    {
            //        string new_line;
            //        Account = Details.instance.Unscrambler(cell[0].ToString(), true);
            //        Username = Details.instance.Unscrambler(cell[1].ToString(), true);
            //        Password = Details.instance.Unscrambler(cell[2].ToString(), true);
            //        Email = Details.instance.Unscrambler(cell[3].ToString(), true);
            //        Link = Details.instance.Unscrambler(cell[4].ToString(), true);
            //        new_line = Account + " 98 " + Username + " 98 " + Password + " 98 " + Email + " 98 " + Link + " 98 ";
            //        info_list.Add(new_line);
            //    }
            //    if (!Backup) { instance.Save(info_list.ToArray(), "Info.txt"); }
            //    else if (Backup) { instance.Save(info_list.ToArray(), "ManualBackup.txt"); }
            //}

            public void Load_Info() { Create_Datatable(); Load("Alerts.txt"); }

            private void Load(string FileName)
            {
                lines = ReadLines(FileName).ToArray();

                Line_Sorter_Adder();

                instance.dataGridView1.Columns.Clear();

                instance.dt = table;

                instance.dv = new DataView(instance.dt);
                instance.dataGridView1.DataSource = instance.dv;

                instance.dataGridView1.Columns[3].Visible = false;
                instance.dataGridView1.Columns[4].Visible = false;
                instance.dataGridView1.Columns[5].Visible = false;
                instance.dataGridView1.Columns[6].Visible = false;
                instance.dataGridView1.Columns[7].Visible = false;

                instance.dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                instance.dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                instance.dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        #region Search

        private void lblType_Click(object sender, EventArgs e)
        {
            string[] myarray = { "", "Crypto", "Stock", "BTC" };
            if (index_adv_search_type == 3) { index_adv_search_type = 0; }
            else { index_adv_search_type++; }

            lblType.Text = myarray[index_adv_search_type];
            Search();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e) { Search(); }

        private void Search()
        {
            if (lblType.Text != "")
            {
                dv.RowFilter = "MainAsset = '" + lblType.Text + "'" + "AND Asset LIKE '%" + txtSearch.Text + "%'";
            }
            else
            {
                dv.RowFilter = "Asset LIKE '%" + txtSearch.Text + "%'";
            }
            if (view == 2) { Colour_All_Near(); }
        }


        private void lblType_DoubleClick(object sender, EventArgs e)
        {
            index_adv_search_type = 0;
            lblType.Text = "";
            Search();
        }

        #endregion

        #region Removed_Add_Back
        private void lblRemoved1_DoubleClick(object sender, EventArgs e) { Re_Add(lblRemoved1.Text, 0); }

        private void lblRemoved2_DoubleClick(object sender, EventArgs e) { Re_Add(lblRemoved2.Text, 1); }

        private async void Re_Add(string row, int index)
        {
            string[] array = row.Split(" ".ToCharArray());

            StocksRunning = false;
            Running = false;
            await Task.Delay(1000);
            turn.Off();
            await Task.Delay(1000);

            while (true)
            {
                if (Running | StocksRunning | ToRun) { await Task.Delay(100); }
                else { break; }
            }

            // Asset | Price/Alert | Price | Crypto/Stock | AboveBelow | Alerted | Time | Index
            dv.Table.Rows.Add(array[0], array[1], array[2], array[3], array[4], array[5],array[6], 0);
            await Task.Delay(100);
            Order_Rows();
            Form_Asset_List();
            await Task.Delay(1000);
            turn.On();
            turn.Reset();
            if (view == 1) { Colour_All_Rows_If_Alerted(); }
            if (view == 2) { Colour_All_Near(); }

            if (index == 0)
            {
                lblRemoved1.Text = lblRemoved2.Text;
                lblRemoved2.Text = string.Empty;
            }
            else
            {
                lblRemoved2.Text = string.Empty;
            }
        }

        #endregion

        private string Get_Current_Time()
        {
            DateTimeOffset now = DateTimeOffset.Now;
            string milliseconds = now.ToUnixTimeMilliseconds().ToString();
            milliseconds = milliseconds.Substring(0,milliseconds.Length - 3)+"000";
            return milliseconds;
        }

        private void foo()
        {
            string[] st = stock.Price_Check_Advanced("VICI");
            string[] advanced_options = new string[] { "Change", "Percent", "Premarket Price", "Change", "Percent" };

            foreach (string option in advanced_options)
            {
                DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                col.HeaderText = option;
                if (option == "Premarket Price") { col.HeaderCell.Style.Font = new Font("Segoe UI", 8F, FontStyle.Bold); }
                col.Name = option;
                dataGridView1.Columns.Add(col);
            }
        }

        #region Settings

        public bool settings_shown = false;
        Settings settings;

        private void btnSettings_Click(object sender, EventArgs e)
        {
            if (!settings_shown)
            {
                settings = new Settings();
                settings.Show();
                settings_shown = true;
            }
            else
            {
                settings.Dispose();
                settings_shown = false;
            }
        }

        #endregion


        // ListView Setup
        private void LView_Setup() 
        {
            // for changing row views, whether it was alerted or how near it is to the alert
            listView.Items.Add("Clear");
            listView.Items.Add("Alerted");
            listView.Items.Add("How Near");
        }

        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView.SelectedIndex == 0) { Colour_All_Nothing(); }
            if (listView.SelectedIndex == 1) { Colour_All_Rows_If_Alerted(); }
            if (listView.SelectedIndex == 2) { Colour_All_Near(); }
            view = Convert.ToByte(listView.SelectedIndex);
        }
    }
}
