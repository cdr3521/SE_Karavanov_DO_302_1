namespace lab3_11.api.Models;

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public virtual ICollection<StudentRoom> StudentRooms { get; set; }
}