using System;
using System.Collections.Generic;

namespace TaskList4CapstoneRevisted.Models
{
    public partial class ToDoList
    {
        public int ToDoListId { get; set; }
        public string TaskDescription { get; set; }
        public DateTime? DueDate { get; set; }
        public bool? Complete { get; set; }
        public string UserId { get; set; }

        public virtual AspNetUsers User { get; set; }
    }
}
