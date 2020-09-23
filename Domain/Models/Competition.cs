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
        [Key]
        public int Id { get; set; }

        public int IdService { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string AreaName { get; set; }
        public ICollection<Team> Teams { get; set; }
    }
}