using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Data.Configurations
{
    public class TeamPlayerEntityTypeConfiguration : IEntityTypeConfiguration<TeamPlayer>
    {
        public void Configure(EntityTypeBuilder<TeamPlayer> builder)
        {
            builder.HasKey(e => new { e.TeamId, e.PlayerId });
        }
    }
}
