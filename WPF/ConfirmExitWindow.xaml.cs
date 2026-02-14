using System.Windows;

namespace WPF
{
    public partial class ConfirmExitWindow : Window
    {
        public ConfirmExitWindow()
        {
            InitializeComponent();

            btnOk.Click += (s, e) => { DialogResult = true; Close(); };
            btnCancel.Click += (s, e) => { DialogResult = false; Close(); };
        }
    }
}
