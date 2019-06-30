using NgSchoolsDataLayer.Models.BaseTypes;
using System;
using System.ComponentModel.DataAnnotations;

namespace NgSchoolsDataLayer.Models
{
    public class GoverningCouncilMember : DatabaseEntity
    {
        public GoverningCouncilMember() : base() { }

        [Key]
        public int Id { get; set; }

        [Required]
        public int GoverningCouncilId { get; set; }

        [Required]
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public string Role { get; set; }
    }
}
