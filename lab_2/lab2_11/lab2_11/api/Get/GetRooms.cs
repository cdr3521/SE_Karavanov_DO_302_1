using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using lab2_11.Entity;
using Newtonsoft.Json;
using JsonException = System.Text.Json.JsonException;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace lab2_11.api.Get;

public class GetRooms
{
    private const string ServerAddress = "localhost";
    private const int ServerPort = 5000;

    // ІМІТАЦІЯ: завжди успішно повертаємо статичний список кімнат
    public static async Task<(bool, List<Room>)> Send()
    {
        // невелика затримка для реалізму
        await Task.Delay(300);

        // статичні дані для симуляції
        var rooms = new List<Room>
        {
            new Room { Id = 1, Number = 101, Capacity = 2, Fine = 0 },
            new Room { Id = 2, Number = 102, Capacity = 3, Fine = 50 },
            new Room { Id = 3, Number = 201, Capacity = 2, Fine = 20 },
            new Room { Id = 4, Number = 202, Capacity = 4, Fine = 0 },
            new Room { Id = 5, Number = 301, Capacity = 1, Fine = 10 }
        };

        var responseWrapper = new ResponseWrapper
        {
            success = true,
            message = "Rooms fetched successfully (mocked).",
            rooms = rooms
        };

        // (необов’язково) виводимо “відповідь сервера” у консоль
        Console.WriteLine(JsonConvert.SerializeObject(responseWrapper, Formatting.Indented));

        return (responseWrapper.success, responseWrapper.rooms);
    }

    public class ResponseWrapper
    {
        public bool success { get; set; }
        public string message { get; set; }
        public List<Room> rooms { get; set; }
    }
}