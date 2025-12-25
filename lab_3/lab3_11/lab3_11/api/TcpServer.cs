using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using lab3_11.api.Services;

public class TcpServer
{
    private readonly RoomService _roomService;
    private readonly ILogger _logger;

    public TcpServer(
        RoomService roomService,
        ILogger<TcpServer> logger)
    {
        _roomService = roomService;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        TcpListener listener = new TcpListener(IPAddress.Any, 5000);
        listener.Start();
        _logger.LogInformation("TCP Server started on port 5000.");

        while (!cancellationToken.IsCancellationRequested)
        {
            var client = await listener.AcceptTcpClientAsync();
            _ = HandleClientAsync(client);
        }
    }

    private async Task HandleClientAsync(TcpClient client)
    {
        _logger.LogInformation("Client connected.");
        using (client)
        using (var stream = client.GetStream())
        {
            var buffer = new byte[1024];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            string request = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            Console.WriteLine(request);

            try
            {
                // Десеріалізація запиту
                var requestData = JsonSerializer.Deserialize<Dictionary<string, string>>(request);
                if (requestData == null)
                {
                    var response = new { message = "Invalid command." };
                    var jsonResponse = JsonSerializer.Serialize(response);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                    return;
                }

                var command = requestData["command"].ToLower();
                if (command == "createroom")
                {
                    var (createSuccess, message) = await _roomService.CreateRoom(
                        int.Parse(requestData["number"]),
                        int.Parse(requestData["capacity"]));
                    var response = new
                    {
                        success = createSuccess,
                        message = message
                    };
                    var jsonResponse = JsonSerializer.Serialize(response);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                }
                else if (command == "getrooms")
                {
                    var rooms = await _roomService.GetAllRooms();
                    var response = new
                    {
                        success = true,
                        message = "Rooms retrieved successfully",
                        rooms = rooms
                    };
                    var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                }
                else if (command == "assignstudent")
                {
                    var (assignSuccess, message) = await _roomService.AssignStudentToRoom(
                        requestData["studentName"],
                        int.Parse(requestData["roomId"]));
                    var response = new
                    {
                        success = assignSuccess,
                        message = message
                    };
                    var jsonResponse = JsonSerializer.Serialize(response);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                }
                    else if (command == "getroomstudents")
                    {
                        var students = await _roomService.GetStudentsInRoom(
                            int.Parse(requestData["roomId"]));
                    var response = new
                    {
                        success = true,
                        message = "Room students retrieved successfully",
                        students = students
                    };
                    var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                }
                else if (command == "addfine")
                {
                    var (success, message) = await _roomService.AddFineToRoom(
                        int.Parse(requestData["roomId"]),
                        int.Parse(requestData["fineAmount"]));
    
                    var response = new
                    {
                        success = success,
                        message = message
                    };
    
                    var jsonResponse = JsonSerializer.Serialize(response);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                }
                else if (command == "removestudent")
                {
                    var (success, message) = await _roomService.RemoveStudentFromRoom(
                        int.Parse(requestData["studentId"]),
                        int.Parse(requestData["roomId"]));
    
                    var response = new
                    {
                        success = success,
                        message = message
                    };
    
                    var jsonResponse = JsonSerializer.Serialize(response);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                }
                else if (command == "updateroom")
                {
                    int roomId = int.Parse(requestData["roomId"]);
                    int? newNumber = requestData.ContainsKey("newNumber") ? int.Parse(requestData["newNumber"]) : (int?)null;
                    int? newCapacity = requestData.ContainsKey("newCapacity") ? int.Parse(requestData["newCapacity"]) : (int?)null;

                    var (updateSuccess, message) = await _roomService.UpdateRoom(roomId, newNumber, newCapacity);

                    var response = new
                    {
                        success = updateSuccess,
                        message = message
                    };

                    var jsonResponse = JsonSerializer.Serialize(response);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                }
                else if (command == "addreview")
                {
                    int roomId = int.Parse(requestData["roomId"]);
                    string text = requestData["text"];

                    var (success, message) = await _roomService.AddReview(roomId, text);

                    var response = new
                    {
                        success = success,
                        message = message
                    };

                    var jsonResponse = JsonSerializer.Serialize(response);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                }
                else if (command == "getreviews")
                {
                    int roomId = int.Parse(requestData["roomId"]);
                    var reviews = await _roomService.GetReviews(roomId);

                    var response = new
                    {
                        success = true,
                        message = "Reviews retrieved successfully",
                        reviews = reviews
                    };

                    var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                }
                else
                {
                    var response = new
                    {
                        success = false,
                        message = "Invalid command."
                    };
                    var jsonResponse = JsonSerializer.Serialize(response);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                }
            }
            
            catch (Exception ex)
            {
                var response = new { success = false, message = ex.Message };
                var jsonResponse = JsonSerializer.Serialize(response);
                await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
            }
        }
    }
}