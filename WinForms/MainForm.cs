using DAL.Models;
using DAL.Services;
using DAL.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinForms.Controls;
using System.IO;
using System.Globalization;
using WinForms.Dialogs;

namespace WinForms
{
    public partial class MainForm : Form
    {
        private readonly DAL.Services.AppContext _context = new DAL.Services.AppContext();
        private AppSettings _settings;

        private List<TeamResult> _teams = new List<TeamResult>();
        private TeamResult _selectedTeam;

        private Favourites _favourites;

        private List<Player> _currentPlayers = new List<Player>();

        private PlayerImageMap _imageMap;
        private string _defaultImagePath;

        private bool _isLoadingTeams;

        public MainForm()
        {
            InitializeComponent();

            this.FormClosing += MainForm_FormClosing;

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // ako se zatvara zbog Windows shutdown / task manager, možeš pustiti
            if (e.CloseReason == CloseReason.WindowsShutDown)
                return;

            using (var dlg = new ConfirmCloseForm())
            {
                var result = dlg.ShowDialog(this);
                if (result != DialogResult.OK)
                {
                    e.Cancel = true;
                }
            }
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            _settings = _context.Storage.LoadSettings();

            //MessageBox.Show(_context.ActiveDataSourceName);

            if (_settings != null)
            {
                DAL.Services.LocalizationService.ApplyLanguage(_settings.Language);
            }

            ApplyLocalization();

            if (_settings == null)
            {
                await ShowSettingsDialog();
            }



            _favourites = _context.Storage.LoadFavourites() ?? new Favourites();

            //debug
            //Text = $"WorldCup - {_settings.Championship} - {_settings.Language}";

            _imageMap = _context.Storage.LoadPlayerImageMap() ?? new PlayerImageMap();

            _defaultImagePath = Path.Combine(
                 _context.Storage.StorageDirectory, "NoImage.jpg");

            await LoadTeamsAsync();

            ApplyFavouriteTeamSelection();

            await LoadPlayersForSelectedTeamAsync();

            pnlAllPlayers.AllowDrop = true;
            pnlFavourites.AllowDrop = true;

            pnlAllPlayers.DragEnter += Panel_DragEnter;
            pnlFavourites.DragEnter += Panel_DragEnter;

            pnlAllPlayers.DragDrop += PnlAllPlayers_DragDrop;
            pnlFavourites.DragDrop += PnlFavourites_DragDrop;
        }

