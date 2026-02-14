using DAL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF
{
    public partial class MainWindow : Window
    {
        private List<TeamResult> _teams = new List<TeamResult>();
        private TeamResult _selectedTeam;

        private bool _isLoadingTeams;

        private DAL.Models.Match _selectedMatch;
        private List<DAL.Models.Player> _leftStarting = new List<DAL.Models.Player>();
        private List<DAL.Models.Player> _rightStarting = new List<DAL.Models.Player>();


        public MainWindow()
        {
            InitializeComponent();

            ApplyWindowMode();

            btnSettings.Click += BtnSettings_Click;

            Loaded += MainWindow_Loaded;
            cbTeams.SelectionChanged += CbTeams_SelectionChanged;
            cbOpponents.SelectionChanged += CbOpponents_SelectionChanged;

            btnTeamInfo.Click += BtnTeamInfo_Click;
            btnOpponentInfo.Click += BtnOpponentInfo_Click;

            canvasPitch.SizeChanged += (s, e) =>
            {
                if (_leftStarting.Count > 0 || _rightStarting.Count > 0)
                {
                    canvasPitch.Children.Clear();
                    DrawTeamOnHalf(_leftStarting, true);
                    DrawTeamOnHalf(_rightStarting, false);
                }
            };

            this.Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var dlg = new WPF.ConfirmExitWindow
            {
                Owner = this
            };

            var ok = dlg.ShowDialog();
            if (ok != true)
            {
                e.Cancel = true;
            }
        }


        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SettingsWindow
            {
                Owner = this
            };

            if (App.Settings != null)
            {
                dlg.SetInitial(App.Settings); 
            }

            var ok = dlg.ShowDialog();
            if (ok != true || dlg.SelectedSettings == null)
                return;

         

            App.Settings = dlg.SelectedSettings;
            App.Context.Storage.SaveSettings(App.Settings);

            DAL.Services.LocalizationService.ApplyLanguage(App.Settings.Language);

            ApplyWindowMode();

           // _ discard 
            _ = ReloadAfterSettingsChangedAsync();
        }

        private async Task ReloadAfterSettingsChangedAsync()
        {
            try
            {
                await LoadTeamsAsync();            
                ApplyFavouriteTeamSelection();    
                await LoadOpponentsAsync();        
                await LoadPlayersAsync();    
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }



        private void BtnTeamInfo_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedTeam == null) return;
            OpenTeamInfoWindow(_selectedTeam.FifaCode);
        }

        private void BtnOpponentInfo_Click(object sender, RoutedEventArgs e)
        {
            var opponent = cbOpponents.SelectedItem as DAL.Models.Team;
            if (opponent == null) return;
            OpenTeamInfoWindow(opponent.Code);
        }

        private void OpenTeamInfoWindow(string fifaCode)
        {
            var wnd = new TeamInfoWindow(fifaCode);
            wnd.Owner = this;
            wnd.Show();
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadTeamsAsync();
            ApplyFavouriteTeamSelection();
        }

        private async Task LoadTeamsAsync()
        {
            try
            {
                _isLoadingTeams = true;

                cbTeams.IsEnabled = false;
                cbTeams.ItemsSource = null;

                _teams = (await App.Context.WorldCup.GetTeamsAsync(App.Settings.Championship))
                    .OrderBy(t => t.Country)
                    .ToList();

                cbTeams.ItemsSource = _teams;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Load teams failed");
            }
            finally
            {
                cbTeams.IsEnabled = true;
                _isLoadingTeams = false;
            }
        }

        private async Task LoadOpponentsAsync()
        {
            try
            {
                cbOpponents.IsEnabled = false;
                cbOpponents.ItemsSource = null;
                txtScore.Text = "";

                if (_selectedTeam == null) return;

                var opponents = await App.Context.WorldCup.GetOpponentsAsync(App.Settings.Championship, _selectedTeam.FifaCode);

                cbOpponents.ItemsSource = opponents;

                if (opponents.Count == 0)
                    return;

                if (App.Settings.LastOpponentByTeam != null &&
                    App.Settings.LastOpponentByTeam.TryGetValue(_selectedTeam.FifaCode, out var lastOpp) &&
                    !string.IsNullOrWhiteSpace(lastOpp))
                {
                    var idx = opponents.FindIndex(t =>
                        string.Equals(t.Code, lastOpp, StringComparison.OrdinalIgnoreCase));

                    cbOpponents.SelectedIndex = idx >= 0 ? idx : 0;
                }
                else
                {
                    cbOpponents.SelectedIndex = 0;
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Load opponents failed");
            }
            finally
            {
                cbOpponents.IsEnabled = true;
            }
        }

        private void ApplyFavouriteTeamSelection()
        {
            if (_teams == null || _teams.Count == 0)
                return;

            var fav = App.Context.Storage.LoadFavourites();
            var code = fav?.FavouriteTeamFifaCode;

            if (!string.IsNullOrWhiteSpace(code))
            {
                var team = _teams.FirstOrDefault(t =>
                    string.Equals(t.FifaCode, code, StringComparison.OrdinalIgnoreCase));

                if (team != null)
                {
                    cbTeams.SelectedItem = team;
                    return;
                }
            }

            // fallback ako nema favourite ili ga nema u listi
            cbTeams.SelectedIndex = 0;
        }

        private async void CbTeams_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            _selectedTeam = cbTeams.SelectedItem as TeamResult;
            if (_selectedTeam == null) return;

            var fav = App.Context.Storage.LoadFavourites() ?? new DAL.Storage.Favourites();
            fav.FavouriteTeamFifaCode = _selectedTeam.FifaCode;
            App.Context.Storage.SaveFavourites(fav);

            Title = $"WPF - {_selectedTeam.DisplayName}";

            await LoadOpponentsAsync();
            await LoadPlayersAsync();
        }

        private async void CbOpponents_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var opponent = cbOpponents.SelectedItem as DAL.Models.Team;
            if (_selectedTeam == null || opponent == null) return;

            try
            {
                txtScore.Text = "Loading...";

                App.Settings.LastOpponentByTeam[_selectedTeam.FifaCode] = opponent.Code;
                App.Context.Storage.SaveSettings(App.Settings);

                var match = await App.Context.WorldCup.GetMatchBetweenAsync(
                    App.Settings.Championship,
                    _selectedTeam.FifaCode,
                    opponent.Code);

                if (match == null)
                {
                    txtScore.Text = "No match found.";
                    return;
                }

                txtScore.Text = $"{match.HomeTeam.Country} {match.HomeTeam.Goals} : {match.AwayTeam.Goals} {match.AwayTeam.Country}";

                await LoadMatchAndLineupsAsync();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Load match failed");
                txtScore.Text = "";
            }
        }

        private async Task LoadPlayersAsync()
        {
            try
            {
                lbPlayers.ItemsSource = null;

                if (_selectedTeam == null) return;

                var players = await App.Context.WorldCup.GetPlayersForTeamAsync(
                    App.Settings.Championship,
                    _selectedTeam.FifaCode);

                var map = App.Context.Storage.LoadPlayerImageMap() ?? new DAL.Storage.PlayerImageMap();

                var storageDir = App.Context.Storage.StorageDirectory;

                //TODO: POPRAVITI!!
                var defaultImg = System.IO.Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "Storage", "NoImage.jpeg");

                var vms = players
                    .OrderBy(p => p.Name)
                    .Select(p =>
                    {
                        var rel = map.Get(p.PlayerKey);
                        var full = !string.IsNullOrWhiteSpace(rel)
                            ? System.IO.Path.Combine(storageDir, rel)
                            : defaultImg;

                        return new PlayerVm
                        {
                            Name = p.Name,
                            ShirtNumber = p.ShirtNumber,
                            Position = p.Position,
                            CaptainLabel = p.Captain ? "C" : "",
                            ImagePath = full
                        };
                    })
                    .ToList();

                lbPlayers.ItemsSource = vms;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Load players failed");
            }
        }


        private void ApplyWindowMode()
        {
            var settings = App.Settings;
            if (settings == null) return;

            switch (settings.WindowMode)
            {
                case DAL.Storage.WindowMode.Fullscreen:
                    WindowStyle = WindowStyle.None;
                    WindowState = WindowState.Maximized;
                    ResizeMode = ResizeMode.NoResize;
                    break;

                case DAL.Storage.WindowMode.R1280x720:
                    SetFixedSize(1280, 720);
                    break;

                case DAL.Storage.WindowMode.R1600x900:
                    SetFixedSize(1600, 900);
                    break;
                
                case DAL.Storage.WindowMode.R1920x1080:
                    SetFixedSize(1920, 1080);
                    break;

                default:
                    SetFixedSize(1280, 720);
                    break;
            }
        }

        private void SetFixedSize(int width, int height)
        {
            WindowStyle = WindowStyle.SingleBorderWindow;
            ResizeMode = ResizeMode.NoResize;
            WindowState = WindowState.Normal;

            Width = width;
            Height = height;

            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private async Task LoadMatchAndLineupsAsync()
        {
            _selectedMatch = null;
            _leftStarting.Clear();
            _rightStarting.Clear();

            //canvasLeft.Children.Clear();
            //canvasRight.Children.Clear();

            var opponent = cbOpponents.SelectedItem as DAL.Models.Team;
            if (_selectedTeam == null || opponent == null)
                return;

            var match = await App.Context.WorldCup.GetMatchBetweenAsync(
                App.Settings.Championship,
                _selectedTeam.FifaCode,
                opponent.Code);

            _selectedMatch = match;
            if (match == null) return;

            bool selectedIsHome = string.Equals(match.HomeTeam?.Code, _selectedTeam.FifaCode, StringComparison.OrdinalIgnoreCase);

            var home = match.HomeTeamStatistics;
            var away = match.AwayTeamStatistics;

            _leftStarting = (selectedIsHome ? home?.StartingEleven : away?.StartingEleven)?.ToList()
                            ?? new List<DAL.Models.Player>();

            _rightStarting = (selectedIsHome ? away?.StartingEleven : home?.StartingEleven)?.ToList()
                             ?? new List<DAL.Models.Player>();

            RenderLineups();
        }

        private void RenderLineups()
        {
            spLeftLineup.Children.Clear();
            spRightLineup.Children.Clear();

            foreach (var p in _leftStarting)
            {
                spLeftLineup.Children.Add(new System.Windows.Controls.TextBlock
                {
                    Text = $"{p.ShirtNumber} {p.Name} ({p.Position})",
                    Margin = new Thickness(0, 2, 0, 2)
                });
            }

            foreach (var p in _rightStarting)
            {
                spRightLineup.Children.Add(new System.Windows.Controls.TextBlock
                {
                    Text = $"{p.ShirtNumber} {p.Name} ({p.Position})",
                    Margin = new Thickness(0, 2, 0, 2)
                });
            }

            canvasPitch.Children.Clear();

            // Canvas.ActualWidth/Height su dostupni nakon layouta
            Dispatcher.InvokeAsync(() =>
            {
                canvasPitch.Children.Clear();
                DrawTeamOnHalf(_leftStarting, isLeftTeam: true);
                DrawTeamOnHalf(_rightStarting, isLeftTeam: false);
            }, System.Windows.Threading.DispatcherPriority.Loaded);
        }


        private string GetPlayerImagePath(string playerKey)
        {
            var map = App.Context.Storage.LoadPlayerImageMap() ?? new DAL.Storage.PlayerImageMap();
            var rel = map.Get(playerKey);

            if (!string.IsNullOrWhiteSpace(rel))
                return System.IO.Path.Combine(App.Context.Storage.StorageDirectory, rel);

            return System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Resources", "Images", "NoImage.jpeg");
        }

        private static string NormalizePos(string pos)
        {
            if (string.IsNullOrWhiteSpace(pos)) return "";
            return pos.Trim().ToLowerInvariant();
        }

        private void DrawTeamOnHalf(IEnumerable<DAL.Models.Player> starting, bool isLeftTeam)
        {
            var rect = GetPitchRect();
            if (rect == Rect.Empty) return;

            double padX = rect.Width * 0.03;   
            double padY = rect.Height * 0.05; 

            double scale = Math.Min(rect.Width / 1100.0, rect.Height / 420.0);
            scale = Math.Max(0.70, Math.Min(1.0, scale));

            double leftBound = rect.Left + padX;
            double rightBound = rect.Right - padX;
            double top = rect.Top + padY;
            double bottom = rect.Bottom - padY;

            double innerW = rightBound - leftBound;
            double innerH = bottom - top;

            double halfW = innerW / 2.0;

            // X zone po poziciji
            double xGoalie = isLeftTeam ? leftBound + halfW * 0.12 : leftBound + halfW + halfW * 0.88;
            double xDef = isLeftTeam ? leftBound + halfW * 0.30 : leftBound + halfW + halfW * 0.70;
            double xMid = isLeftTeam ? leftBound + halfW * 0.52 : leftBound + halfW + halfW * 0.48;
            double xFor = isLeftTeam ? leftBound + halfW * 0.76 : leftBound + halfW + halfW * 0.24;

            var gk = starting.Where(p => NormalizePos(p.Position) == "goalie").ToList();
            var df = starting.Where(p => NormalizePos(p.Position) == "defender").ToList();
            var mf = starting.Where(p => NormalizePos(p.Position) == "midfield").ToList();
            var fw = starting.Where(p => NormalizePos(p.Position) == "forward").ToList();

            PlaceLine(gk, xGoalie);
            PlaceLine(df, xDef);
            PlaceLine(mf, xMid);
            PlaceLine(fw, xFor);

            void PlaceLine(List<DAL.Models.Player> players, double x)
            {
                if (players.Count == 0) return;

                for (int i = 0; i < players.Count; i++)
                {
                    var p = players[i];

                    var card = new WPF.PlayerCard();
                    var imgPath = GetPlayerImagePath(p.PlayerKey);
                    card.Bind(p.PlayerKey, p.Name, p.ShirtNumber, imgPath);
                    card.MouseLeftButtonUp += (s, e) => OpenPlayerInfo(p);

                    card.LayoutTransform = new ScaleTransform(scale, scale);

                    double cardW = card.Width * scale;
                    double cardH = card.Height * scale;

                    double baseY = top + (i + 1) * (innerH / (players.Count + 1)) - (cardH / 2);

                    double stagger = (i % 2 == 0) ? -cardH * 0.12 : cardH * 0.12;

                    double y = baseY + stagger;
                    double left = x - (cardW / 2);

                    Canvas.SetLeft(card, left);
                    Canvas.SetTop(card, y);

                    canvasPitch.Children.Add(card);
                }
            }
        }

        private void OpenPlayerInfo(DAL.Models.Player player)
        {
            if (_selectedMatch == null || player == null) return;

            var wnd = new PlayerInfoWindow(player, _selectedMatch);
            wnd.Owner = this;
            wnd.Show();
        }



        private Rect GetPitchRect()
        {
            double w = canvasPitch.ActualWidth;
            double h = canvasPitch.ActualHeight;

            if (w <= 0 || h <= 0)
                return Rect.Empty;

            // omjer  slike pitch.jpg
            double pitchAspect = 1536.0 / 864.0;

            // container aspect
            double containerAspect = w / h;

            double drawW, drawH, x, y;

            if (containerAspect > pitchAspect)
            {
                drawH = h;
                drawW = h * pitchAspect;
                x = (w - drawW) / 2.0;
                y = 0;
            }
            else
            {
                drawW = w;
                drawH = w / pitchAspect;
                x = 0;
                y = (h - drawH) / 2.0;
            }

            return new Rect(x, y, drawW, drawH);
        }



        private class PlayerVm
        {
            public string Name { get; set; }
            public int ShirtNumber { get; set; }
            public string Position { get; set; }
            public string CaptainLabel { get; set; }
            public string ImagePath { get; set; } //full path
        }
    }
}
