namespace lab3_11.api.Models;

public class Room
{
    public int Id { get; set; }
    public int Number { get; set; }
    public int Capacity { get; set; }
    public int Fine { get; set; }
    public virtual ICollection<StudentRoom> StudentRooms { get; set; }
    public virtual List<Review> Reviews { get; set; }  // Додаємо virtual для підтримки lazy loading
}