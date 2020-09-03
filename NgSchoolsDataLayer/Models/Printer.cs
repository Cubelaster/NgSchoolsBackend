using NgSchoolsDataLayer.Models.BaseTypes;

namespace NgSchoolsDataLayer.Models
{
    public class Printer : DatabaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Data { get; set; }
    }
}
