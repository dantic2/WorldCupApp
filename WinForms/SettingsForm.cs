using DAL.Interfaces;
using DAL.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForms
{
    public partial class SettingsForm : Form
    {
        public AppSettings SelectedSettings { get; private set; }

        public SettingsForm(AppSettings current)
        {
            InitializeComponent();
            this.FormClosing += SettingsForm_FormClosing;
            this.AcceptButton = btnOk;      
            this.CancelButton = btnCancel;


            // postavi radio prema current settingsima
            if (current != null)
            {
                rbMen.Checked = current.Championship == Championship.Men;
                rbWomen.Checked = current.Championship == Championship.Women;

                rbHr.Checked = current.Language == AppLanguage.Hr;
                rbEn.Checked = current.Language == AppLanguage.En;
            }
            else
            {
                rbMen.Checked = true;
                rbHr.Checked = true;
            }

            ApplyLocalization();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            var championship = rbMen.Checked ? Championship.Men : Championship.Women;
            var language = rbHr.Checked ? AppLanguage.Hr : AppLanguage.En;

            SelectedSettings = new AppSettings
            {
                Championship = championship,
                Language = language
            };

            DialogResult = DialogResult.OK;
            Close();
        }

        private void brnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ApplyLocalization()
        {
            var rm = WinForms.Properties.Resources.ResourceManager;
            var c = CultureInfo.CurrentUICulture;

            Text = rm.GetString("Settings_Title", c);

            grbChampionship.Text = rm.GetString("Settings_Championship", c);
            rbMen.Text = rm.GetString("Settings_Men", c);
            rbWomen.Text = rm.GetString("Settings_Women", c);

            grbLanguage.Text = rm.GetString("Settings_Language", c);
            rbHr.Text = rm.GetString("Settings_Hr", c);
            rbEn.Text = rm.GetString("Settings_En", c);

            btnOk.Text = rm.GetString("Common_Ok", c);
            btnCancel.Text = rm.GetString("Common_Cancel", c);
        }

        private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult != DialogResult.OK)
                return;

            var msg = Properties.Resources.Msg_ConfirmSettingsChange;
            var title = Properties.Resources.Msg_ConfirmTitle;

            if (MessageBox.Show(this, msg, title, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
            {
                e.Cancel = true;
                this.DialogResult = DialogResult.None;
            }
        }

    }
}
