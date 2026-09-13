using Maks.TaskPlanner.Domain.Models.Enums;

namespace Maks.TaskPlanner.Domain.Models;

public class WorkItem
{
    public DateTime CreationDate { get; set; }

    public DateTime DueDate { get; set; }

    public Priority Priority { get; set; }

    public Complexity Complexity { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool IsCompleted { get; set; }

    public override string ToString()
    {
        return $"{Title}: due {DueDate:dd.MM.yyyy}, {Priority.ToString().ToLower()} priority";
    }
}