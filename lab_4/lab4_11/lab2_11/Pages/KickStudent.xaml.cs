using System;
using System.Collections.ObjectModel;
using System.Windows;
using lab2_11.api.Get;
using lab2_11.api.Post;
using lab2_11.Entity;

namespace lab2_11.Pages
{
    public partial class KickStudent : Window
    {
        public ObservableCollection<Student> Students { get; set; }
        public Room Room { get; set; }

        public KickStudent(Room room)
        {
            InitializeComponent();
            Room = room;
            Students = new ObservableCollection<Student>();
            GetStudentsFromServer();

            DataContext = this;
        }

        private async void KickStudent_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as FrameworkElement;
            var student = button?.DataContext as Student;

            if (student == null) return;
            var success = await KickStudentFromRoom.Send(Room.Id, student.Id);
            if (success)
            {
                Close();
            }
        }

        private async void GetStudentsFromServer()
        {
            var (success, students) = await GetStudentsFromRoom.Send(Room.Id);
            if (success)
            {
                Students.Clear();
                foreach (var student in students)
                {
                    Students.Add(student);
                }
            }
        }
    }
}