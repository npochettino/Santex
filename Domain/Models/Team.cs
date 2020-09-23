using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Domain.Models
{
    public class Team
    {
        [Key]
        public int Id { get; set; }

        public int IdService { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Tla { get; set; }
        public string Email { get; set; }
        public string AreaName { get; set; }
        public int CompetitionId { get; set; }
        public ICollection<Player> Players { get; set; }
    }
}
