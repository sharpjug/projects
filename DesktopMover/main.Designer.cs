
namespace DesktopMover
{
    partial class Mover
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            btnLeft = new System.Windows.Forms.Button();
            btnRight = new System.Windows.Forms.Button();
            btnExit = new System.Windows.Forms.Button();
            btnAdd = new System.Windows.Forms.Button();
            lblNo = new System.Windows.Forms.Label();
            btnTeleport = new System.Windows.Forms.Button();
            toolTip1 = new System.Windows.Forms.ToolTip(components);
            SuspendLayout();
            // 
            // btnLeft
            // 
            btnLeft.Location = new System.Drawing.Point(12, 26);
            btnLeft.Name = "btnLeft";
            btnLeft.Size = new System.Drawing.Size(63, 45);
            btnLeft.TabIndex = 0;
            btnLeft.Text = "<";
            btnLeft.UseVisualStyleBackColor = true;
            btnLeft.Click += btnLeft_Click;
            // 
            // btnRight
            // 
            btnRight.Location = new System.Drawing.Point(81, 26);
            btnRight.Name = "btnRight";
            btnRight.Size = new System.Drawing.Size(63, 45);
            btnRight.TabIndex = 1;
            btnRight.Text = ">";
            btnRight.UseVisualStyleBackColor = true;
            btnRight.Click += btnRight_Click;
            // 
            // btnExit
            // 
            btnExit.Location = new System.Drawing.Point(132, -1);
            btnExit.Name = "btnExit";
            btnExit.Size = new System.Drawing.Size(25, 25);
            btnExit.TabIndex = 2;
            btnExit.Text = "X";
            btnExit.UseVisualStyleBackColor = true;
            btnExit.Click += btnExit_Click;
            // 
            // btnAdd
            // 
            btnAdd.Location = new System.Drawing.Point(67, -1);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new System.Drawing.Size(25, 25);
            btnAdd.TabIndex = 3;
            btnAdd.Text = "+";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // lblNo
            // 
            lblNo.Location = new System.Drawing.Point(0, -1);
            lblNo.Name = "lblNo";
            lblNo.Size = new System.Drawing.Size(25, 25);
            lblNo.TabIndex = 4;
            lblNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnTeleport
            // 
            btnTeleport.Location = new System.Drawing.Point(98, -1);
            btnTeleport.Name = "btnTeleport";
            btnTeleport.Size = new System.Drawing.Size(25, 25);
            btnTeleport.TabIndex = 5;
            btnTeleport.Text = ".";
            btnTeleport.UseVisualStyleBackColor = true;
            btnTeleport.Click += btnTeleport_Click;
            // 
            // Mover
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(156, 83);
            ControlBox = false;
            Controls.Add(btnTeleport);
            Controls.Add(lblNo);
            Controls.Add(btnAdd);
            Controls.Add(btnExit);
            Controls.Add(btnRight);
            Controls.Add(btnLeft);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Mover";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Mover";
            TopMost = true;
            Load += Mover_Load;
            MouseDown += Mover_MouseDown;
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button btnLeft;
        private System.Windows.Forms.Button btnRight;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label lblNo;
        private System.Windows.Forms.Button btnTeleport;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}

