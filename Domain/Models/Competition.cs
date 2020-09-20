using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Domain.Models
{
    public class Competition
    {
        //public Competition()
        //{
        //    CompetitionTeam = new HashSet<CompetitionTeam>();
        //}

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string AreaName { get; set; }
        public virtual ICollection<CompetitionTeam> CompetitionTeam { get; set; }

        [NotMapped]
        public virtual ICollection<Team> Teams
        {
            get
            {
                if (CompetitionTeam?.FirstOrDefault()?.Team != null)
                {
                    return CompetitionTeam.Select(ct => ct.Team).ToList();
                }

                return new List<Team>();
            }
        }
    }
}