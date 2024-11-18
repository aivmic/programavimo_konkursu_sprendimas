using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Org.Ktu.T120B197.DBs.Entities;

namespace Org.Ktu.T120B197.DBs;

public partial class DB : DbContext
{
    private DB(DbContextOptions<DB> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Example>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}



