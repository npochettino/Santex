using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Models
{
    public class Player
    {
        [Key]
        public int Id { get; set; }

        public int IdService { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string CountryOfBirth { get; set; }
        public string Nationality { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
    }
}
