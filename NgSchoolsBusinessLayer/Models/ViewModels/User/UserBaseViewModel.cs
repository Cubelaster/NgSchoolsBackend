using System;

namespace NgSchoolsBusinessLayer.Models.ViewModels.User
{
    public class UserBaseViewModel
    {
        public Guid? Id { get; set; }
        public int? UserDetailsId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public string DisplayName => $"{FirstName} {LastName}";
    }
}
