namespace WinForms
{
    partial class SettingsForm
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
            this.grbChampionship = new System.Windows.Forms.GroupBox();
            this.rbWomen = new System.Windows.Forms.RadioButton();
            this.rbMen = new System.Windows.Forms.RadioButton();
            this.grbLanguage = new System.Windows.Forms.GroupBox();
            this.rbEn = new System.Windows.Forms.RadioButton();
            this.rbHr = new System.Windows.Forms.RadioButton();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grbChampionship.SuspendLayout();
            this.grbLanguage.SuspendLayout();
            this.SuspendLayout();
            // 
            // grbChampionship
            // 
            this.grbChampionship.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.grbChampionship.Controls.Add(this.rbWomen);
            this.grbChampionship.Controls.Add(this.rbMen);
            this.grbChampionship.Location = new System.Drawing.Point(99, 69);
            this.grbChampionship.Name = "grbChampionship";
            this.grbChampionship.Size = new System.Drawing.Size(197, 75);
            this.grbChampionship.TabIndex = 0;
            this.grbChampionship.TabStop = false;
            this.grbChampionship.Text = "Championship";
            // 
            // rbWomen
            // 
            this.rbWomen.AutoSize = true;
            this.rbWomen.Location = new System.Drawing.Point(20, 52);
            this.rbWomen.Name = "rbWomen";
            this.rbWomen.Size = new System.Drawing.Size(62, 17);
            this.rbWomen.TabIndex = 1;
            this.rbWomen.TabStop = true;
            this.rbWomen.Text = "Women";
            this.rbWomen.UseVisualStyleBackColor = true;
            // 
            // rbMen
            // 
            this.rbMen.AutoSize = true;
            this.rbMen.Location = new System.Drawing.Point(20, 29);
            this.rbMen.Name = "rbMen";
            this.rbMen.Size = new System.Drawing.Size(46, 17);
            this.rbMen.TabIndex = 0;
            this.rbMen.TabStop = true;
            this.rbMen.Text = "Men";
            this.rbMen.UseVisualStyleBackColor = true;
            // 
            // grbLanguage
            // 
            this.grbLanguage.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.grbLanguage.Controls.Add(this.rbEn);
            this.grbLanguage.Controls.Add(this.rbHr);
            this.grbLanguage.Location = new System.Drawing.Point(99, 173);
            this.grbLanguage.Name = "grbLanguage";
            this.grbLanguage.Size = new System.Drawing.Size(200, 68);
            this.grbLanguage.TabIndex = 1;
            this.grbLanguage.TabStop = false;
            this.grbLanguage.Text = "Language";
            // 
            // rbEn
            // 
            this.rbEn.AutoSize = true;
            this.rbEn.Location = new System.Drawing.Point(20, 43);
            this.rbEn.Name = "rbEn";
            this.rbEn.Size = new System.Drawing.Size(59, 17);
            this.rbEn.TabIndex = 1;
            this.rbEn.TabStop = true;
            this.rbEn.Text = "English";
            this.rbEn.UseVisualStyleBackColor = true;
            // 
            // rbHr
            // 
            this.rbHr.AutoSize = true;
            this.rbHr.Location = new System.Drawing.Point(20, 20);
            this.rbHr.Name = "rbHr";
            this.rbHr.Size = new System.Drawing.Size(64, 17);
            this.rbHr.TabIndex = 0;
            this.rbHr.TabStop = true;
            this.rbHr.Text = "Croatian";
            this.rbHr.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnOk.Location = new System.Drawing.Point(99, 259);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "Confirm";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnCancel.Location = new System.Drawing.Point(224, 259);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.brnCancel_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 377);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.grbLanguage);
            this.Controls.Add(this.grbChampionship);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SettingsForm";
            this.grbChampionship.ResumeLayout(false);
            this.grbChampionship.PerformLayout();
            this.grbLanguage.ResumeLayout(false);
            this.grbLanguage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grbChampionship;
        private System.Windows.Forms.RadioButton rbWomen;
        private System.Windows.Forms.RadioButton rbMen;
        private System.Windows.Forms.GroupBox grbLanguage;
        private System.Windows.Forms.RadioButton rbEn;
        private System.Windows.Forms.RadioButton rbHr;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
    }
}