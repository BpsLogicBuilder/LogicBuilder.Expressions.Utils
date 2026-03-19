using System.ComponentModel.DataAnnotations.Schema;

namespace LogicBuilder.Expressions.Utils.Tests.Data
{
    [Table("CourseAssignment")]
    public class CourseAssignment : BaseDataClass
    {
        public int InstructorID { get; set; }
        public int CourseID { get; set; }
        public Instructor Instructor { get; set; }
        public Course Course { get; set; }
    }
}
