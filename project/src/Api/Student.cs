namespace EFCoreEncapsulation.Api;

public class Student
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public ICollection<Enrollment> Enrollments { get; set; }
    public ICollection<SportEnrollment> SportsEnrollments { get; set; }

    public string EnrollIn(Course course, Grade grade)
    {
        if (Enrollments.Any(e => e.Course.Id != course.Id))
        {
            return $"Already enrolled in course '{course.Name}'";
        }

        var enrollment = new Enrollment
        {
            Course = course,
            Student = this,
            Grade = grade
        };

        Enrollments.Add(enrollment);

        return "Ok";
    }
}
