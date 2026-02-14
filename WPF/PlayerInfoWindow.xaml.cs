using DAL.Models;
using DAL.Storage;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace WPF
{
    public partial class PlayerInfoWindow : Window
    {
        private readonly Player _player;
        private readonly Match _match;

        public PlayerInfoWindow(Player player, Match match)
        {
            InitializeComponent();

            _player = player;
            _match = match;

            btnClose.Click += (s, e) => Close();
            Loaded += PlayerInfoWindow_Loaded;
        }

        private void PlayerInfoWindow_Loaded(object sender, RoutedEventArgs e)
        {
            BindUi();
            PlaySlideInAnimation();
        }

        private void BindUi()
        {
            if (_player == null) return;

            txtName.Text = _player.Name;

            txtMeta.Text = string.Format(
                WPF.Properties.Resources.PlayerInfo_MetaFormat,
                _player.ShirtNumber,
                _player.Position);

            txtCaptain.Text = _player.Captain
                ? WPF.Properties.Resources.PlayerInfo_CaptainYes
                : WPF.Properties.Resources.PlayerInfo_CaptainNo;

            // slika iz shared storage
            var map = App.Context.Storage.LoadPlayerImageMap() ?? new PlayerImageMap();
            var rel = map.Get(_player.PlayerKey);

            string imgPath = null;
            if (!string.IsNullOrWhiteSpace(rel))
                imgPath = System.IO.Path.Combine(App.Context.Storage.StorageDirectory, rel);
            else
                imgPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Images", "NoImage.jpeg");

            try
            {
                var bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.UriSource = new Uri(imgPath, UriKind.Absolute);
                bmp.EndInit();
                imgPlayer.Source = bmp;
            }
            catch
            {
                imgPlayer.Source = null;
            }

            // golovi i zuti kartoni u toj utakmici
            int goals = CountPlayerEvents("goal");
            int yellow = CountPlayerEvents("yellow-card");

            txtGoals.Text = string.Format(
                WPF.Properties.Resources.PlayerInfo_GoalsThisMatchFormat,
                goals);

            txtYellow.Text = string.Format(
                WPF.Properties.Resources.PlayerInfo_YellowThisMatchFormat,
                yellow);
        }


        private int CountPlayerEvents(string type)
        {
            if (_match == null || _player == null)
                return 0;

            return _match.AllEvents()
                .Count(ev =>
                    ev != null &&
                    string.Equals(ev.TypeOfEvent, type, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(ev.Player, _player.Name, StringComparison.OrdinalIgnoreCase));
        }

        private void PlaySlideInAnimation()
        {

            var startTop = Top + 30;
            var endTop = Top;

            Top = startTop;

            var anim = new DoubleAnimation
            {
                From = startTop,
                To = endTop,
                Duration = TimeSpan.FromSeconds(0.3),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };

            BeginAnimation(Window.TopProperty, anim);
        }
    }
}
