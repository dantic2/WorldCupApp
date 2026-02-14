using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL.Models;

namespace WinForms.Controls
{
    public partial class PlayerControl : UserControl
    {

        public Player Player { get; private set; }

        public string PlayerKey { get; private set; }

        public bool IsSelected { get; private set; }

        public PlayerControl()
        {
            InitializeComponent();
        }
       
        public void SetSelected(bool selected)
        {
            IsSelected = selected;
            BackColor = selected ? System.Drawing.Color.LightSkyBlue : System.Drawing.Color.Transparent;
        }


        public void Bind(Player player)
        {
            Player = player; 
            PlayerKey = player?.PlayerKey;

            SetSelected(false);

            if (player == null)
            {
                lbName.Text = string.Empty;
                lbNumber.Text = string.Empty;
                lbPosition.Text = string.Empty;
                lbCaptian.Visible = false;
                return;
            }

            lbName.Text = player.Name;
            lbNumber.Text = $"#{player.ShirtNumber}";
            lbPosition.Text = player.Position;
            lbCaptian.Visible = player.Captain;
        }

        public void BindKey(string playerKey)
        {
            Player = null;
            PlayerKey = playerKey;

            SetSelected(false);

            lbCaptian.Visible = false;
            lbPosition.Text = "";

            lbNumber.Text = "";
            lbName.Text = playerKey ?? "";
        }

        public void SetImage(string imagePath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(imagePath) || !System.IO.File.Exists(imagePath))
                {
                    pbPlayer.ImageLocation = null;
                    pbPlayer.Image = null;
                    return;
                }

                pbPlayer.ImageLocation = imagePath;
            }
            catch
            {
                pbPlayer.ImageLocation = null;
                pbPlayer.Image = null;
            }
        }
    }
}
