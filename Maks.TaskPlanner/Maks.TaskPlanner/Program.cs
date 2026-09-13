using Maks.TaskPlanner.Domain.Logic;
using Maks.TaskPlanner.Domain.Models;
using Maks.TaskPlanner.Domain.Models.Enums;

namespace Maks.TaskPlanner;

internal static class Program
{
    public static void Main(string[] args)
    {
        var workItems = new List<WorkItem>();

        Console.WriteLine("Task Planner");
        Console.WriteLine("Enter tasks. Leave title empty to finish.");
        Console.WriteLine();

        while (true)
        {
            Console.Write("Title: ");
            string? title = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(title))
            {
                break;
            }

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

            workItems.Add(workItem);

            Console.WriteLine();
        }

        var planner = new SimpleTaskPlanner();

        WorkItem[] plan = planner.CreatePlan(workItems.ToArray());

        Console.WriteLine();
        Console.WriteLine("Sorted plan:");

        foreach (WorkItem item in plan)
        {
            Console.WriteLine(item);
        }
    }
}