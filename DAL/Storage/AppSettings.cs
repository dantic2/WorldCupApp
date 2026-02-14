using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Storage
{


    public enum AppLanguage
    {
        Hr,
        En
    }

    public enum WindowMode
    {
        Fullscreen,
        R1280x720,
        R1600x900,
        R1920x1080
    }


    public class AppSettings
    {
        public Dictionary<string, string> LastOpponentByTeam { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public Championship Championship { get; set; } = Championship.Men;
        public AppLanguage Language { get; set; } = AppLanguage.Hr;


        public WindowMode WindowMode { get; set; } = WindowMode.R1280x720;
    }
}
