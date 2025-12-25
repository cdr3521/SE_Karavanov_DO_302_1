using System.Windows;
using lab2_11.api.Post;
using lab2_11.Entity;
using System;

namespace lab2_11.Pages;

public partial class Occupy : Window
{
    public Student Student { get; set; }
    public Room Room { get; set; }

    public Occupy(Room room)
    {
        InitializeComponent();
        Student = new Student("");
        Room = room;
        DataContext = Student;
    }

    private async void OccupyClick(object sender, RoutedEventArgs e)
    {
        if (Room.Capacity != 0)
        {
            try
            {
                var success = await AddStudentToRoom.Send(Student.Name, Room.Id);
                if (success)
                {
                    MessageBox.Show("Student successfully added to the room!");
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
        else
        {
            MessageBox.Show("Кімната заповнена");
        }
    }
}