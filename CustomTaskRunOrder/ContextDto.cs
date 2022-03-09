using System.Collections.Concurrent;

namespace CustomTaskRunOrder;

public class ContextDto
{
    /// <summary>
    /// 單位：ms
    /// </summary>
    public int DelayTime { get; set; }

    public ConcurrentBag<string> StringResult { get; set; }
}
