using NgSchoolsDataLayer.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace NgSchoolsDataLayer.Models.BaseTypes
{
    public abstract class DatabaseEntity
    {
        public DatabaseEntity()
        {
            this.DateCreated = DateTime.Now;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DatabaseEntityStatusEnum Status { get; set; }
    }
}