        private void Panel_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(List<string>)))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }


        private void ApplyLocalization()
        {
            Text = Properties.Resources.Main_Title;

            lblTeam.Text = Properties.Resources.Main_Team;
            btnRankings.Text = Properties.Resources.Main_Rankings;

            lblAllPlayers.Text = Properties.Resources.Main_AllPlayers;
            lbFavourites.Text = Properties.Resources.Main_Favourites;
            btnSettings.Text = Properties.Resources.Main_Settings;
        }



        private async Task LoadTeamsAsync()
        {
            try
            {
                _isLoadingTeams = true;

                cbTeams.Enabled = false;

                _teams = (await _context.WorldCup.GetTeamsAsync(_settings.Championship))
                    .OrderBy(t => t.Country)
                    .ToList();

                cbTeams.DataSource = null; // reset
                cbTeams.DisplayMember = nameof(DAL.Models.TeamResult.DisplayName);
                cbTeams.ValueMember = nameof(DAL.Models.TeamResult.FifaCode);
                cbTeams.DataSource = _teams;

                //cbTeams.Items.AddRange(items.ToArray());

                if (cbTeams.Items.Count > 0)
                {
                    cbTeams.SelectedIndex = 0;
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(this,
                    string.Format(Properties.Resources.Msg_LoadTeamsError, ex.Message),
                    Properties.Resources.Msg_Error,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                cbTeams.Enabled = true;
                _isLoadingTeams = false;
            }
        }

        private void ApplyFavouriteTeamSelection()
        {
            if (_favourites == null || string.IsNullOrWhiteSpace(_favourites.FavouriteTeamFifaCode))
                return;

            var idx = _teams.FindIndex(t =>
                string.Equals(t.FifaCode, _favourites.FavouriteTeamFifaCode, StringComparison.OrdinalIgnoreCase));

            if (idx >= 0)
                cbTeams.SelectedIndex = idx;
        }

        private async Task LoadPlayersForSelectedTeamAsync()
        {
            _selectedTeam = cbTeams.SelectedItem as TeamResult;
            if (_selectedTeam == null) return;

            _currentPlayers = await _context.WorldCup.GetPlayersForTeamAsync(_settings.Championship, _selectedTeam.FifaCode);
            RefreshPlayerPanels();
        }


        private async void cbTeams_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isLoadingTeams) return;

            _selectedTeam = cbTeams.SelectedItem as TeamResult;
            if (_selectedTeam == null) return;

            _favourites.FavouriteTeamFifaCode = _selectedTeam.FifaCode;
            _context.Storage.SaveFavourites(_favourites);

            if (_selectedTeam == null) return;


            await LoadPlayersForSelectedTeamAsync();

            //try
            //{
            //    _currentPlayers = await _context.WorldCup.GetPlayersForTeamAsync(_settings.Championship, _selectedTeam.FifaCode);
            //    RefreshPlayerPanels();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(this, $"Error loading players: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}

        }

        private void RefreshPlayerPanels()
        {
            RenderFavouriteKeys();
            RenderAllPlayersPanel();
        }

        private void RenderPlayersInPanel(Panel panel, List<Player> players, bool isFavouritePanel)
        {
            panel.SuspendLayout();
            panel.AutoScrollPosition = new System.Drawing.Point(0, 0);
            panel.Controls.Clear();

            if (players == null || players.Count == 0)
            {
                panel.ResumeLayout();
                return;
            }

            int x = 10;
            int y = 10;
            int gap = 0;

            foreach (var p in players)
            {
                var ctrl = new PlayerControl();
                ctrl.Bind(p);

                var rel = _imageMap.Get(p.PlayerKey);
                var full = !string.IsNullOrWhiteSpace(rel)
                    ? Path.Combine(_context.Storage.StorageDirectory, rel)
                    : _defaultImagePath;

                ctrl.SetImage(full);

                AttachImageContextMenu(ctrl, p.PlayerKey);
                AttachFavouriteContextMenu(ctrl, isInFavouritesPanel: false);


                ctrl.Left = x;
                ctrl.Top = y;

                //ctrl.Click += PlayerControl_Click;

                //foreach (Control c in ctrl.Controls)
                //{
                //    c.Click += (s, e) => PlayerControl_Click(ctrl, EventArgs.Empty);
                //}

                ctrl.Click += PlayerControl_SelectClick;
                foreach (Control c in ctrl.Controls)
                {
                    c.Click += (s, e) => PlayerControl_SelectClick(ctrl, EventArgs.Empty);
                }

                ctrl.MouseDown += PlayerControl_MouseDownStartDrag;
                foreach (Control c in ctrl.Controls)
                    c.MouseDown += (s, e) => PlayerControl_MouseDownStartDrag(ctrl, e);

                panel.Controls.Add(ctrl);

                y += ctrl.Height + gap;
            }

            panel.AutoScrollPosition = new System.Drawing.Point(0, 0);
            panel.VerticalScroll.Value = 0;
            panel.PerformLayout();

            panel.ResumeLayout();
        }

        private void RenderFavouriteKeys()
        {
            pnlFavourites.SuspendLayout();
            pnlFavourites.Controls.Clear();

            int x = 10;
            int y = 10;
            int gap = 8;

            foreach (var key in _favourites.NormalizedPlayerKeys())
            {
                var ctrl = new PlayerControl();
                ctrl.BindKey(key);

                var rel = _imageMap.Get(key);
                var full = !string.IsNullOrWhiteSpace(rel)
                    ? Path.Combine(_context.Storage.StorageDirectory, rel)
                    : _defaultImagePath;

                ctrl.SetImage(full);

                AttachImageContextMenu(ctrl, key);
                AttachFavouriteContextMenu(ctrl, isInFavouritesPanel: true);


                ctrl.Left = x;
                ctrl.Top = y;

                ctrl.Click += PlayerControl_SelectClick;
                foreach (Control c in ctrl.Controls)
                    c.Click += (s, e) => PlayerControl_SelectClick(ctrl, EventArgs.Empty);

                // klik za uklanjanje favorita
                //ctrl.Click += (s, e) => RemoveFavouriteByKey(key);
                //foreach (Control c in ctrl.Controls)
                //    c.Click += (s, e) => RemoveFavouriteByKey(key);

                ctrl.MouseDown += PlayerControl_MouseDownStartDrag;
                foreach (Control c in ctrl.Controls)
                    c.MouseDown += (s, e) => PlayerControl_MouseDownStartDrag(ctrl, e);

                pnlFavourites.Controls.Add(ctrl);
                y += ctrl.Height + gap;
            }

            pnlFavourites.ResumeLayout();
        }

        private void PlayerControl_MouseDownStartDrag(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            if (!(sender is PlayerControl pc))
                return;

            // ako user klikne na neselctirani, selektiraj samo njega
            if (!pc.IsSelected)
            {
                ClearSelections();
                pc.SetSelected(true);
            }

            // skupi selektirane kljuceve
            var keys = GetSelectedPlayerControls()
                .Select(x => x.PlayerKey)
                .Where(k => !string.IsNullOrWhiteSpace(k))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (keys.Count == 0)
                return;

            DoDragDrop(keys, DragDropEffects.Move);
        }

        private void PnlFavourites_DragDrop(object sender, DragEventArgs e)
        {
            var keys = e.Data.GetData(typeof(List<string>)) as List<string>;
            if (keys == null || keys.Count == 0) return;

            var fav = _favourites.NormalizedPlayerKeys();

            foreach (var key in keys)
            {
                if (fav.Any(k => string.Equals(k, key, StringComparison.OrdinalIgnoreCase)))
                    continue;

                if (fav.Count >= 3)
                {
                    MessageBox.Show(this,
                        Properties.Resources.Msg_LimitFav,
                        Properties.Resources.Msg_Error,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    break;
                }

                fav.Add(key);
            }

            _favourites.FavouritePlayerKeys = fav;
            _context.Storage.SaveFavourites(_favourites);

            ClearSelections();
            RefreshPlayerPanels();
        }

        private void PnlAllPlayers_DragDrop(object sender, DragEventArgs e)
        {

            var keys = e.Data.GetData(typeof(List<string>)) as List<string>;
            if (keys == null || keys.Count == 0) return;

            var fav = _favourites.NormalizedPlayerKeys();

            fav = fav
                .Where(k => !keys.Any(x => string.Equals(x, k, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            _favourites.FavouritePlayerKeys = fav;
            _context.Storage.SaveFavourites(_favourites);

            ClearSelections();
            RefreshPlayerPanels();
        }



        private void PlayerControl_SelectClick(object sender, EventArgs e)
        {
            if (!(sender is PlayerControl pc))
                return;

            bool ctrlDown = (ModifierKeys & Keys.Control) == Keys.Control;

            if (!ctrlDown)
            {
                // single select
                ClearSelections();
                pc.SetSelected(true);
            }
            else
            {
                // toggle select
                pc.SetSelected(!pc.IsSelected);
            }
        }


        private void RemoveFavouriteByKey(string key)
        {
            _favourites.FavouritePlayerKeys = _favourites
                .NormalizedPlayerKeys()
                .Where(k => !string.Equals(k, key, StringComparison.OrdinalIgnoreCase))
                .ToList();

            _context.Storage.SaveFavourites(_favourites);

            // refresh oba panela
            RenderFavouriteKeys();
            RenderAllPlayersPanel(); ;
        }

        private void RenderAllPlayersPanel()
        {
            var favKeys = _favourites.NormalizedPlayerKeys();

            var otherPlayers = _currentPlayers
                .Where(p => !favKeys.Contains(p.PlayerKey, StringComparer.OrdinalIgnoreCase))
                .ToList();

            RenderPlayersInPanel(pnlAllPlayers, otherPlayers, false);
        }

        private void PlayerControl_Click(object sender, EventArgs e)
        {
            if (!(sender is PlayerControl pc) || pc.Player == null)
                return;

            var key = pc.Player.PlayerKey;
            var list = _favourites.NormalizedPlayerKeys() ?? new List<string>();

            bool isFav = list.Any(k => string.Equals(k, key, StringComparison.OrdinalIgnoreCase));

            if (!isFav)
            {
                //limit 3
                if (_favourites.NormalizedPlayerKeys().Count >= 3)
                {
                    MessageBox.Show(this,
                        Properties.Resources.Msg_LimitFav,
                        Properties.Resources.Common_Error,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }

                list.Add(key);
                _favourites.FavouritePlayerKeys = list;
            }
            else
            {
                _favourites.FavouritePlayerKeys = list
                    .Where(k => !string.Equals(k, key, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            //save
            _context.Storage.SaveFavourites(_favourites);
            RefreshPlayerPanels();
        }

        private void btnRankings_Click(object sender, EventArgs e)
        {
            if (_selectedTeam == null)
            {
                MessageBox.Show(this,
                        Properties.Resources.Msg_SelectTeamFirst,
                        Properties.Resources.Common_Info,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                return;
            }

            using (var frm = new WinForms.RankingsForm(_context, _settings, _selectedTeam.FifaCode))
            {
                frm.ShowDialog(this);
            }
        }

        private void AttachImageContextMenu(PlayerControl ctrl, string playerKey)
        {
            var menu = new ContextMenuStrip();

            var miChoose = new ToolStripMenuItem(Properties.Resources.Msg_ImageChoose);
            miChoose.Click += (s, e) => ChooseImageForPlayer(playerKey);

            var miClear = new ToolStripMenuItem(Properties.Resources.Ctx_RemoveImage);
            miClear.Click += (s, e) =>
            {
                _imageMap.Set(playerKey, null);
                _context.Storage.SavePlayerImageMap(_imageMap);
                RefreshPlayerPanels();
            };

            menu.Items.Add(miChoose);
            menu.Items.Add(miClear);

            ctrl.ContextMenuStrip = menu;
        }

        private void AttachFavouriteContextMenu(PlayerControl ctrl, bool isInFavouritesPanel)
        {
            var menu = ctrl.ContextMenuStrip ?? new ContextMenuStrip();

            if (!isInFavouritesPanel)
            {
                var miAdd = new ToolStripMenuItem(Properties.Resources.Menu_AddToFavourites);
                miAdd.Click += (s, e) => AddFavouriteByKey(ctrl.PlayerKey);
                menu.Items.Insert(0, miAdd);
            }
            else
            {
                var miRemove = new ToolStripMenuItem(Properties.Resources.Menu_RemoveFromFavourites);
                miRemove.Click += (s, e) => RemoveFavouriteByKey(ctrl.PlayerKey);
                menu.Items.Insert(0, miRemove);
            }

            ctrl.ContextMenuStrip = menu;
        }

        private void AddFavouriteByKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return;

            var fav = _favourites.NormalizedPlayerKeys();

            if (fav.Any(k => string.Equals(k, key, StringComparison.OrdinalIgnoreCase)))
                return;

            if (fav.Count >= 3)
            {
                MessageBox.Show(this,
                    Properties.Resources.Msg_LimitFav,
                    Properties.Resources.Msg_Error,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            fav.Add(key);
            _favourites.FavouritePlayerKeys = fav;
            _context.Storage.SaveFavourites(_favourites);

            RefreshPlayerPanels();
        }


        private void ChooseImageForPlayer(string playerKey)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image files|*.png;*.jpg;*.jpeg;*.bmp;*.gif";
                ofd.Title = "Select player image";

                if (ofd.ShowDialog(this) != DialogResult.OK)
                    return;

                // cilj: u output folderu -> Resources\Images\PlayerImages
                //var destDir = Path.Combine(
                //    AppDomain.CurrentDomain.BaseDirectory,
                //    "Resources",
                //    "Images",
                //    "PlayerImages"
                //);

                var destDir = Path.Combine(_context.Storage.StorageDirectory, "PlayerImages");
                Directory.CreateDirectory(destDir);

                // napravi sigurno ime datoteke
                var ext = Path.GetExtension(ofd.FileName);
                var safeName = MakeSafeFileName(playerKey) + ext;
                var destFullPath = Path.Combine(destDir, safeName);

                File.Copy(ofd.FileName, destFullPath, overwrite: true);

                // spremamo RELATIVNO
                //var relPath = Path.Combine("Resources", "Images", "PlayerImages", safeName);

                var relPath = Path.Combine("PlayerImages", safeName);

                _imageMap.Set(playerKey, relPath);
                _context.Storage.SavePlayerImageMap(_imageMap);

                RefreshPlayerPanels();
            }
        }

        private string MakeSafeFileName(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "player";

            foreach (var c in Path.GetInvalidFileNameChars())
                text = text.Replace(c, '_');

            text = text.Replace(' ', '_');

            return text;
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
             ShowSettingsDialog();
        }

        private async Task ShowSettingsDialog()
        {
            using (var dlg = new SettingsForm(_settings))
            {
                var result = dlg.ShowDialog();

                if (result == DialogResult.OK && dlg.SelectedSettings != null)
                {
                    _settings = dlg.SelectedSettings;
                    _context.Storage.SaveSettings(_settings);

                    DAL.Services.LocalizationService.ApplyLanguage(_settings.Language);
                    ApplyLocalization();

                    await LoadTeamsAsync();
                    ApplyFavouriteTeamSelection();
                    await LoadPlayersForSelectedTeamAsync();
                }
            }
        }

        private void ClearSelections()
        {
            ClearSelectionsInPanel(pnlAllPlayers);
            ClearSelectionsInPanel(pnlFavourites);
        }

        private void ClearSelectionsInPanel(Panel panel)
        {
            foreach (Control c in panel.Controls)
            {
                if (c is PlayerControl pc)
                    pc.SetSelected(false);
            }
        }

        private List<PlayerControl> GetSelectedPlayerControls()
        {
            var selected = new List<PlayerControl>();

            foreach (Control c in pnlAllPlayers.Controls)
                if (c is PlayerControl pc && pc.IsSelected)
                    selected.Add(pc);

            foreach (Control c in pnlFavourites.Controls)
                if (c is PlayerControl pc && pc.IsSelected)
                    selected.Add(pc);

            return selected;
        }
    }
}
