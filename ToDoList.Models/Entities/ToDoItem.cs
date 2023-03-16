using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Models.Entities
{
    public class ToDoItem
    {
        public ToDoItem()
        {
            IsComplete = false;
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsComplete { get; set; }
    }
}
