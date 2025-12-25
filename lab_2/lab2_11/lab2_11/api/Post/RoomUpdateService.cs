using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using lab2_11.Entity;
using Newtonsoft.Json;
using JsonException = System.Text.Json.JsonException;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace lab2_11.api.Post;

public class RoomUpdateService
{
    private const string ServerAddress = "localhost";
    private const int ServerPort = 5000;
    
    // ІМІТАЦІЯ: завжди успішно оновлюємо кімнату
    public static async Task<bool> Send(Room room)
    {
        // Невелика затримка для реалізму
        await Task.Delay(300);

        // Статична "успішна" відповідь
        var responseObject = new ResponseWrapper
        {
            success = true,
            message = $"Room ID {room.Id} updated to Number={room.Number}, Capacity={room.Capacity} (mocked)."
        };

        // Логування у консоль для налагодження
        Console.WriteLine(JsonConvert.SerializeObject(responseObject, Formatting.Indented));

        return responseObject.success; // Завжди true
    }
    
    public class ResponseWrapper
    {
        public bool success { get; set; }
        public string message { get; set; }
    }
}