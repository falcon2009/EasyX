using System;
using System.Collections.Generic;
using Organization.Share.Model;
using Person.Share.Model.PersonContact;

namespace OrganizatonOrc.Shared.Model
{
    public record EmployeeOrcModel
    {
        public Guid? OrganizationId { get; set; }
        public Guid? PersonId { get; set; }
        public int PositionId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public string CreatedById { get; set; }
        public string CreatedByName { get; set; }
        public PositionModel Position { get; set; }
        public List<PersonContactModel> PersonContactList { get; set; }
    }
}
