using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using lab2_11.Entity;
using Newtonsoft.Json;
using JsonException = System.Text.Json.JsonException;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace lab2_11.api.Post;

public class KickStudentFromRoom
{
    private const string ServerAddress = "localhost";
    private const int ServerPort = 5000;
    
    // ІМІТАЦІЯ: завжди успішно "виганяємо" студента з кімнати
    public static async Task<bool> Send(int roomId, int studentId)
    {
        // Затримка для симуляції запиту
        await Task.Delay(300);

        // Статична відповідь (успішна)
        var responseObject = new ResponseWrapper
        {
            success = true,
            message = $"Student with ID {studentId} was successfully removed from room #{roomId} (mocked)."
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