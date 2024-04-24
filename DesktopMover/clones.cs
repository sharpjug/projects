using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DesktopMover
{
    public partial class Clones : Form
    {
        public Clones(int Number)
        {
            InitializeComponent();
            desktopNumber = Number;
        }

        private int desktopNumber = 0;

        #region Dragable

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void Mover_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        #endregion

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        private void btnRight_Click(object sender, EventArgs e)
        {
            Mover.move(1, int.Parse(lblNo.Text) - 1);
        }

        private void Clones_Load(object sender, EventArgs e)
        {
            Mover.forms.Add(this);
            Mover.forms_intptr.Add(this.Handle);
            Mover.forms_labels.Add(lblNo);
            lblNo.Text = (desktopNumber + 1).ToString();

            toolTip1.Active = true;
            toolTip1.SetToolTip(btnDeleteWindow, "Removes the window");
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            Mover.move(-1, int.Parse(lblNo.Text) - 1);
        }

        private void btnDeleteWindow_Click(object sender, EventArgs e)
        {
            Mover.close_form_(int.Parse(lblNo.Text) - 1);
        }
    }
}
