namespace NgSchoolsBusinessLayer.Models.Dto
{
    public class InstitutionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string InstitutionCode { get; set; }
        public string City { get; set; }
        public UserDto Principal { get; set; }
    }
}
