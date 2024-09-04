using Microsoft.EntityFrameworkCore;

namespace EFCoreEncapsulation.Api;

public class StudentRepository : Repository<Student>
{
    private readonly SchoolContext _context;

    public StudentRepository(SchoolContext context)
        : base(context)
    {
        _context = context;
    }

    public Student GetByIdSplitQueries(long id)
    {
        var student = _context
            .Students
            .Include(s => s.Enrollments)
                .ThenInclude(e => e.Course)
            .Include(s => s.SportsEnrollments)
                .ThenInclude(se => se.Sports)
            .AsSplitQuery()
            .SingleOrDefault(s => s.Id == id);  

        return student;
    }

    public override Student GetById(long id)
    {
        var student = _context.Students.Find(id);

        if (student is null) return null;

        // Explicit Loading
        _context.Entry(student).Collection(s => s.Enrollments).Load();
        _context.Entry(student).Collection(s => s.SportsEnrollments).Load();

        return student;
    }

    public override void Save(Student entity)
    {
        _context.Add(entity);
        _context.Update(entity);
        _context.Attach(entity);
    }
}

public abstract class Repository<T> 
    where T : class
{
    private readonly SchoolContext _context;

    protected Repository(SchoolContext context)
    {
        _context = context;
    }

    public virtual T GetById(long id)
    {
        return _context.Set<T>().Find(id);
    }

    public virtual void Save(T entity)
    {
        _context.Add(entity);
    }
}

public class CourseRepository : Repository<Course>
{
    private readonly SchoolContext _context;

    public CourseRepository(SchoolContext context)
        : base(context)
    {
    }


}
