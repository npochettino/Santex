using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Domain.Models
{
    public partial class Team
    {
        //public Team()
        //{
        //    TeamPlayer = new HashSet<TeamPlayer>();
        //}

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Tla { get; set; }
        public string Email { get; set; }
        public string AreaName { get; set; }
        public virtual ICollection<CompetitionTeam> CompetitionTeam { get; set; }
        public virtual ICollection<TeamPlayer> TeamPlayer { get; set; }

        [NotMapped]
        public virtual ICollection<Player> Players
        {
            get
            {
                if (TeamPlayer?.FirstOrDefault()?.Player != null)
                {
                    return TeamPlayer.Select(tp => tp.Player).ToList();
                }

                return new List<Player>();
            }
        }
    }
}
