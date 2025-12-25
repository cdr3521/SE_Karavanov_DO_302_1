using System.Windows;
using lab2_11.api.Post;
using lab2_11.Entity;
using System;

namespace lab2_11.Pages;

public partial class FinePage : Window
{
    public Fine Fine { get; set; }
    public Room Room { get; set; }
    
    public FinePage(Room room)
    {
        InitializeComponent();
        Fine = new Fine(0);
        Room = room;
        DataContext = this;
    }
    
    private async void FineClick(object sender, RoutedEventArgs e)
    {
        try
        {
            var success = await FineService.Send(Fine.Amount, Room.Id);
            if (success)
            {
                MessageBox.Show("Fine successfully processed!");
                var main = new MainWindow();
                main.Show();
                Close();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred: {ex.Message}");
        }
        finally
        {
            Close();
        }
    }
}