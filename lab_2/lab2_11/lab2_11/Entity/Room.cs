namespace lab2_11.Entity;

public class Room
{
    public int Id { get; set; }
    public int Number { get; set; }
    public int Capacity { get; set; }
    public int Fine { get; set; }

    public Room(int number, int capacity)
    {
        Number = number;
        Capacity = capacity;
        Fine = Random.Shared.Next(0, Number);
    }
    
    public Room() {}
}