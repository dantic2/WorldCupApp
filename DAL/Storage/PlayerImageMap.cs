using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Storage
{
    public class PlayerImageMap : IEquatable<PlayerImageMap>
    {

        public Dictionary<string, string> Map { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public void Set(string playerKey, string relativeImagePath)
        {
            if (string.IsNullOrWhiteSpace(playerKey))
                return;

            if (string.IsNullOrWhiteSpace(relativeImagePath))
            {
                Map.Remove(playerKey);
                return;
            }

            Map[playerKey] = relativeImagePath.Trim();
        }

        public string Get(string playerKey)
        {
            if (string.IsNullOrWhiteSpace(playerKey))
                return null;

            return Map.TryGetValue(playerKey, out var path) ? path : null;
        }

        public override string ToString()
        {
            return $"Images mapped: {Map?.Count ?? 0}";
        }


        public override bool Equals(object obj) => Equals(obj as PlayerImageMap);

        public bool Equals(PlayerImageMap other)
        {
            if (other is null) return false;

            var a = (Map ?? new Dictionary<string, string>()).OrderBy(k => k.Key, StringComparer.OrdinalIgnoreCase).ToList();
            var b = (other.Map ?? new Dictionary<string, string>()).OrderBy(k => k.Key, StringComparer.OrdinalIgnoreCase).ToList();

            if (a.Count != b.Count) return false;

            for (int i = 0; i < a.Count; i++)
            {
                if (!string.Equals(a[i].Key, b[i].Key, StringComparison.OrdinalIgnoreCase)) return false;
                if (!string.Equals(a[i].Value, b[i].Value, StringComparison.OrdinalIgnoreCase)) return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                foreach (var kv in (Map ?? new Dictionary<string, string>()).OrderBy(k => k.Key, StringComparer.OrdinalIgnoreCase))
                {
                    hash = hash * 23 + kv.Key.ToUpperInvariant().GetHashCode();
                    hash = hash * 23 + (kv.Value?.ToUpperInvariant().GetHashCode() ?? 0);
                }
                return hash;
            }
        }
    }
}
