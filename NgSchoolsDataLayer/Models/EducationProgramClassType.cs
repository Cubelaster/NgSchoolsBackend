using NgSchoolsDataLayer.Models.BaseTypes;

namespace NgSchoolsDataLayer.Models
{
    public class EducationProgramClassType : DatabaseEntity
    {
        public int Id { get; set; }
        public int EducationProgramId { get; set; }
        public virtual EducationProgram EducationProgram { get; set; }
        public int ClassTypeId { get; set; }
        public virtual ClassType ClassType { get; set; }
    }
}
