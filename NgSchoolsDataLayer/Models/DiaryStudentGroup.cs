namespace NgSchoolsDataLayer.Models
{
    public class DiaryStudentGroup
    {
        public int Id { get; set; }
        public int DiaryId { get; set; }
        public virtual Diary Diary { get; set; }
        public int StudentGroupId { get; set; }
        public virtual StudentGroup StudentGroup { get; set; }
    }
}
