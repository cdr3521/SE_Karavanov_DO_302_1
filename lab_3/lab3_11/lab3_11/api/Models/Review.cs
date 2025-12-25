using System.Text.Json.Serialization;

namespace lab3_11.api.Models;

public class Review
{
    public int Id { get; set; }
    public string Text { get; set; }
    public string Date { get; set; }
    public int RoomId { get; set; }

    [JsonIgnore]  // Ігнорує поле Room під час серіалізації
    public Room Room { get; set; }
}
