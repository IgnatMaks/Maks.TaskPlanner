using Maks.TaskPlanner.DataAccess.Abstractions;
using Maks.TaskPlanner.Domain.Models;
using Newtonsoft.Json;

namespace Maks.TaskPlanner.DataAccess;

public class FileWorkItemsRepository : IWorkItemsRepository
{
    private const string FileName = "work-items.json";

    private readonly Dictionary<Guid, WorkItem> _workItems;

    public FileWorkItemsRepository()
    {
        if (File.Exists(FileName) && !string.IsNullOrWhiteSpace(File.ReadAllText(FileName)))
        {
            string json = File.ReadAllText(FileName);

            WorkItem[]? items = JsonConvert.DeserializeObject<WorkItem[]>(json);

            _workItems = items?
                .ToDictionary(item => item.Id, item => item)
                ?? new Dictionary<Guid, WorkItem>();
        }
        else
        {
            _workItems = new Dictionary<Guid, WorkItem>();
        }
    }

    public Guid Add(WorkItem workItem)
    {
        WorkItem newWorkItem = workItem.Clone();

        Guid id = Guid.NewGuid();
        newWorkItem.Id = id;

        _workItems.Add(id, newWorkItem);

        return id;
    }

    public WorkItem Get(Guid id)
    {
        return _workItems[id].Clone();
    }

    public WorkItem[] GetAll()
    {
        return _workItems.Values
            .Select(item => item.Clone())
            .ToArray();
    }

    public bool Update(WorkItem workItem)
    {
        if (!_workItems.ContainsKey(workItem.Id))
        {
            return false;
        }

        _workItems[workItem.Id] = workItem.Clone();

        return true;
    }

    public bool Remove(Guid id)
    {
        return _workItems.Remove(id);
    }

    public void SaveChanges()
    {
        WorkItem[] items = _workItems.Values.ToArray();

        string json = JsonConvert.SerializeObject(
            items,
            Formatting.Indented);

        File.WriteAllText(FileName, json);
    }
}