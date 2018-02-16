namespace Contoso.Models
{
    public enum Grade
    {
        A, B, C, D, F
    }
    public class Enrollment
    {
        public int EnrollmentId { get; set; }  // this could also be just Id
        public int CourseId { get; set; }
        public int StudentId { get; set; } // an enrollment can only have one student
        public Grade? Grade { get; set; }  // ? means field is nullable
        public Course Course { get; set; } // an enrollment can only have one course
        public Student Student { get; set; }
    }
}