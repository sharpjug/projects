using ChartPriceScalper.functions;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChartPriceScalper
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }

        Scalper mainform = Scalper.instance;

        #region Dragable

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void headerPanel_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }

        private void Settings_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }


        #endregion

        private void btnClose_Click(object sender, EventArgs e) { mainform.settings_shown = false; this.Dispose(); }

        private void Settings_Load(object sender, EventArgs e)
        {
            //time_sleep_alert_length = Convert.ToInt32(wav.TotalTime.TotalMilliseconds);
            listSetup();
            sound.Audio_Start(listSound, lblSound);
        }

        #region Lists
        // For different exchanges when one may be down and different row views

        private void listSetup() // adding the selectable choice
        {
            // for changing exchanges to view
            listExchange.Items.Add("Yahoo");
            listExchange.Items.Add("Marketwatch");
        }

        private void listExchange_SelectedIndexChanged(object sender, EventArgs e) // new exchange is chosen
        {
            mainform.Stock_Data = Convert.ToByte(listExchange.SelectedIndex);
        }

        #endregion

        private void cBoxTop_CheckedChanged(object sender, EventArgs e) { if (cBoxTop.Checked) { mainform.TopMost = true; } else { mainform.TopMost = false; } }



        Sound sound = new Sound();
        private void listSound_SelectedIndexChanged(object sender, EventArgs e) { sound.Sound_Changed(listSound.SelectedIndex,lblSound); }
        private async void btnSoundTest_Click(object sender, EventArgs e) { await Task.Delay(50); sound.Play(); }

    }
}
