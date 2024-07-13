using System.Windows.Forms;

namespace Appointment_Tracker
{
    partial class MainScreen
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
            this.radioButtonMonth = new System.Windows.Forms.RadioButton();
            this.radioButtonWeek = new System.Windows.Forms.RadioButton();
            this.dataGridViewCalendar = new System.Windows.Forms.DataGridView();
            this.labelAppt = new System.Windows.Forms.Label();
            this.buttonAddAppt = new System.Windows.Forms.Button();
            this.buttonUpdateAppt = new System.Windows.Forms.Button();
            this.buttonDeleteAppt = new System.Windows.Forms.Button();
            this.buttonManage = new System.Windows.Forms.Button();
            this.radioButtonAll = new System.Windows.Forms.RadioButton();
            this.buttonReports = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCalendar)).BeginInit();
            this.SuspendLayout();
            // 
            // radioButtonMonth
            // 
            this.radioButtonMonth.AutoSize = true;
            this.radioButtonMonth.Location = new System.Drawing.Point(163, 50);
            this.radioButtonMonth.Name = "radioButtonMonth";
            this.radioButtonMonth.Size = new System.Drawing.Size(92, 17);
            this.radioButtonMonth.TabIndex = 1;
            this.radioButtonMonth.TabStop = true;
            this.radioButtonMonth.Text = "Current Month";
            this.radioButtonMonth.UseVisualStyleBackColor = true;
            this.radioButtonMonth.CheckedChanged += new System.EventHandler(this.radioButtonMonth_CheckedChanged);
            // 
            // radioButtonWeek
            // 
            this.radioButtonWeek.AutoSize = true;
            this.radioButtonWeek.Location = new System.Drawing.Point(261, 50);
            this.radioButtonWeek.Name = "radioButtonWeek";
            this.radioButtonWeek.Size = new System.Drawing.Size(91, 17);
            this.radioButtonWeek.TabIndex = 2;
            this.radioButtonWeek.TabStop = true;
            this.radioButtonWeek.Text = "Current Week";
            this.radioButtonWeek.UseVisualStyleBackColor = true;
            this.radioButtonWeek.CheckedChanged += new System.EventHandler(this.radioButtonWeek_CheckedChanged);
            // 
            // dataGridViewCalendar
            // 
            this.dataGridViewCalendar.AllowUserToAddRows = false;
            this.dataGridViewCalendar.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCalendar.Location = new System.Drawing.Point(39, 73);
            this.dataGridViewCalendar.Name = "dataGridViewCalendar";
            this.dataGridViewCalendar.RowHeadersVisible = false;
            this.dataGridViewCalendar.Size = new System.Drawing.Size(394, 302);
            this.dataGridViewCalendar.TabIndex = 3;
            this.dataGridViewCalendar.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridViewCalendar_CellFormatting);
            // 
            // labelAppt
            // 
            this.labelAppt.AutoSize = true;
            this.labelAppt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAppt.Location = new System.Drawing.Point(160, 15);
            this.labelAppt.Name = "labelAppt";
            this.labelAppt.Size = new System.Drawing.Size(168, 20);
            this.labelAppt.TabIndex = 6;
            this.labelAppt.Text = "Appointment Calendar";
            // 
            // buttonAddAppt
            // 
            this.buttonAddAppt.Location = new System.Drawing.Point(84, 394);
            this.buttonAddAppt.Name = "buttonAddAppt";
            this.buttonAddAppt.Size = new System.Drawing.Size(100, 25);
            this.buttonAddAppt.TabIndex = 4;
            this.buttonAddAppt.Text = "Add Appt.";
            this.buttonAddAppt.UseVisualStyleBackColor = true;
            this.buttonAddAppt.Click += new System.EventHandler(this.buttonAddAppt_Click);
            // 
            // buttonUpdateAppt
            // 
            this.buttonUpdateAppt.Location = new System.Drawing.Point(192, 394);
            this.buttonUpdateAppt.Name = "buttonUpdateAppt";
            this.buttonUpdateAppt.Size = new System.Drawing.Size(100, 25);
            this.buttonUpdateAppt.TabIndex = 5;
            this.buttonUpdateAppt.Text = "Update Appt.";
            this.buttonUpdateAppt.UseVisualStyleBackColor = true;
            this.buttonUpdateAppt.Click += new System.EventHandler(this.buttonUpdateAppt_Click);
            // 
            // buttonDeleteAppt
            // 
            this.buttonDeleteAppt.Location = new System.Drawing.Point(298, 394);
            this.buttonDeleteAppt.Name = "buttonDeleteAppt";
            this.buttonDeleteAppt.Size = new System.Drawing.Size(100, 25);
            this.buttonDeleteAppt.TabIndex = 7;
            this.buttonDeleteAppt.Text = "Delete Appt.";
            this.buttonDeleteAppt.UseVisualStyleBackColor = true;
            this.buttonDeleteAppt.Click += new System.EventHandler(this.buttonDeleteAppt_Click);
            // 
            // buttonManage
            // 
            this.buttonManage.Location = new System.Drawing.Point(177, 448);
            this.buttonManage.Name = "buttonManage";
            this.buttonManage.Size = new System.Drawing.Size(125, 25);
            this.buttonManage.TabIndex = 7;
            this.buttonManage.Text = "Manage Customers";
            this.buttonManage.UseVisualStyleBackColor = true;
            this.buttonManage.Click += new System.EventHandler(this.buttonManage_Click);
            // 
            // radioButtonAll
            // 
            this.radioButtonAll.AutoSize = true;
            this.radioButtonAll.Location = new System.Drawing.Point(121, 50);
            this.radioButtonAll.Name = "radioButtonAll";
            this.radioButtonAll.Size = new System.Drawing.Size(36, 17);
            this.radioButtonAll.TabIndex = 0;
            this.radioButtonAll.TabStop = true;
            this.radioButtonAll.Text = "All";
            this.radioButtonAll.UseVisualStyleBackColor = true;
            this.radioButtonAll.CheckedChanged += new System.EventHandler(this.radioButtonAll_CheckedChanged);
            // 
            // buttonReports
            // 
            this.buttonReports.Location = new System.Drawing.Point(12, 476);
            this.buttonReports.Name = "buttonReports";
            this.buttonReports.Size = new System.Drawing.Size(75, 23);
            this.buttonReports.TabIndex = 8;
            this.buttonReports.Text = "Reports";
            this.buttonReports.UseVisualStyleBackColor = true;
            this.buttonReports.Click += new System.EventHandler(this.buttonReports_Click);
            // 
            // MainScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(476, 511);
            this.Controls.Add(this.buttonReports);
            this.Controls.Add(this.radioButtonAll);
            this.Controls.Add(this.buttonManage);
            this.Controls.Add(this.buttonDeleteAppt);
            this.Controls.Add(this.buttonUpdateAppt);
            this.Controls.Add(this.buttonAddAppt);
            this.Controls.Add(this.labelAppt);
            this.Controls.Add(this.dataGridViewCalendar);
            this.Controls.Add(this.radioButtonWeek);
            this.Controls.Add(this.radioButtonMonth);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MainScreen";
            this.Text = "Calendar";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCalendar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private RadioButton radioButtonMonth;
        private RadioButton radioButtonWeek;
        private DataGridView dataGridViewCalendar;
        private Label labelAppt;
        private Button buttonAddAppt;
        private Button buttonUpdateAppt;
        private Button buttonDeleteAppt;
        private Button buttonManage;
        private RadioButton radioButtonAll;
        private Button buttonReports;
    }
}