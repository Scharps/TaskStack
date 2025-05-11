using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaskStack.Data.Models;

public class TaskEntity
{
    public int Id { get; set; }

    [MaxLength(36)]
    public required string Title { get; set; }

    public required DateTime Created { get; set; }

    public ICollection<TaskStepEntity> Tasks { get; set; } = [];
}