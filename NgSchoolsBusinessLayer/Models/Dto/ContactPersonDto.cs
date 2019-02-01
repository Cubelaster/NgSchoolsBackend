namespace NgSchoolsBusinessLayer.Models.Dto
{
    public class ContactPersonDto
    {
        public int? Id { get; set; }
        public string FullName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public int? BusinessPartnerId { get; set; }
    }
}
