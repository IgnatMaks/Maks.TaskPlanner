using Maks.TaskPlanner.DataAccess;
using Maks.TaskPlanner.Domain.Logic;
using Maks.TaskPlanner.Domain.Models;
using Maks.TaskPlanner.Domain.Models.Enums;

namespace Maks.TaskPlanner;

internal static class Program
{
    public static void Main(string[] args)
    {
        var repository = new FileWorkItemsRepository();

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("[A]dd work item");
            Console.WriteLine("[B]uild a plan");
            Console.WriteLine("[M]ark work item as completed");
            Console.WriteLine("[R]emove a work item");
            Console.WriteLine("[Q]uit the app");
            Console.Write("Choose an option: ");

            string? command = Console.ReadLine();

            switch (command?.ToUpper())
            {
                case "A":
                    AddWorkItem(repository);
                    break;

                case "B":
                    BuildPlan(repository);
                    break;

                case "M":
                    MarkAsCompleted(repository);
                    break;

                case "R":
                    RemoveWorkItem(repository);
                    break;

                case "Q":
                    repository.SaveChanges();
                    return;

                default:
                    Console.WriteLine("Unknown command.");
                    break;
            }
        }
    }

    private static void AddWorkItem(FileWorkItemsRepository repository)
    {
        Console.Write("Title: ");
        string title = Console.ReadLine() ?? string.Empty;

        Console.Write("Description: ");
        string description = Console.ReadLine() ?? string.Empty;

        Console.Write("Due date (dd.MM.yyyy): ");
        DateTime dueDate = DateTime.Parse(Console.ReadLine()!);

        Console.Write("Priority (None, Low, Medium, High, Urgent): ");
        Priority priority = Enum.Parse<Priority>(
            Console.ReadLine()!,
            ignoreCase: true);

        Console.Write("Complexity (None, Minutes, Hours, Days, Weeks): ");
        Complexity complexity = Enum.Parse<Complexity>(
            Console.ReadLine()!,
            ignoreCase: true);

        var workItem = new WorkItem
        {
            CreationDate = DateTime.Now,
            DueDate = dueDate,
            Priority = priority,
            Complexity = complexity,
            Title = title,
            Description = description,
            IsCompleted = false
        };

        Guid id = repository.Add(workItem);

        repository.SaveChanges();

        Console.WriteLine($"Work item added. Id: {id}");
    }

    private static void BuildPlan(FileWorkItemsRepository repository)
    {
        var planner = new SimpleTaskPlanner(repository);

        WorkItem[] plan = planner.CreatePlan();

        Console.WriteLine();
        Console.WriteLine("Plan:");

        foreach (WorkItem item in plan)
        {
            Console.WriteLine($"{item.Id} - {item}");
        }
    }

    private static void MarkAsCompleted(FileWorkItemsRepository repository)
    {
        Console.Write("Enter work item Id: ");

        Guid id = Guid.Parse(Console.ReadLine()!);

        WorkItem workItem = repository.Get(id);

        workItem.IsCompleted = true;

        bool updated = repository.Update(workItem);

        if (updated)
        {
            repository.SaveChanges();
            Console.WriteLine("Work item marked as completed.");
        }
        else
        {
            Console.WriteLine("Work item not found.");
        }
    }

    private static void RemoveWorkItem(FileWorkItemsRepository repository)
    {
        Console.Write("Enter work item Id: ");

        Guid id = Guid.Parse(Console.ReadLine()!);

        bool removed = repository.Remove(id);

        if (removed)
        {
            repository.SaveChanges();
            Console.WriteLine("Work item removed.");
        }
        else
        {
            Console.WriteLine("Work item not found.");
        }
    }
}