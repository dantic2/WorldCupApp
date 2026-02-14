using DAL.Interfaces;
using DAL.Storage;
using System.Windows;
using System.Windows.Input;

namespace WPF
{
    public partial class SettingsWindow : Window
    {
        public AppSettings SelectedSettings { get; private set; }

        public SettingsWindow()
        {
            InitializeComponent();

            cbChampionship.ItemsSource = new[]
            {
                Championship.Men,
                Championship.Women
            };

            cbLanguage.ItemsSource = new[]
            {
                AppLanguage.Hr,
                AppLanguage.En
            };

            cbWindowMode.ItemsSource = new[]
            {
                WindowMode.Fullscreen,
                WindowMode.R1280x720,
                WindowMode.R1600x900,
                WindowMode.R1920x1080
            };

            // defaulti
            cbChampionship.SelectedIndex = 0;
            cbLanguage.SelectedIndex = 0;
            cbWindowMode.SelectedIndex = 1;

            btnOk.Click += BtnOk_Click;
            btnCancel.Click += (s, e) => { DialogResult = false; Close(); };
        }

        // poziva MainWindow prije ShowDialog()
        public void SetInitial(AppSettings current)
        {
            if (current == null) return;

            cbChampionship.SelectedItem = current.Championship;
            cbLanguage.SelectedItem = current.Language;
            cbWindowMode.SelectedItem = current.WindowMode;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            // potvrda
            if (MessageBox.Show(
                    this,
                    Properties.Resources.Msg_ConfirmSettingsChange,
                    Properties.Resources.Msg_ConfirmTitle,
                    MessageBoxButton.OKCancel,
                    MessageBoxImage.Question) != MessageBoxResult.OK)
            {
                return;
            }

            SelectedSettings = new AppSettings
            {
                Championship = (Championship)cbChampionship.SelectedItem,
                Language = (AppLanguage)cbLanguage.SelectedItem,
                WindowMode = (WindowMode)cbWindowMode.SelectedItem
            };

            DialogResult = true;
            Close();
        }

        private void SettingsWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                DialogResult = false;
                Close();
                e.Handled = true;
                return;
            }

            if (e.Key == Key.Enter)
            {
                BtnOk_Click(this, new RoutedEventArgs());
                e.Handled = true;
                return;
            }
        }
    }
}
