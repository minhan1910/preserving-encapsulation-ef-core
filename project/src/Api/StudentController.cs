using Microsoft.AspNetCore.Mvc;

namespace EFCoreEncapsulation.Api;

[ApiController]
[Route("students")]
public class StudentController : ControllerBase
{
    private readonly StudentRepository _studentRepository;
    private readonly CourseRepository _courseRepository;

    public StudentController(StudentRepository studentRepository, CourseRepository courseRepository)
    {
        _studentRepository = studentRepository;
        _courseRepository = courseRepository;
    }

    [HttpGet("{id}")]
    public StudentDto Get(long id)
    {
        Student student = _studentRepository.GetByIdSplitQueries(id);

        if (student is null)
            return null;

        return new StudentDto
        {
            StudentId = student.Id,
            Name = student.Name,
            Email = student.Email,
            Enrollments = student.Enrollments.Select(x => new EnrollmentDto
            {
                Course = x.Course.Name,
                Grade  = x.Grade.ToString(),
            }).ToList()
        };
    }

    [HttpPost]
    public string Enroll(long studentId, long courseId, Grade grade)
    {
        var student = _studentRepository.GetById(studentId);
        if (student is null) return null;        

        var course = _courseRepository.GetById(courseId);
        if (course is null) return null;

        var result = student.EnrollIn(course, grade);

        _studentRepository.Save(student);

        return result;
    }
}
