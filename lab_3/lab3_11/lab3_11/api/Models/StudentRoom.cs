namespace lab3_11.api.Models;

public class StudentRoom
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int RoomId { get; set; }
    public DateTime AssignmentDate { get; set; }
    
    public virtual Student Student { get; set; }
    public virtual Room Room { get; set; }
}