﻿using NgSchoolsDataLayer.Models.BaseTypes;
using System;
using System.Collections.Generic;

namespace NgSchoolsDataLayer.Models
{
    public class StudentRegisterEntry : DatabaseEntity
    {
        public int Id { get; set; }
        public int StudentRegisterNumber { get; set; }
        public string Notes { get; set; }
        public DateTime EntryDate { get; set; }

        public int StudentsInGroupsId { get; set; }
        public virtual StudentsInGroups StudentsInGroups { get; set; }

        public virtual ICollection<StudentRegisterEntryPrint> StudentRegisterEntryPrints { get; set; }
    }
}
