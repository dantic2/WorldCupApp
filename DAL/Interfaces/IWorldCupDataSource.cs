using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{

    public enum Championship
    {
        Men,
        Women
    }
        
    public interface IWorldCupDataSource
    {
        Task<string> GetTeamsResultsJsonAsync(Championship championship);
        Task<string> GetMatchesJsonAsync(Championship championship);
    }
}
