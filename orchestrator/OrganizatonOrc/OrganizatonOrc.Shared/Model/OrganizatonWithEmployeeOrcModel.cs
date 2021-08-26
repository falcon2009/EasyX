using System;
using System.Collections.Generic;

namespace OrganizatonOrc.Shared.Model
{
    public class OrganizatonWithEmployeeOrcModel
    {
        public Guid? Id { get; set; }
        public string Titlte { get; set; }
        public string IdentificationNumber { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public string CreatedById { get; set; }
        public string CreatedByName { get; set; }
        public List<EmployeeOrcModel> EmployeeList { get; set; }
    }
}
