namespace Schedule.Data.Models
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;

    public class ScheduleViewModel
    {
        // Для группы
        public int SelectedGroupId { get; set; }
        public List<SelectListItem> Groups { get; set; }

        // Для преподавателя
        public int SelectedTeacherId { get; set; }
        public List<SelectListItem> Teachers { get; set; }
    }
}
