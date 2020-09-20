using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Data.Configurations
{
    public class CompetitionTeamEntityTypeConfiguration : IEntityTypeConfiguration<CompetitionTeam>
    {
        public void Configure(EntityTypeBuilder<CompetitionTeam> builder)
        {
            builder.HasKey(e => new { e.CompetitionId, e.TeamId });
        }
    }
}