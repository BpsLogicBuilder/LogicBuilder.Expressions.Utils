using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogicBuilder.Expressions.Utils.Tests.Data
{
    [Table("OfficeAssignment")]
    public class OfficeAssignment : BaseDataClass
    {
        [Key]
        [ForeignKey("Instructor")]
        public int InstructorID { get; set; }
        [StringLength(50)]
        [Display(Name = "Office Location")]
        public string Location { get; set; }

        
        public virtual Instructor Instructor { get; set; }
    }
}
