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
    
    public static async Task<bool> Send(Room room)
    {
        var registerModel = new
        {
            command = "createroom",
            number = room.Number.ToString(),
            capacity = room.Capacity.ToString(),
        };

        var request = JsonSerializer.Serialize(registerModel);
        
        var bytesToSend = Encoding.UTF8.GetBytes(request);

        using (var client = new TcpClient(ServerAddress, ServerPort))
        {
            using (var stream = client.GetStream())
            {
                await stream.WriteAsync(bytesToSend, 0, bytesToSend.Length);

                var buffer = new byte[1024];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                dynamic decodedResponse = JsonConvert.DeserializeObject(response);
                Console.WriteLine(decodedResponse);
                
                try
                {
                    var responseObject = JsonSerializer.Deserialize<ResponseWrapper>(response);
                    if (responseObject.success)
                    {
                        return responseObject.success;
                    }
                    
                    MessageBox.Show(responseObject.message);
                    return responseObject.success;
                }
                catch (JsonException ex)
                {
                    MessageBox.Show($"Error parsing response: {ex.Message}");
                    return false;
                }
            }
        }
    }
    
    public class ResponseWrapper
    {
        public bool success { get; set; }
        public string message { get; set; }
    }
}