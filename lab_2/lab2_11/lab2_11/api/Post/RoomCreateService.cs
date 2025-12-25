using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using lab2_11.Entity;
using Newtonsoft.Json;
using JsonException = System.Text.Json.JsonException;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace lab2_11.api.Post;

public class RoomCreateService
{
    private const string ServerAddress = "localhost";
    private const int ServerPort = 5000;
    
    // ІМІТАЦІЯ: завжди успішно "створюємо" кімнату
    public static async Task<bool> Send(Room room)
    {
        // Невелика затримка для реалізму
        await Task.Delay(300);

        // Статична успішна відповідь
        var responseObject = new ResponseWrapper
        {
            success = true,
            message = $"Room #{room.Number} with capacity {room.Capacity} successfully created (mocked)."
        };

        // Лог у консоль для перевірки
        Console.WriteLine(JsonConvert.SerializeObject(responseObject, Formatting.Indented));

        // Завжди успішно повертаємо true
        return responseObject.success;
    }
    
    public class ResponseWrapper
    {
        public bool success { get; set; }
        public string message { get; set; }
    }
}