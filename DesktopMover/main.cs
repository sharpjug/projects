using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopMover
{
    public partial class Mover : Form
    {
        public Mover()
        {
            InitializeComponent();
            lblNo.Text = "1";
        }

        public static List<Form> forms = new List<Form>();
        public static List<IntPtr> forms_intptr = new List<IntPtr>();
        public static List<Label> forms_labels = new List<Label>();
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
            SetForegroundWindow(forms_intptr[1]);
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            SetForegroundWindow(forms_intptr[forms_intptr.Count - 1]);
        }

        public static void move(int Direction, int FormNumber)
        {
            int move_to = FormNumber + Direction;
            if (move_to < 0) { move_to = forms.Count - 1; }
            else if (move_to == forms.Count) { move_to = 0; }
            SetForegroundWindow(forms_intptr[move_to]);
        }

        public static void close_form_(int FormNumber)
        {
            forms[FormNumber].Close();
            forms.RemoveAt(FormNumber);
            forms_intptr.RemoveAt(FormNumber);
            forms_labels.RemoveAt(FormNumber);

            for (int i = 0; i < forms.Count; i++)
            {
                forms_labels[i].Text = (i + 1).ToString();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Clones clones = new Clones(forms.Count);
            clones.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Mover_Load(object sender, EventArgs e)
        {
            forms.Add(this);
            forms_intptr.Add(this.Handle);
            forms_labels.Add(lblNo);

            toolTip1.Active = true;
            toolTip1.SetToolTip(btnExit, "Closes program");
            toolTip1.SetToolTip(btnAdd, "Adds another mover window");
            toolTip1.SetToolTip(btnTeleport, "Teleports all windows to same location on the screen");
        }

        private void btnTeleport_Click(object sender, EventArgs e)
        {
            foreach(Form form in forms)
            {
                form.DesktopLocation = this.DesktopLocation;
            }
        }
    }
}
