using DAL.Models;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace WPF
{
    public partial class TeamInfoWindow : Window
    {
        private readonly string _fifaCode;

        public TeamInfoWindow(string fifaCode)
        {
            InitializeComponent();
            _fifaCode = fifaCode;

            Loaded += TeamInfoWindow_Loaded;
        }

        private async void TeamInfoWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadSummaryAsync();
            PlayOpenAnimation();
        }

        private async Task LoadSummaryAsync()
        {
            try
            {
                TeamSummary s = await App.Context.WorldCup.GetTeamSummaryAsync(App.Settings.Championship, _fifaCode);
                if (s == null)
                {
                    txtTitle.Text = WPF.Properties.Resources.TeamInfo_NoData;
                    return;
                }

                txtTitle.Text = $"{s.Country} ({s.FifaCode})";

                txtPlayed.Text = string.Format(
                    WPF.Properties.Resources.TeamInfo_PlayedFormat,
                    s.Played);

                txtWdl.Text = string.Format(
                    WPF.Properties.Resources.TeamInfo_WdlFormat,
                    s.Wins, s.Draws, s.Losses);

                txtGoals.Text = string.Format(
                    WPF.Properties.Resources.TeamInfo_GoalsForAgainstFormat,
                    s.GoalsFor, s.GoalsAgainst);

                txtDiff.Text = string.Format(
                    WPF.Properties.Resources.TeamInfo_GoalDiffFormat,
                    s.GoalDifference);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, WPF.Properties.Resources.TeamInfo_FailedTitle);
                Close();
            }
        }

        private void PlayOpenAnimation()
        {
            var anim = new DoubleAnimation
            {
                From = 0.0,
                To = 1.0,
                Duration = TimeSpan.FromSeconds(0.5)
            };

            BeginAnimation(Window.OpacityProperty, anim);
        }
    }
}
