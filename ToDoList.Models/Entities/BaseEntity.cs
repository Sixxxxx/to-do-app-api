
namespace ToDoList.Models.Entities
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            CreatedAt = DateTime.Now;
            UpdateAt = DateTime.Now;
            Active = true;
        }

        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}
