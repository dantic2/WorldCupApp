using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Storage
{
    public class Favourites
    {
        public string FavouriteTeamFifaCode { get; set; }

        //saving player keys (from Player.Key) for favourite players
        public List<string> FavouritePlayerKeys { get; set; } = new List<string>();

        //HELPERS

        public bool HasTeam => !string.IsNullOrWhiteSpace(FavouriteTeamFifaCode);

        public List<string> NormalizedPlayerKeys()
        {
            return (FavouritePlayerKeys ?? new List<string>())
                .Where(k => !string.IsNullOrWhiteSpace(k))
                .Select(k => k.Trim())
                .Distinct()
                .ToList();
        }

        public override string ToString()
        {
            return $"Team={FavouriteTeamFifaCode}, Players={NormalizedPlayerKeys().Count}";
        }

        public override bool Equals(object obj) => Equals(obj as Favourites);

        public bool Equals(Favourites other)
        {
            if (other is null) return false;

            if (!string.Equals(FavouriteTeamFifaCode, other.FavouriteTeamFifaCode, StringComparison.OrdinalIgnoreCase))
                return false;

            var a = NormalizedPlayerKeys();
            var b = other.NormalizedPlayerKeys();

            if (a.Count != b.Count) return false;

            return !a.Except(b, StringComparer.OrdinalIgnoreCase).Any();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (FavouriteTeamFifaCode?.ToUpperInvariant().GetHashCode() ?? 0);
                foreach (var k in NormalizedPlayerKeys())
                    hash = hash * 23 + k.ToUpperInvariant().GetHashCode();
                return hash;
            }
        }



    }
}
