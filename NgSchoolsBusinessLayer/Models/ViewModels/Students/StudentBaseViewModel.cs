using Core.Utilities.Attributes;

namespace NgSchoolsBusinessLayer.Models.ViewModels.Students
{
    public class StudentBaseViewModel
    {
        public int Id { get; set; }
        [Searchable]
        public string FirstName { get; set; }
        [Searchable]
        public string LastName { get; set; }
        public string Oib { get; set; }
        public string Email { get; set; }
    }
}
