namespace CustomTaskRunOrder;

public class CustomTaskRunService<TContextDto> where TContextDto : ContextDto
{
    /// <summary>
    /// 可執行的 Actions
    /// </summary>
    public Dictionary<string, Func<TContextDto, Task>> Actions { get; set; }

    /// <summary>
    /// string 就是 Actions 的 Key
    /// </summary>
    public List<List<string>> TaskGroups { get; set; }

    /// <summary>
    /// 已加入待執行的 Tasks 總數量
    /// </summary>
    public int TotalTasksCount => TaskGroups.Sum(t => t.Count);

    public async Task Run(TContextDto contextDto, Action perTaskGroupPostAction)
    {
        foreach (var taskGroup in TaskGroups)
        {
            Task[] parallelTasks = taskGroup.Select(c => Actions.GetValueOrDefault(c))
                                            .Where(t => t != null)
                                            .Select(t => t.Invoke(contextDto))
                                            .ToArray();

            await Task.WhenAll(parallelTasks);

            perTaskGroupPostAction.Invoke();
        }
    }
}
