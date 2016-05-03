using System;

namespace WebTaskManager.Models.repository
{
    public class UserFilter
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public bool IsPerformanceFilter { get; set; }
        public int[] PriorityFilter { get; set; }
        public int[] CategoryFilter { get; set; }
    }

    public class TaskData
    {
        public int TaskId { get; set; }
        public string Name { get; set; }
        public string Descriptyon { get; set; }
        public int Priority { get; set; }
        public int[] TaskCategory { get; set; }
        public string TaskDate { get; set; }
        public int SpendTime { get; set; }
    }
}