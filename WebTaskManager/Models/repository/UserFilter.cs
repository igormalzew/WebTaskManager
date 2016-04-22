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
}