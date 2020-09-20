using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public partial class CompetitionTeam
    {
        public int CompetitioId { get; set; }
        public int TeamId { get; set; }
        public virtual Competition Competition { get; set; }
        public virtual Team Team { get; set; }
    }
}
