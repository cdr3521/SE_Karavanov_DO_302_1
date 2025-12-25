using System.Text;
using System.Windows;
using lab2_11.Entity;
using Newtonsoft.Json;

namespace lab2_11.api.Get
{
    public class GetReviews
    {
        private const string ServerAddress = "localhost";
        private const int ServerPort = 5000;

        // Симуляція запиту
        public static async Task<(bool, List<Review>)> Send(int roomId)
        {
            await Task.Delay(300); // невелика затримка для імітації запиту

            // Статичні дані для симуляції
            var reviews = new List<Review>
            {
                new Review
                {
                    Text = "Гарний номер, чисто і затишно!",
                    Date = "2025-10-01"
                },
                new Review
                {
                    Text = "Все сподобалось, чудовий сервіс.",
                    Date = "2025-09-22"
                },
                new Review
                {
                    Text = "Меблі трохи старі, але все інше на рівні.",
                    Date = "2025-09-10"
                }
            };

            // Завжди успішна відповідь
            bool success = true;

            // Імітація формату відповіді сервера
            var responseWrapper = new ResponseWrapper
            {
                success = success,
                message = "Reviews fetched successfully",
                reviews = reviews
            };

            // (необов’язковий) вивід у консоль для перевірки
            Console.WriteLine(JsonConvert.SerializeObject(responseWrapper, Formatting.Indented));

            return (responseWrapper.success, responseWrapper.reviews);
        }

        public class ResponseWrapper
        {
            public bool success { get; set; }
            public string message { get; set; }
            public List<Review> reviews { get; set; }
        }
    }
}