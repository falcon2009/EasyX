using EasyX.Data.Api.Entity;
using Organization.Share.Key;
using System;

namespace Organization.Storage.Entity
{
    public record Employee : IKey<EmployeeKey>, ICreateTrackable
    {
        public Guid OrganizationId { get; set; }
        public Guid PersonId { get; set; }
        public int PositionId { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public string CreatedById { get; set; }
        public string CreatedByName { get; set; }
        public Organization Organization { get; set; }
        public Position Position { get; set; }
        public EmployeeKey Key => new() { PersonId = PersonId, OrganizationId = OrganizationId };
    }
}
