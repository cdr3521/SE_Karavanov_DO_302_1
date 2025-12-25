using lab3_11.api.dto;
using lab3_11.api.Models;
using Microsoft.EntityFrameworkCore;

namespace lab3_11.api.Services;

public class RoomService
{
    private readonly AppDbContext _context;

    public RoomService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(bool success, string message)> CreateRoom(int number, int capacity)
    {
        if (number <= 0)
            return (false, "Room number must be positive");

        if (capacity <= 0)
            return (false, "Room capacity must be positive");

        if (capacity > 8) // Припустимо, що максимальна місткість кімнати - 8 осіб
            return (false, "Room capacity cannot exceed 8 students");

        if (await _context.Rooms.AnyAsync(r => r.Number == number))
            return (false, "Room with this number already exists");

        try
        {
            var room = new Room
            {
                Number = number,
                Capacity = capacity,
                Fine = 0
            };

            _context.Rooms.Add(room);
            var result = await _context.SaveChangesAsync();

            return result > 0
                ? (true, "Room successfully created")
                : (false, "Failed to save room to database");
        }
        catch (Exception ex)
        {
            return (false, $"Room creation failed: {ex.Message}");
        }
    }

    public async Task<(bool success, string message)> AssignStudentToRoom(string studentName, int roomId)
    {
        if (string.IsNullOrWhiteSpace(studentName))
            return (false, "Student name cannot be empty");

        if (studentName.Length > 100)
            return (false, "Student name is too long");

        if (roomId <= 0)
            return (false, "Invalid room ID");

        try
        {
            var room = await _context.Rooms
                .Include(r => r.StudentRooms)
                .FirstOrDefaultAsync(r => r.Id == roomId);

            if (room == null)
                return (false, "Room not found");

            if (room.StudentRooms.Count >= room.Capacity)
                return (false, "Room is at full capacity");

            // Перевіряємо чи існує студент з таким іменем
            var existingStudent = await _context.Students
                .FirstOrDefaultAsync(s => s.Name == studentName);

            if (existingStudent != null)
                return (false, "Student with this name already exists");

            // Створюємо нового студента
            var student = new Student
            {
                Name = studentName
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            // Перевіряємо чи студент вже не призначений до будь-якої кімнати
            if (await _context.StudentRooms.AnyAsync(sr => sr.StudentId == student.Id))
                return (false, "Student is already assigned to another room");

            var assignment = new StudentRoom
            {
                StudentId = student.Id,
                RoomId = roomId,
                AssignmentDate = DateTime.UtcNow
            };

            _context.StudentRooms.Add(assignment);
            await _context.SaveChangesAsync();

            return (true, $"Student {studentName} successfully created and assigned to room");
        }
        catch (Exception ex)
        {
            return (false, $"Failed to assign student: {ex.Message}");
        }
    }

    public async Task<List<StudentDto>> GetStudentsInRoom(int roomId)
    {
        return await _context.StudentRooms
            .Where(sr => sr.RoomId == roomId)
            .Select(sr => new StudentDto
            {
                Id = sr.Student.Id,
                Name = sr.Student.Name
            })
            .ToListAsync();
    }

    public async Task<(bool success, string message)> AddFineToRoom(int roomId, int fineAmount)
    {
        if (roomId <= 0)
            return (false, "Invalid room ID");

        if (fineAmount <= 0)
            return (false, "Fine amount must be positive");

        if (fineAmount > 10000) // Припустиме обмеження максимального штрафу
            return (false, "Fine amount exceeds maximum allowed value");

        try
        {
            var room = await _context.Rooms.FindAsync(roomId);

            if (room == null)
                return (false, "Room not found");

            if (room.Fine + fineAmount > 50000) // Припустиме обмеження на загальну суму штрафів
                return (false, "Total fine amount would exceed maximum allowed value");

            // Можливо, варто перевірити чи є студенти в кімнаті
            var hasStudents = await _context.StudentRooms.AnyAsync(sr => sr.RoomId == roomId);
            if (!hasStudents)
                return (false, "Cannot add fine to empty room");

            room.Fine += fineAmount;

            await _context.SaveChangesAsync();

            return (true, $"Fine of {fineAmount} successfully added to room {room.Number}. Total fine: {room.Fine}");
        }
        catch (Exception ex)
        {
            return (false, $"Failed to add fine: {ex.Message}");
        }
    }

    public async Task<List<RoomDto>> GetAllRooms()
    {
        return await _context.Rooms
            .Select(r => new RoomDto
            {
                Id = r.Id,
                Number = r.Number,
                Capacity = r.Capacity,
                Fine = r.Fine
            })
            .ToListAsync();
    }

    public async Task<(bool success, string message)> RemoveStudentFromRoom(int studentId, int roomId)
    {
        try
        {
            var assignment = await _context.StudentRooms
                .FirstOrDefaultAsync(sr => sr.StudentId == studentId && sr.RoomId == roomId);

            if (assignment == null)
                return (false, "Student is not assigned to this room");

            _context.StudentRooms.Remove(assignment);
            await _context.SaveChangesAsync();

            var student = await _context.Students.FindAsync(studentId);
            var room = await _context.Rooms.FindAsync(roomId);

            return (true, $"Student {student?.Name} successfully removed from room {room?.Number}");
        }
        catch (Exception ex)
        {
            return (false, $"Failed to remove student from room: {ex.Message}");
        }
    }

    public async Task<(bool success, string message)> UpdateRoom(int roomId, int? newNumber = null,
        int? newCapacity = null)
    {
        if (roomId <= 0)
            return (false, "Invalid room ID");

        if (!newNumber.HasValue && !newCapacity.HasValue)
            return (false, "No updates provided");

        if (newNumber.HasValue)
        {
            if (newNumber.Value <= 0)
                return (false, "Room number must be positive");

            if (newNumber.Value > 1000) // Припустиме обмеження на номер кімнати
                return (false, "Room number exceeds maximum allowed value");
        }

        if (newCapacity.HasValue)
        {
            if (newCapacity.Value <= 0)
                return (false, "Room capacity must be positive");

            if (newCapacity.Value > 8) // Максимальна місткість кімнати
                return (false, "Room capacity cannot exceed 8 students");
        }

        try
        {
            var room = await _context.Rooms
                .Include(r => r.StudentRooms)
                .FirstOrDefaultAsync(r => r.Id == roomId);

            if (room == null)
                return (false, "Room not found");

            // Якщо змінюється номер кімнати
            if (newNumber.HasValue)
            {
                if (room.Number == newNumber.Value)
                    return (false, "New room number is the same as current");

                if (await _context.Rooms.AnyAsync(r => r.Number == newNumber.Value && r.Id != roomId))
                    return (false, "Room with this number already exists");

                room.Number = newNumber.Value;
            }

            // Якщо змінюється місткість
            if (newCapacity.HasValue)
            {
                if (room.Capacity == newCapacity.Value)
                    return (false, "New capacity is the same as current");

                if (newCapacity.Value < room.StudentRooms.Count)
                    return (false,
                        $"Cannot reduce capacity below the current number of students ({room.StudentRooms.Count})");

                room.Capacity = newCapacity.Value;
            }

            await _context.SaveChangesAsync();

            var updates = new List<string>();
            if (newNumber.HasValue) updates.Add($"number: {newNumber.Value}");
            if (newCapacity.HasValue) updates.Add($"capacity: {newCapacity.Value}");

            return (true, $"Room updated successfully. Changed: {string.Join(", ", updates)}");
        }
        catch (Exception ex)
        {
            return (false, $"Failed to update room: {ex.Message}");
        }
    }

    public async Task<(bool success, string message)> AddReview(int roomId, string text)
    {
        if (roomId <= 0)
            return (false, "Invalid room ID");

        if (string.IsNullOrWhiteSpace(text))
            return (false, "Review text cannot be empty");

        if (text.Length > 500) // Обмеження на довжину відгуку
            return (false, "Review text is too long (maximum 500 characters)");

        if (text.Length < 10) // Мінімальна довжина відгуку
            return (false, "Review text is too short (minimum 10 characters)");

        try
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null)
                return (false, "Room not found");

            // Перевіряємо чи є студенти в кімнаті
            var hasStudents = await _context.StudentRooms.AnyAsync(sr => sr.RoomId == roomId);
            if (!hasStudents)
                return (false, "Cannot add review for empty room");

            // Перевіряємо чи не було відгуку сьогодні
            var today = DateTime.Now.ToString("dd/MM/yyyy");
            var hasReviewToday = await _context.Reviews
                .AnyAsync(r => r.RoomId == roomId && r.Date == today);

            if (hasReviewToday)
                return (false, "Review for this room already exists today");

            var review = new Review
            {
                RoomId = roomId,
                Text = text.Trim(), // Видаляємо зайві пробіли
                Date = today
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return (true, $"Review added successfully for room {room.Number}");
        }
        catch (Exception ex)
        {
            return (false, $"Failed to add review: {ex.Message}");
        }
    }

    public async Task<List<Review>> GetReviews(int roomId)
    {
        return await _context.Reviews
            .Where(r => r.RoomId == roomId)
            .ToListAsync();
    }
}