using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForms.Dialogs
{
    public partial class ConfirmCloseForm : Form
    {
        public ConfirmCloseForm()
        {
            InitializeComponent();

            // Enter / Esc
            this.AcceptButton = btnOk;
            this.CancelButton = btnCancel;

            // fixed dialog behaviour
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;

            // texts
            Text = Properties.Resources.Msg_ConfirmExitTitle;
            lblText.Text = Properties.Resources.Msg_ConfirmExitText;

            btnOk.Text = Properties.Resources.Common_Confirm;
            btnCancel.Text = Properties.Resources.Common_Cancel;

            btnOk.Click += (s, e) => { DialogResult = DialogResult.OK; Close(); };
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
        }
    }
}
