using System.Collections.ObjectModel;
using lab2_11.Entity;

namespace lab2_11.api.Post;

public class RoomSearchService
{
    private readonly ObservableCollection<Room> _rooms;

    public RoomSearchService(ObservableCollection<Room> rooms)
    {
        _rooms = rooms;
    }

    public ObservableCollection<Room> SearchRooms(string searchQuery)
    {
        // Якщо рядок пошуку порожній — повертаємо всі кімнати
        if (string.IsNullOrWhiteSpace(searchQuery))
        {
            return new ObservableCollection<Room>(_rooms);
        }

        // Фільтруємо кімнати за номером або іншими параметрами (у майбутньому)
        var searchResults = _rooms
            .Where(room =>
            {
                // Пошук за номером кімнати (якщо введено число)
                if (int.TryParse(searchQuery, out int searchNumber))
                {
                    // Повертає ті кімнати, у номері яких є введені цифри
                    return room.Number.ToString().Contains(searchNumber.ToString());
                }

                // Можна розширити: наприклад, пошук за capacity чи іншими параметрами
                return false;
            })
            .ToList();

        return new ObservableCollection<Room>(searchResults);
    }
}
