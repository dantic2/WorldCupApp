using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WPF
{
    public partial class PlayerCard : UserControl
    {

        public string PlayerKey { get; private set; }

        public PlayerCard()
        {
            InitializeComponent();
        }

        public void Bind(string playerKey, string name, int number, string imagePath)
        {
            PlayerKey = playerKey;
            txtName.Text = GetLastName(name);
            ToolTip = name;
            txtNumber.Text = $"#{number}";

            try
            {
                var bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.UriSource = new Uri(imagePath, UriKind.Absolute);
                bmp.EndInit();
                img.Source = bmp;
            }
            catch
            {
                img.Source = null;
            }
        }

        private static string GetLastName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName)) return "";

            var parts = fullName
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            return parts.Length == 0 ? fullName : parts[parts.Length - 1];
        }
    }
}
