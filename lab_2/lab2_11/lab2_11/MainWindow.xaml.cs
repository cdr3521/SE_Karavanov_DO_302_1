using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using lab2_11.api.Get;
using lab2_11.api.Post;
using lab2_11.Entity;
using lab2_11.Pages;

namespace lab2_11
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Room> Rooms { get; set; }
        public ObservableCollection<Room> FilteredRooms { get; set; }
        public SearchForm Search { get; set; }
        private RoomSearchService _roomSearchService;

        public MainWindow()
        {
            InitializeComponent();
            Rooms = new ObservableCollection<Room>();
            FilteredRooms = new ObservableCollection<Room>();
            _roomSearchService = new RoomSearchService(new ObservableCollection<Room>());
            Search = new SearchForm();
            GetRoomsFromServer();
            DataContext = this;
        }

        private async void GetRoomsFromServer()
        {
            var (success, rooms) = await GetRooms.Send();

            if (success)
            {
                Rooms.Clear();
                foreach (var room in rooms)
                {
                    Rooms.Add(room);
                    FilteredRooms.Add(room);
                }
                _roomSearchService = new RoomSearchService(Rooms);
            }
        }

        private void SearchRooms()
        {
            var results = _roomSearchService.SearchRooms(Search.SearchText);
            FilteredRooms.Clear();
            foreach (var room in results)
            {
                FilteredRooms.Add(room);
            }
        }


        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchRooms();
        }

        private void CheckIn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var room = ((MenuItem)sender).DataContext as Room;
                if (room == null) return;
                var occupation = new Occupy(room);
                occupation.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during check-in: {ex.Message}");
            }
        }


        private void CheckOut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var room = ((MenuItem)sender).DataContext as Room;
                if (room == null) return;
                var kickStudent = new KickStudent(room);
                kickStudent.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during check-out: {ex.Message}");
            }
        }


        private void EditRoom_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var room = ((MenuItem)sender).DataContext as Room;
                if (room == null) return;
                var getInfo = new GetInfo(room);
                getInfo.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while editing the room: {ex.Message}");
            }
        }


        private void AddRoom_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var addRoom = new AddRoomPage();
                addRoom.Show();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while adding the room: {ex.Message}");
            }
        }


        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Натисніть ПКМ по кнопці для додаткових можливостей");
        }

        private void FineRoom_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var room = ((MenuItem)sender).DataContext as Room;
                if (room == null) return;
                var fine = new FinePage(room);
                fine.Show();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while applying fine: {ex.Message}");
            }
        }
    }
}