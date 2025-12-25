namespace lab2_11.Entity;

public class Student
{
    public Student(string name)
    {
        Name = name;
    }
    public int Id { get; set; }

    public string Name { get; set; }
}