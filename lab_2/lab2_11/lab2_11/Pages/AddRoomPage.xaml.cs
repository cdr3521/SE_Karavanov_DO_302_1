using System.Windows;
using lab2_11.api.Post;
using lab2_11.Entity;
using System;

namespace lab2_11.Pages;

public partial class AddRoomPage : Window
{
    public Room Room { get; set; }

    public AddRoomPage()
    {
        InitializeComponent();
        Room = new Room(0, 0);

        DataContext = this;
    }

    private void Close(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private async void AddRoomClick(object sender, RoutedEventArgs e)
    {
        var success = await RoomCreateService.Send(Room);
        if (success)
        {
            var main = new MainWindow();
            main.Show();
            Close();
            
        }
    }
}