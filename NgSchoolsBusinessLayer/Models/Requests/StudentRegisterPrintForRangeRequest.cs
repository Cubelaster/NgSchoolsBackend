namespace NgSchoolsBusinessLayer.Models.Requests
{
    public class StudentRegisterPrintForRangeRequest
    {
        public int BookNumber { get; set; }
        public int StudentRegisterNumberRangeFrom { get; set; }
        public int StudentRegisterNumberRangeTo { get; set; }
    }
}
