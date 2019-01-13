using NgSchoolsDataLayer.Models.BaseTypes;

namespace NgSchoolsDataLayer.Models
{
    public class UploadedFile : DatabaseEntity
    {
        public int Id { get; set; }
        public string FileName { get; set; }
    }
}
