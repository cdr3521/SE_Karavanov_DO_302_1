using System.Collections.ObjectModel;
using System.Windows;
using lab2_11.api.Get;
using lab2_11.api.Post;
using lab2_11.Entity;

namespace lab2_11.Pages;

public partial class GetInfo : Window
{
    public ObservableCollection<Review> Reviews { get; set; }
    public Room Room { get; set; }

    public GetInfo(Room room)
    {
        InitializeComponent();
        Reviews = new ObservableCollection<Review>();
        Room = room;
        GetReviewsFromServer();

        DataContext = this;
    }

    private async void GetReviewsFromServer()
    {
        try
        {
            var (success, reviews) = await GetReviews.Send(Room.Id);
            if (success) 
            {
                Reviews.Clear();
                foreach (var review in reviews)
                {
                    Reviews.Add(review);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error fetching reviews from server: {ex.Message}");
            Reviews = new ObservableCollection<Review>();
        }
    }

    private async void SaveChangesButton_Click(object sender, RoutedEventArgs e)
    {
        var success = await RoomUpdateService.Send(Room);
        if (success)
        {
            Close();
        }
    }
}