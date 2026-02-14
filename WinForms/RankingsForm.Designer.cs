namespace WinForms
{
    partial class RankingsForm
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
            this.tabRankings = new System.Windows.Forms.TabControl();
            this.tabGoals = new System.Windows.Forms.TabPage();
            this.tabYellow = new System.Windows.Forms.TabPage();
            this.tabAttendance = new System.Windows.Forms.TabPage();
            this.dgvAttendance = new System.Windows.Forms.DataGridView();
            this.dgvGoals = new System.Windows.Forms.DataGridView();
            this.dgvYellow = new System.Windows.Forms.DataGridView();
            this.btnPrintPdf = new System.Windows.Forms.Button();
            this.tabRankings.SuspendLayout();
            this.tabGoals.SuspendLayout();
            this.tabYellow.SuspendLayout();
            this.tabAttendance.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAttendance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGoals)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvYellow)).BeginInit();
            this.SuspendLayout();
            // 
            // tabRankings
            // 
            this.tabRankings.Controls.Add(this.tabGoals);
            this.tabRankings.Controls.Add(this.tabYellow);
            this.tabRankings.Controls.Add(this.tabAttendance);
            this.tabRankings.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tabRankings.Location = new System.Drawing.Point(0, 35);
            this.tabRankings.Name = "tabRankings";
            this.tabRankings.SelectedIndex = 0;
            this.tabRankings.Size = new System.Drawing.Size(520, 415);
            this.tabRankings.TabIndex = 0;
            // 
            // tabGoals
            // 
            this.tabGoals.Controls.Add(this.dgvGoals);
            this.tabGoals.Location = new System.Drawing.Point(4, 22);
            this.tabGoals.Name = "tabGoals";
            this.tabGoals.Padding = new System.Windows.Forms.Padding(3);
            this.tabGoals.Size = new System.Drawing.Size(512, 424);
            this.tabGoals.TabIndex = 0;
            this.tabGoals.Text = "Goals";
            this.tabGoals.UseVisualStyleBackColor = true;
            // 
            // tabYellow
            // 
            this.tabYellow.Controls.Add(this.dgvYellow);
            this.tabYellow.Location = new System.Drawing.Point(4, 22);
            this.tabYellow.Name = "tabYellow";
            this.tabYellow.Padding = new System.Windows.Forms.Padding(3);
            this.tabYellow.Size = new System.Drawing.Size(512, 424);
            this.tabYellow.TabIndex = 1;
            this.tabYellow.Text = "Yellow Cards";
            this.tabYellow.UseVisualStyleBackColor = true;
            // 
            // tabAttendance
            // 
            this.tabAttendance.Controls.Add(this.dgvAttendance);
            this.tabAttendance.Location = new System.Drawing.Point(4, 22);
            this.tabAttendance.Name = "tabAttendance";
            this.tabAttendance.Padding = new System.Windows.Forms.Padding(3);
            this.tabAttendance.Size = new System.Drawing.Size(512, 389);
            this.tabAttendance.TabIndex = 2;
            this.tabAttendance.Text = "Attendance";
            this.tabAttendance.UseVisualStyleBackColor = true;
            // 
            // dgvAttendance
            // 
            this.dgvAttendance.AllowUserToAddRows = false;
            this.dgvAttendance.AllowUserToDeleteRows = false;
            this.dgvAttendance.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAttendance.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAttendance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAttendance.Location = new System.Drawing.Point(3, 3);
            this.dgvAttendance.Name = "dgvAttendance";
            this.dgvAttendance.ReadOnly = true;
            this.dgvAttendance.Size = new System.Drawing.Size(506, 383);
            this.dgvAttendance.TabIndex = 0;
            // 
            // dgvGoals
            // 
            this.dgvGoals.AllowUserToAddRows = false;
            this.dgvGoals.AllowUserToDeleteRows = false;
            this.dgvGoals.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvGoals.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGoals.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvGoals.Location = new System.Drawing.Point(3, 3);
            this.dgvGoals.Name = "dgvGoals";
            this.dgvGoals.ReadOnly = true;
            this.dgvGoals.Size = new System.Drawing.Size(506, 418);
            this.dgvGoals.TabIndex = 0;
            // 
            // dgvYellow
            // 
            this.dgvYellow.AllowUserToAddRows = false;
            this.dgvYellow.AllowUserToDeleteRows = false;
            this.dgvYellow.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvYellow.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvYellow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvYellow.Location = new System.Drawing.Point(3, 3);
            this.dgvYellow.Name = "dgvYellow";
            this.dgvYellow.ReadOnly = true;
            this.dgvYellow.Size = new System.Drawing.Size(506, 418);
            this.dgvYellow.TabIndex = 0;
            // 
            // btnPrintPdf
            // 
            this.btnPrintPdf.Location = new System.Drawing.Point(7, 6);
            this.btnPrintPdf.Name = "btnPrintPdf";
            this.btnPrintPdf.Size = new System.Drawing.Size(75, 23);
            this.btnPrintPdf.TabIndex = 1;
            this.btnPrintPdf.Text = "Print to PDF";
            this.btnPrintPdf.UseVisualStyleBackColor = true;
            this.btnPrintPdf.Click += new System.EventHandler(this.btnPrintPdf_Click);
            // 
            // RankingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 450);
            this.Controls.Add(this.btnPrintPdf);
            this.Controls.Add(this.tabRankings);
            this.Name = "RankingsForm";
            this.Text = "RankingsForm";
            this.tabRankings.ResumeLayout(false);
            this.tabGoals.ResumeLayout(false);
            this.tabYellow.ResumeLayout(false);
            this.tabAttendance.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAttendance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGoals)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvYellow)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabRankings;
        private System.Windows.Forms.TabPage tabGoals;
        private System.Windows.Forms.TabPage tabYellow;
        private System.Windows.Forms.TabPage tabAttendance;
        private System.Windows.Forms.DataGridView dgvAttendance;
        private System.Windows.Forms.DataGridView dgvGoals;
        private System.Windows.Forms.DataGridView dgvYellow;
        private System.Windows.Forms.Button btnPrintPdf;
    }
}