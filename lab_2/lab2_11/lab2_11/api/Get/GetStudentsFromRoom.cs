using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using lab2_11.Entity;
using Newtonsoft.Json;
using JsonException = System.Text.Json.JsonException;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace lab2_11.api.Get;

public class GetStudentsFromRoom
{
    private const string ServerAddress = "localhost";
    private const int ServerPort = 5000;
    
    public static async Task<(bool, List<Student>)> Send(int roomId)
    {
        // Імітація затримки "запиту до сервера"
        await Task.Delay(300);

        // Статичні тестові дані для різних кімнат
        var allStudents = new Dictionary<int, List<Student>>
        {
            [1] = new List<Student>
            {
                new Student("Іван Іваненко") { Id = 1 },
                new Student("Марія Петренко") { Id = 2 },
            },
            [2] = new List<Student>
            {
                new Student("Олег Коваль") { Id = 3 },
                new Student("Світлана Бондар") { Id = 4 },
                new Student("Дмитро Сидоренко") { Id = 5 },
            },
            [3] = new List<Student>
            {
                new Student("Анна Гутник") { Id = 6 },
            }
        };

        // Якщо кімната існує — повертаємо її студентів, інакше — пустий список
        var students = allStudents.ContainsKey(roomId)
            ? allStudents[roomId]
            : new List<Student>();

        var responseWrapper = new ResponseWrapper
        {
            success = true,
            message = "Students fetched successfully (mocked).",
            students = students
        };

        // (необов’язково) лог для перевірки у консолі
        Console.WriteLine(JsonConvert.SerializeObject(responseWrapper, Formatting.Indented));

        return (responseWrapper.success, responseWrapper.students);
    }
    
    public class ResponseWrapper
    {
        public bool success { get; set; }
        public string message { get; set; }
        public List<Student> students { get; set; }
    }
}