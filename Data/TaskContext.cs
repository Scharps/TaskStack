using System;
using Microsoft.EntityFrameworkCore;
using TaskStack.Data.Models;

namespace TaskStack.Data;

public class TaskContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = string.Join(@"\", "Data Source = " + Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"TaskStack\TaskStack.db");
        optionsBuilder.UseSqlite(connectionString);
    }

    public DbSet<TaskEntity> Tasks { get; set; }
}