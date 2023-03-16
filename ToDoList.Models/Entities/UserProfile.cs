using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ToDoList.Models.Entities;
using ToDoList.Models.Enums;

namespace ToDoList.Models.Entities;

public class UserProfile : BaseEntity
{
    [Key]
    public long Id { get; set; }
    public string UserId { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public long DepartmentId { get; set; }
    public Gender? GenderId { get; set; }
    public int? StudentTypeId { get; set; }

    [NotMapped]
    public bool Active { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual ApplicationUser User { get; set; }
}