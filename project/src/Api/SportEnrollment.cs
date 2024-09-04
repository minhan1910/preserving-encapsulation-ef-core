namespace EFCoreEncapsulation.Api;

public class SportEnrollment
{
    public long Id { get; set; }
    public Grade Grade { get; set; }
    public Sport Sports { get; set; }
    public Student Student { get; set; }
}
