namespace WinForms
{
    partial class MainForm
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
            this.lblTeam = new System.Windows.Forms.Label();
            this.cbTeams = new System.Windows.Forms.ComboBox();
            this.lbFavourites = new System.Windows.Forms.Label();
            this.pnlFavourites = new System.Windows.Forms.Panel();
            this.lblAllPlayers = new System.Windows.Forms.Label();
            this.pnlAllPlayers = new System.Windows.Forms.Panel();
            this.btnRankings = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTeam
            // 
            this.lblTeam.AutoSize = true;
            this.lblTeam.Location = new System.Drawing.Point(29, 21);
            this.lblTeam.Name = "lblTeam";
            this.lblTeam.Size = new System.Drawing.Size(34, 13);
            this.lblTeam.TabIndex = 0;
            this.lblTeam.Text = "Team";
            // 
            // cbTeams
            // 
            this.cbTeams.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTeams.FormattingEnabled = true;
            this.cbTeams.Location = new System.Drawing.Point(32, 37);
            this.cbTeams.Name = "cbTeams";
            this.cbTeams.Size = new System.Drawing.Size(250, 21);
            this.cbTeams.TabIndex = 1;
            this.cbTeams.SelectedIndexChanged += new System.EventHandler(this.cbTeams_SelectedIndexChanged);
            // 
            // lbFavourites
            // 
            this.lbFavourites.AutoSize = true;
            this.lbFavourites.Location = new System.Drawing.Point(296, 62);
            this.lbFavourites.Name = "lbFavourites";
            this.lbFavourites.Size = new System.Drawing.Size(56, 13);
            this.lbFavourites.TabIndex = 2;
            this.lbFavourites.Text = "Favourites";
            // 
            // pnlFavourites
            // 
            this.pnlFavourites.AutoScroll = true;
            this.pnlFavourites.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlFavourites.Location = new System.Drawing.Point(299, 78);
            this.pnlFavourites.Name = "pnlFavourites";
            this.pnlFavourites.Size = new System.Drawing.Size(247, 293);
            this.pnlFavourites.TabIndex = 3;
            // 
            // lblAllPlayers
            // 
            this.lblAllPlayers.AutoSize = true;
            this.lblAllPlayers.Location = new System.Drawing.Point(32, 62);
            this.lblAllPlayers.Name = "lblAllPlayers";
            this.lblAllPlayers.Size = new System.Drawing.Size(54, 13);
            this.lblAllPlayers.TabIndex = 4;
            this.lblAllPlayers.Text = "All players";
            // 
            // pnlAllPlayers
            // 
            this.pnlAllPlayers.AutoScroll = true;
            this.pnlAllPlayers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlAllPlayers.Location = new System.Drawing.Point(35, 79);
            this.pnlAllPlayers.Name = "pnlAllPlayers";
            this.pnlAllPlayers.Size = new System.Drawing.Size(247, 293);
            this.pnlAllPlayers.TabIndex = 5;
            // 
            // btnRankings
            // 
            this.btnRankings.Location = new System.Drawing.Point(314, 35);
            this.btnRankings.Name = "btnRankings";
            this.btnRankings.Size = new System.Drawing.Size(75, 23);
            this.btnRankings.TabIndex = 6;
            this.btnRankings.Text = "Rankings";
            this.btnRankings.UseVisualStyleBackColor = true;
            this.btnRankings.Click += new System.EventHandler(this.btnRankings_Click);
            // 
            // btnSettings
            // 
            this.btnSettings.Location = new System.Drawing.Point(546, 387);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(75, 23);
            this.btnSettings.TabIndex = 7;
            this.btnSettings.Text = "Settings";
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 422);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.btnRankings);
            this.Controls.Add(this.pnlAllPlayers);
            this.Controls.Add(this.lblAllPlayers);
            this.Controls.Add(this.pnlFavourites);
            this.Controls.Add(this.lbFavourites);
            this.Controls.Add(this.cbTeams);
            this.Controls.Add(this.lblTeam);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTeam;
        private System.Windows.Forms.ComboBox cbTeams;
        private System.Windows.Forms.Label lbFavourites;
        private System.Windows.Forms.Panel pnlFavourites;
        private System.Windows.Forms.Label lblAllPlayers;
        private System.Windows.Forms.Panel pnlAllPlayers;
        private System.Windows.Forms.Button btnRankings;
        private System.Windows.Forms.Button btnSettings;
    }
}

