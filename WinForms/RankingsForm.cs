using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL;
using DAL.Interfaces;
using DAL.Services;
using DAL.Storage;
using System.IO;


namespace WinForms
{
    public partial class RankingsForm : Form
    {

        private readonly DAL.Services.AppContext _context;
        private readonly DAL.Storage.AppSettings _settings;
        private readonly string _fifaCode;

        public RankingsForm(DAL.Services.AppContext context, AppSettings settings, string fifaCode)
        {
            InitializeComponent();
            ApplyLocalization();

            _context = context ?? throw new ArgumentNullException(nameof(context));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _fifaCode = fifaCode;

            this.Load += RankingsForm_Load;
        }

        private void ApplyLocalization()
        {
            Text = Properties.Resources.Rankings_Title;

            btnPrintPdf.Text = Properties.Resources.Rankings_ExportPdf;

            tabGoals.Text = Properties.Resources.Rankings_Goals;
            tabYellow.Text = Properties.Resources.Rankings_YellowCards;
            tabAttendance.Text = Properties.Resources.Rankings_Attendance;
        }

        private async void RankingsForm_Load(object sender, EventArgs e)
        {
            try
            {

                ApplyLocalization();

                var goals = await _context.WorldCup.GetGoalsRankingAsync(_settings.Championship, _fifaCode);
                var yellow = await _context.WorldCup.GetYellowCardsRankingAsync(_settings.Championship, _fifaCode);
                var attendance = await _context.WorldCup.GetAttendanceRankingAsync(_settings.Championship, _fifaCode);

                dgvGoals.DataSource = goals
                    .Select(x => new { Player = x.PlayerName, Goals = x.Goals })
                    .ToList();

                dgvYellow.DataSource = yellow
                    .Select(x => new { Player = x.PlayerName, YellowCards = x.YellowCards })
                    .ToList();

                dgvAttendance.DataSource = attendance
                    .Select(x => new { x.Location, x.Attendance, x.Home, x.Away, x.Score })
                    .ToList();

                dgvGoals.Columns["Player"].HeaderText = Properties.Resources.Col_Player;
                dgvGoals.Columns["Goals"].HeaderText = Properties.Resources.Col_Goals;

                dgvYellow.Columns["Player"].HeaderText = Properties.Resources.Col_Player;
                dgvYellow.Columns["YellowCards"].HeaderText = Properties.Resources.Col_YellowCards;

                dgvAttendance.Columns["Location"].HeaderText = Properties.Resources.Col_Location;
                dgvAttendance.Columns["Attendance"].HeaderText = Properties.Resources.Col_Attendance;
                dgvAttendance.Columns["Home"].HeaderText = Properties.Resources.Col_Home;
                dgvAttendance.Columns["Away"].HeaderText = Properties.Resources.Col_Away;
                dgvAttendance.Columns["Score"].HeaderText = Properties.Resources.Col_Score;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,
                    string.Format(Properties.Resources.Rankings_LoadFailed_Message, ex.Message),
                    Properties.Resources.Rankings_LoadFailed_Title,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                Close();
            }
        }

        private async void btnPrintPdf_Click(object sender, EventArgs e)
        {
            try
            {
                // ponovo dohvacamo podatke
                var goals = await _context.WorldCup.GetGoalsRankingAsync(_settings.Championship, _fifaCode);
                var yellow = await _context.WorldCup.GetYellowCardsRankingAsync(_settings.Championship, _fifaCode);
                var attendance = await _context.WorldCup.GetAttendanceRankingAsync(_settings.Championship, _fifaCode);

                using (var sfd = new SaveFileDialog())
                {
                    sfd.Filter = Properties.Resources.Pdf_Filter;
                    sfd.FileName = string.Format(Properties.Resources.Pdf_DefaultFileName, _fifaCode);

                    if (sfd.ShowDialog(this) != DialogResult.OK)
                        return;

                    var title = string.Format(Properties.Resources.Pdf_Title, _fifaCode, _settings.Championship);
                    PdfExporter.ExportRankings(sfd.FileName, title, goals, yellow, attendance);

                    MessageBox.Show(this,
                         Properties.Resources.Pdf_Exported_Message,
                         Properties.Resources.Pdf_Exported_Title,
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,
                    string.Format(Properties.Resources.Pdf_ExportFailed_Message, ex.Message),
                    Properties.Resources.Pdf_ExportFailed_Title,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
