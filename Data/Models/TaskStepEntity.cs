using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskStack.Data.Models;

public class TaskStepEntity
{
    public TaskEntity Task { get; set; }
    public int Id { get; set; }
    
    [MaxLength(200)]
    public required string Step { get; set; }
    public required DateTime Created { get; set; }
    public DateTime? Completed { get; set; }
}