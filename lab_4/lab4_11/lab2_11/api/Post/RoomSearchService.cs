using System.Collections.ObjectModel;
using lab2_11.Entity;

namespace lab2_11.api.Post;

public class RoomSearchService
{
    private ObservableCollection<Room> _rooms;

    public RoomSearchService(ObservableCollection<Room> rooms)
    {
        _rooms = rooms;
    }

    public ObservableCollection<Room> SearchRooms(string searchQuery)
    {
        if (string.IsNullOrWhiteSpace(searchQuery))
        {
            return new ObservableCollection<Room>(_rooms);
        }

        var searchResults = _rooms.Where(room =>
        {
            // Пошук за номером кімнати
            if (int.TryParse(searchQuery, out int searchNumber))
            {
                // Шукаємо кімнати, які містять введені цифри
                return room.Number.ToString().Contains(searchNumber.ToString());
            }
            
            // Можна додати додаткові критерії пошуку тут
            // Наприклад, пошук за статусом кімнати чи іншими параметрами
            return false;
        }).ToList();

        return new ObservableCollection<Room>(searchResults);
    }
}