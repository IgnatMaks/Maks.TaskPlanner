using Maks.TaskPlanner.DataAccess.Abstractions;
using Maks.TaskPlanner.Domain.Models;

namespace Maks.TaskPlanner.Domain.Logic;

public class SimpleTaskPlanner
{
    private readonly IWorkItemsRepository _repository;

    public SimpleTaskPlanner(IWorkItemsRepository repository)
    {
        _repository = repository;
    }

    public WorkItem[] CreatePlan()
    {
        WorkItem[] items = _repository.GetAll();

        var itemsAsList = items
            .Where(item => !item.IsCompleted)
            .ToList();

        itemsAsList.Sort(CompareWorkItems);

        return itemsAsList.ToArray();
    }

    private static int CompareWorkItems(
        WorkItem firstItem,
        WorkItem secondItem)
    {
        int priorityComparison =
            secondItem.Priority.CompareTo(firstItem.Priority);

        if (priorityComparison != 0)
        {
            return priorityComparison;
        }

        int dueDateComparison =
            firstItem.DueDate.CompareTo(secondItem.DueDate);

        if (dueDateComparison != 0)
        {
            return dueDateComparison;
        }

        return string.Compare(
            firstItem.Title,
            secondItem.Title,
            StringComparison.OrdinalIgnoreCase);
    }
}