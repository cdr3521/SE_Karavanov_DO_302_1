using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using lab2_11.Entity;
using Newtonsoft.Json;
using JsonException = System.Text.Json.JsonException;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace lab2_11.api.Post;

public class AddStudentToRoom
{
    private const string ServerAddress = "localhost";
    private const int ServerPort = 5000;
    
    // ІМІТАЦІЯ: завжди успішно “додаємо” студента в кімнату
    public static async Task<bool> Send(string studentName, int roomId)
    {
        // невелика затримка для реалізму
        await Task.Delay(300);

        // імітована відповідь “сервера”
        var responseObject = new ResponseWrapper
        {
            success = true,
            message = $"Student '{studentName}' assigned to room #{roomId} (mocked)."
        };

        // (необов’язково) лог для зручності налагодження
        Console.WriteLine(JsonConvert.SerializeObject(responseObject, Formatting.Indented));

        return responseObject.success; // завжди true
    }
    
    public class ResponseWrapper
    {
        public bool success { get; set; }
        public string message { get; set; }
    }
}