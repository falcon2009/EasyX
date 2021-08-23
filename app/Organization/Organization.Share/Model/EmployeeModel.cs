using EasyX.Data.Api.Entity;
using Organization.Share.Key;
using System;

namespace Organization.Share.Model
{
    public record EmployeeModel : IKey<EmployeeKey>
    {
        public Guid? OrganizationId { get; set; }
        public Guid PersonId { get; set; }
        public int PositionId { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public string CreatedById { get; set; }
        public string CreatedByName { get; set; }
        public PositionModel Position { get; set; }
        public EmployeeKey Key => OrganizationId.HasValue ? new() { PersonId = PersonId, OrganizationId = OrganizationId.Value } : null;
    }
}
