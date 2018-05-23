namespace GISWinApp
{
    partial class LogFrm
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
            this.dataLogs = new System.Windows.Forms.DataGridView();
            this.Temp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Hum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NH3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Light = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataLogs)).BeginInit();
            this.SuspendLayout();
            // 
            // dataLogs
            // 
            this.dataLogs.AllowUserToAddRows = false;
            this.dataLogs.AllowUserToDeleteRows = false;
            this.dataLogs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataLogs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Temp,
            this.Hum,
            this.NH3,
            this.Light,
            this.Date});
            this.dataLogs.Location = new System.Drawing.Point(12, 11);
            this.dataLogs.Name = "dataLogs";
            this.dataLogs.ReadOnly = true;
            this.dataLogs.RowTemplate.Height = 23;
            this.dataLogs.Size = new System.Drawing.Size(613, 431);
            this.dataLogs.TabIndex = 44;
            // 
            // Temp
            // 
            this.Temp.HeaderText = "温度(℃)";
            this.Temp.Name = "Temp";
            this.Temp.ReadOnly = true;
            // 
            // Hum
            // 
            this.Hum.HeaderText = "湿度(%RH)";
            this.Hum.Name = "Hum";
            this.Hum.ReadOnly = true;
            // 
            // NH3
            // 
            this.NH3.HeaderText = "氨气(ppm)";
            this.NH3.Name = "NH3";
            this.NH3.ReadOnly = true;
            // 
            // Light
            // 
            this.Light.HeaderText = "光照(Lx)";
            this.Light.Name = "Light";
            this.Light.ReadOnly = true;
            // 
            // Date
            // 
            this.Date.HeaderText = "日期";
            this.Date.Name = "Date";
            this.Date.ReadOnly = true;
            this.Date.Width = 150;
            // 
            // LogFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(638, 454);
            this.Controls.Add(this.dataLogs);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "LogFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "历史记录";
            this.Load += new System.EventHandler(this.LogFrm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataLogs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataLogs;
        private System.Windows.Forms.DataGridViewTextBoxColumn Temp;
        private System.Windows.Forms.DataGridViewTextBoxColumn Hum;
        private System.Windows.Forms.DataGridViewTextBoxColumn NH3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Light;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date;
    }
}