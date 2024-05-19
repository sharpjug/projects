using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChartPriceScalper.functions
{
    internal class Sound
    {
        public Scalper mainform = Scalper.instance;

        int Device_Number = -1;
        bool Change_Index = true;
        List<string> audio_names = new List<string>();
        int time_sleep_alert_length = 0;
        WaveFileReader wav = new WaveFileReader(@"alert.wav");

        public void Setup_Alert_Length() { time_sleep_alert_length = Convert.ToInt32(wav.TotalTime.TotalMilliseconds); }

        private void Save_Device_Number(int i)
        {
            Properties.Settings.Default.DeviceNo = Convert.ToByte(i);
            Properties.Settings.Default.Save();
            Device_Number = i;
        }

        public async void Play()
        {
            var output = new WaveOutEvent { DeviceNumber = Device_Number };
            output.Init(wav);
            output.Play();
            await Task.Delay(time_sleep_alert_length);
            output.Dispose();
            wav.Dispose();
            wav = new WaveFileReader(@"alert.wav");
        }

        public void Sound_Changed(int i, System.Windows.Forms.Label label)
        {
            if (Change_Index)
            {
                int selected = i;
                Device_Number = selected - 1;
                label.Text = audio_names[selected];
                Change_Index = false;
                selected = -1;
                Save_Device_Number(Device_Number);

            }
            else
            {
                Change_Index = true;
            }
        }

        public void Audio_Start(ListBox listSound, System.Windows.Forms.Label lblSound)
        {
            for (int n = -1; n < WaveOut.DeviceCount; n++)
            {
                var caps = WaveOut.GetCapabilities(n);
                audio_names.Add(caps.ProductName);
                string[] caps_name = caps.ProductName.Split(' ');
                listSound.Items.Add(caps_name[0]);
            }
            lblSound.Text = audio_names[0];
        }
    }
}
