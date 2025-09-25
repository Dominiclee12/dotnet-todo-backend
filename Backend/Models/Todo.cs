using System;

namespace Backend.Models;

public class Todo
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool Completed { get; set; } = false;
    public int UserId { get; set; }
}
