namespace Stock_Scalper
{
    partial class Stock_Analysis
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea7 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend7 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea8 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend8 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea9 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend9 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea10 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend10 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea11 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend11 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea12 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend12 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.ChartRevenue = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lbl1 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.ChartNetIncome = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.ChartFreeCashFlow = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.ChartEBITDA = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.ChartEPS = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.ChartSharesOutstanding = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.ChartRevenue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChartNetIncome)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChartFreeCashFlow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChartEBITDA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChartEPS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChartSharesOutstanding)).BeginInit();
            this.SuspendLayout();
            // 
            // ChartRevenue
            // 
            this.ChartRevenue.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea7.Name = "ChartArea1";
            this.ChartRevenue.ChartAreas.Add(chartArea7);
            legend7.Name = "Legend1";
            this.ChartRevenue.Legends.Add(legend7);
            this.ChartRevenue.Location = new System.Drawing.Point(477, 70);
            this.ChartRevenue.Name = "ChartRevenue";
            this.ChartRevenue.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Light;
            this.ChartRevenue.Size = new System.Drawing.Size(455, 253);
            this.ChartRevenue.TabIndex = 0;
            this.ChartRevenue.Text = "chart1";
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtSearch.Location = new System.Drawing.Point(416, 12);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(116, 20);
            this.txtSearch.TabIndex = 1;
            // 
            // lbl1
            // 
            this.lbl1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lbl1.AutoSize = true;
            this.lbl1.Location = new System.Drawing.Point(366, 15);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(44, 13);
            this.lbl1.TabIndex = 2;
            this.lbl1.Text = "Search:";
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnSearch.Location = new System.Drawing.Point(538, 10);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "Go";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // ChartNetIncome
            // 
            this.ChartNetIncome.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea8.Name = "ChartArea1";
            this.ChartNetIncome.ChartAreas.Add(chartArea8);
            legend8.Name = "Legend1";
            this.ChartNetIncome.Legends.Add(legend8);
            this.ChartNetIncome.Location = new System.Drawing.Point(12, 70);
            this.ChartNetIncome.Name = "ChartNetIncome";
            this.ChartNetIncome.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Light;
            this.ChartNetIncome.Size = new System.Drawing.Size(455, 253);
            this.ChartNetIncome.TabIndex = 4;
            this.ChartNetIncome.Text = "chart1";
            // 
            // ChartFreeCashFlow
            // 
            this.ChartFreeCashFlow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea9.Name = "ChartArea1";
            this.ChartFreeCashFlow.ChartAreas.Add(chartArea9);
            legend9.Name = "Legend1";
            this.ChartFreeCashFlow.Legends.Add(legend9);
            this.ChartFreeCashFlow.Location = new System.Drawing.Point(12, 329);
            this.ChartFreeCashFlow.Name = "ChartFreeCashFlow";
            this.ChartFreeCashFlow.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Light;
            this.ChartFreeCashFlow.Size = new System.Drawing.Size(455, 253);
            this.ChartFreeCashFlow.TabIndex = 5;
            this.ChartFreeCashFlow.Text = "chart1";
            // 
            // ChartEBITDA
            // 
            this.ChartEBITDA.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea10.Name = "ChartArea1";
            this.ChartEBITDA.ChartAreas.Add(chartArea10);
            legend10.Name = "Legend1";
            this.ChartEBITDA.Legends.Add(legend10);
            this.ChartEBITDA.Location = new System.Drawing.Point(477, 329);
            this.ChartEBITDA.Name = "ChartEBITDA";
            this.ChartEBITDA.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Light;
            this.ChartEBITDA.Size = new System.Drawing.Size(455, 253);
            this.ChartEBITDA.TabIndex = 6;
            this.ChartEBITDA.Text = "chart1";
            // 
            // ChartEPS
            // 
            this.ChartEPS.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea11.Name = "ChartArea1";
            this.ChartEPS.ChartAreas.Add(chartArea11);
            legend11.Name = "Legend1";
            this.ChartEPS.Legends.Add(legend11);
            this.ChartEPS.Location = new System.Drawing.Point(12, 588);
            this.ChartEPS.Name = "ChartEPS";
            this.ChartEPS.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Light;
            this.ChartEPS.Size = new System.Drawing.Size(455, 253);
            this.ChartEPS.TabIndex = 7;
            this.ChartEPS.Text = "chart1";
            // 
            // ChartSharesOutstanding
            // 
            this.ChartSharesOutstanding.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea12.Name = "ChartArea1";
            this.ChartSharesOutstanding.ChartAreas.Add(chartArea12);
            legend12.Name = "Legend1";
            this.ChartSharesOutstanding.Legends.Add(legend12);
            this.ChartSharesOutstanding.Location = new System.Drawing.Point(477, 588);
            this.ChartSharesOutstanding.Name = "ChartSharesOutstanding";
            this.ChartSharesOutstanding.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Light;
            this.ChartSharesOutstanding.Size = new System.Drawing.Size(455, 253);
            this.ChartSharesOutstanding.TabIndex = 8;
            this.ChartSharesOutstanding.Text = "chart1";
            // 
            // Stock_Analysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(941, 849);
            this.Controls.Add(this.ChartSharesOutstanding);
            this.Controls.Add(this.ChartEPS);
            this.Controls.Add(this.ChartEBITDA);
            this.Controls.Add(this.ChartFreeCashFlow);
            this.Controls.Add(this.ChartNetIncome);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.lbl1);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.ChartRevenue);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(957, 888);
            this.MinimumSize = new System.Drawing.Size(957, 888);
            this.Name = "Stock_Analysis";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Stock_Analysis";
            ((System.ComponentModel.ISupportInitialize)(this.ChartRevenue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChartNetIncome)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChartFreeCashFlow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChartEBITDA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChartEPS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChartSharesOutstanding)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart ChartRevenue;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lbl1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DataVisualization.Charting.Chart ChartNetIncome;
        private System.Windows.Forms.DataVisualization.Charting.Chart ChartFreeCashFlow;
        private System.Windows.Forms.DataVisualization.Charting.Chart ChartEBITDA;
        private System.Windows.Forms.DataVisualization.Charting.Chart ChartEPS;
        private System.Windows.Forms.DataVisualization.Charting.Chart ChartSharesOutstanding;
    }
}