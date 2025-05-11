using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using TaskStack.Data.Models;

namespace TaskStack.Data;

public class TaskContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var dbFileDirectory = string.Join(@"\", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"TaskStack");
        if (Directory.Exists(dbFileDirectory) == false)
        {
            Directory.CreateDirectory(dbFileDirectory);
        }
        var connectionString = string.Join(@"\", "Data Source =" + dbFileDirectory, "TaskStack.db");
        optionsBuilder.UseSqlite(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TaskEntity>()
            .HasMany(t => t.Tasks)
            .WithOne(s => s.Task);
    }

    public DbSet<TaskEntity> Tasks { get; set; }
}